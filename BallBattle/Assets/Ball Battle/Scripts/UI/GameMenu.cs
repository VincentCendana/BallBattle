using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject pausePanel;               // pause menu display UI
    public GameObject gameOverPanel;            // game over UI displayed when the match is over
    public TextMeshProUGUI winnerText;          // game over UI text showing the match winner
    public TextMeshProUGUI playerScoreText;     // game over UI text displaying the amount of rounds won by the player
    public TextMeshProUGUI enemyScoreText;      // game over UI text displaying the amount of rounds won by the enemy
    public Image[] playerRoundWins;             // UI image representing the amount of rounds the player has won
    public Image[] enemyRoundWins;              // UI image representing the amount of rounds the enemy has won

    private void Start()
    {
        // ensure time is running at the start
        Time.timeScale = 1.0f;

        // disable UI panels
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    // pause current game and display pause menu
    public void PauseGame()
    {
        // stop game time
        Time.timeScale = 0.0f;

        // display pause UI
        pausePanel.SetActive(true);
    }

    // resume game from paused state
    public void ResumeGame()
    {
        // resume game time
        Time.timeScale = 1.0f;

        // hides pause UI
        pausePanel.SetActive(false);
    }

    // reload current scene
    public void RestartLevel()
    {
        // resume game time
        Time.timeScale = 1.0f;

        // reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // return to main menu
    public void ReturnToMenu()
    {
        // resume game time
        Time.timeScale = 1.0f;

        // return to main menu
        SceneManager.LoadScene(GameManager.instance.menuScene);
    }

    // display game over menu
    public void DisplayGameOverPanel(string winner)
    {
        // stop game time
        Time.timeScale = 0.0f;

        // set UI texts
        winnerText.text = winner;
        playerScoreText.text = GameManager.instance.playerWinCount.ToString();
        enemyScoreText.text = GameManager.instance.enemyWinCount.ToString();

        // display game over UI
        gameOverPanel.SetActive(true);
    }

    // update game round wins UI
    public void UpdateRoundIcons()
    {
        // enable player win icons up to the number of the rounds won by the player
        for (int i = 0; i < playerRoundWins.Length; i++)
        {
            if (i < GameManager.instance.playerWinCount)
            {
                playerRoundWins[i].enabled = true;
            }
            else
            {
                playerRoundWins[i].enabled = false;
            }
        }

        // enable enemy win icons up to the number of the rounds won by the enemy
        for (int i = 0; i < enemyRoundWins.Length; i++)
        {
            if (i < GameManager.instance.enemyWinCount)
            {
                enemyRoundWins[i].enabled = true;
            }
            else
            {
                enemyRoundWins[i].enabled = false;
            }
        }
    }
}
