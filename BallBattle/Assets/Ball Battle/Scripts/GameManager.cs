using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Static Information")]
    public static GameManager instance;         // instance of game manager

    [Header("Game Components")]
    public InputManager inputManager;           // game input component
    public GameTimer gameTimer;                 // game timer component
    public SoldierSpawner soldierSpawner;       // soldier / game pieces spawner component
    public BallSpawner ballSpawner;             // ball object spawner component
    public ParticleSpawner particleSpawner;     // particle effects spawner component
    public PenaltyGameManager penaltyManager;   // penalty game (maze runner) manager component
    public GameMenu gameMenu;                   // game scene UIs manager component

    [Header("Field Components")]
    public GameObject gameField;                // current game field objects
    public GameObject centerLine;               // game field center line
    public GameObject playerGate;               // player field's gate transform
    public GameObject enemyGate;                // enemy field's gate transform
    public GameObject playerFence;              // player field's fence transform
    public GameObject enemyFence;               // enemy field's fence transform
    public GameObject targetGate;               // current target/defender's gate to place the ball in
    public GameObject targetFence;               // current target/defender's fence
    public Ball currentBall;                    // reference to the ball object
    public float currentScale;                  // current game field's scale (to adjust soldier/ball position and speed in the field)

    [Header("Scene Information")]
    public string menuScene;                    // scene name for the main menu scene
    public string mainScene;                    // scene name for the default game scene
    public string arScene;                      // scene name for the AR version
    private string currentScene;                // current scene name

    [Header("UI Components")]
    public TextMeshProUGUI roundText;           // UI text displaying the current round
    public TextMeshProUGUI announcementText;    // UI text announcing the next game round
    public GameObject gamePanels;               // UI panels to display gameplay status (timer and energy points)
    public TextMeshProUGUI playerText;          // UI to display the player's current role
    public TextMeshProUGUI enemyText;           // UI to display the enemy's current role

    [Header("Game Status")]
    public GameState currentState;              // current state/phase of the game
    public int totalRound;                      // the total amount of round per match
    public int currentRound;                    // the current round of the match
    public float currentEnergyRate;             // energy regeneration rate per second
    public GameWinner roundWinner;              // winner of the round
    public int playerWinCount = 0;              // total round won by player per match
    public int enemyWinCount = 0;               // total round won by enemy per match

    [Header("Soldier Spawn")]
    public float spawnTime;                     // duration before a spawned soldier is activated
    public float normalEnergyRate;              // energy regeneration rate in normal mode
    public float rushEnergyRate;                // energy regeneration rate in rush time mode
    public float attackerCost;                  // energy cost to spawn attacker soldiers
    public float defenderCost;                  // energy cost to spawn defender soldiers

    [Header("Player Status")]
    public PlayerUI playerStatus;               // reference to the player's status component 
    public PlayerUI enemyStatus;                // reference to the enemy's status component

    private void Awake()
    {
        // ensure only one instance of game manager is instantiated
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        // set current scene name
        currentScene = SceneManager.GetActiveScene().name;

        // set current scale
        currentScale = gameField.transform.localScale.x;
    }

    private void Start()
    {
        // start match
        SetState(new Phase01RoundStart());
    }

    // change current state/phase of the game
    public void SetState(GameState newState)
    {
        currentState = newState;
        StartCoroutine(currentState.Start());
    }

    // announce current round
    public IEnumerator UpdateRound()
    {
        currentRound++;

        // announce current round
        announcementText.text = "ROUND - " + currentRound.ToString();

        yield return new WaitForSeconds(2.0f);

        // update UI to start match
        announcementText.text = "";
        roundText.text = "Round - " + currentRound.ToString();

        yield break;
    }

    public void UpdateRoleText()
    {
        if (targetGate == playerGate)
        {
            playerText.text = "Player (Defender)";
            enemyText.text = "Enemy - AI (Attacker)";
        }
        else
        {
            playerText.text = "Player (Attacker)";
            enemyText.text = "Enemy - AI (Defender)";
        }
    }

    // destroy all soldier objects in the scene
    public void DestroySoldiers()
    {
        foreach (var soldier in FindObjectsOfType<Soldier>())
        {
            Destroy(soldier.gameObject);
        }
    }

    // destroy current ball object if exist
    public void DestroyBall()
    {
        if (currentBall != null)
        {
            Destroy(currentBall.gameObject);
        }
    }

    // set as round over when a round has ended
    public void RoundOver(GameWinner winner)
    {
        // stop game
        gameTimer.roundEnded = true;

        // deactivate and stop all soldiers
        foreach (var soldier in FindObjectsOfType<Soldier>())
        {
            soldier.SetRoundEnd();
        }

        // set round winner
        roundWinner = winner;

        // update total win(s) per match
        if (winner == GameWinner.player)
        {
            playerWinCount++;
        }
        else if (winner == GameWinner.enemy)
        {
            enemyWinCount++;
        }

        // update round win icons
        gameMenu.UpdateRoundIcons();
    }
}

// enum to identify a round's winner
public enum GameWinner
{
    draw,
    player,
    enemy
}
