using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PenaltyGameManager : MonoBehaviour
{
    [Header("Maze Runner Components")]
    public GameObject mazeObjects;          // reference to the object parent containing all the maze elements
    public GameObject mazeWalls;            // reference to the parent object containing all the maze walls
    public GameObject playerSpawn;          // transform for the player's spawn position
    public GameObject ballSpawn;            // transform for the ball's spawn position
    public float xSpan;                     // ball spawn's x position range
    public float zSpan;                     // ball spawn's z position range
    public float timeLimit;                 // the duration of the penalty game round

    [Header("Prefabs")]
    public GameObject mazePlayer;           // player object for the maze runner round
    public GameObject mazeBall;             // ball object for the maze runner round

    [Header("UI Component")]
    public GameObject penaltyInput;         // UI element to get player's touch input
    public GameObject timerPanel;           // timer UI display for penalty game
    public TextMeshProUGUI timerText;       // timer text UI for penalty game

    [Header("Penalty Status")]
    public bool penaltyEnded = false;       // current penalty game status
    public bool playerWon = false;          // player win/lose status
    public float currentTime = 0.0f;        // penalty game countdown
    public MazePlayer currentPlayer;        // current maze player
    public MazeBall currentBall;            // current maze ball

    private void Start()
    {
        penaltyInput.SetActive(false);
        timerPanel.SetActive(false);
    }

    // penalty game initialization
    public void StartPenaltyGame()
    {
        // enable maze walls
        mazeObjects.SetActive(true);

        // disable player's gate
        GameManager.instance.playerGate.gameObject.SetActive(false);

        // spawn player
        SpawnPlayer();

        // spawn ball
        SpawnBall();

        // enable timer UI
        timerPanel.SetActive(true);

        // set initial timer
        currentTime = timeLimit;

        // enable touch input
        penaltyInput.SetActive(true);
    }

    // spawn maze player
    public void SpawnPlayer()
    {
        // spawn new soldier object
        GameObject mazeSoldier = Instantiate(mazePlayer, playerSpawn.transform);
        mazeSoldier.transform.localPosition = Vector3.zero;
        mazeSoldier.transform.rotation = Quaternion.identity;

        // setup maze player components
        currentPlayer = mazeSoldier.GetComponent<MazePlayer>();
        PenaltyGameInputManager touchInput = penaltyInput.GetComponent<PenaltyGameInputManager>();

        if (touchInput != null)
        {
            currentPlayer.InputSetup(touchInput);
        }
    }

    // spawn maze ball
    public void SpawnBall()
    {
        // randomize spawn position without touching walls
        Vector3 spawnPosition = ballSpawn.transform.localPosition;

        float currentXSpan = 0.0f;
        float currentZSpan = 0.0f;

        do
        {
            // randomize range and update based on current scale
            currentXSpan = xSpan * GameManager.instance.currentScale;
            currentZSpan = zSpan * GameManager.instance.currentScale;

            spawnPosition.x = ballSpawn.transform.localPosition.x + Random.Range(-currentXSpan, currentXSpan);
            spawnPosition.z = ballSpawn.transform.localPosition.z + Random.Range(-currentZSpan, currentZSpan);

            // set spawn position
            ballSpawn.transform.localPosition = Vector3.forward * spawnPosition.z + Vector3.right * spawnPosition.x;

            // move spawn position to the ground
            ballSpawn.transform.localPosition = new Vector3(ballSpawn.transform.localPosition.x, 0.575f, ballSpawn.transform.localPosition.z);

        } while (!ValidSpawn());

        // spawn maze ball at assigned location
        GameObject newBall = Instantiate(mazeBall, ballSpawn.transform);
        newBall.transform.localPosition = Vector3.zero;
        newBall.transform.rotation = Quaternion.identity;
        newBall.transform.localScale = Vector3.one;

        // assign as current maze ball
        currentBall = newBall.GetComponent<MazeBall>();
    }

    public bool ValidSpawn()
    {
        bool isValid = true;

        // check if any of the walls intersect with the current ball spawn position
        foreach (Transform child in mazeWalls.transform)
        {
            if (ballSpawn.GetComponent<Collider>().bounds.Intersects(child.GetComponent<Collider>().bounds))
            {
                isValid = false;
                break;
            }
        }

        return isValid;
    }

    public void TimeCountdown()
    {
        if (currentTime > 0.0f)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = 0.0f;

            // play times up audio
            AudioManager.instance.PlayDefaultAudio(AudioManager.instance.timesUp);

            // game over in a loss if time runs out
            GameOver(false);
        }

        // update timer UI
        timerText.text = Mathf.Floor(currentTime) + "s";
    }

    // end penalty game and set player win/lose status
    public void GameOver(bool winStatus)
    {
        // end penalty game
        penaltyEnded = true;

        // player won/lost the game
        playerWon = winStatus;

        // disable timer UI
        timerPanel.SetActive(false);

        // disable touch input
        penaltyInput.SetActive(false);
    }
}
