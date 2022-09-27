using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI Component")]
    public GameObject timerPanel;                   // UI panel displaying the current timer
    public TextMeshProUGUI timerText;               // UI text displaying the amount of time left

    [Header("Timer Parameters")]
    public float initialTimer;                      // amount of time at the start of the game

    [Header("Current Status")]  
    public float currentTimer;                      // current amount of time left
    public bool roundEnded = false;                 // current round status
    public bool rushTime = false;                   // timer is in the last 15 seconds (rush time mode)

    private void Start()
    {
        timerPanel.SetActive(false);
    }

    // reduce the amount of time remaining and update the timer UI
    public void UpdateTimer()
    {
        // reduce timer if has not reached 0
        if (currentTimer > 0.0f)
        {
            currentTimer -= Time.deltaTime;

            // activate rush time if timer is in its last 15 seconds
            if (!rushTime && currentTimer <= 15.0f)
            {
                RushTimeActivation(true);
            }
        }
        else
        {
            currentTimer = 0.0f;

            // timer runs out
            TimerOut();
        }

        // update timer UI
        timerText.text = Mathf.Floor(currentTimer) + "s";
    }

    // reset and display current timer
    public void SetInitialTimer()
    {
        // update round status
        roundEnded = false;
        RushTimeActivation(false);

        // reset the amount of time remaining
        currentTimer = initialTimer;

        // display timer panel
        timerPanel.SetActive(true);

        // update timer UI
        timerText.text = currentTimer.ToString("F0") + "s";
    }

    // activate rush time mode:
    // - increase energy activation rate
    // - defenders now become activated when it returned to its original position
    // - attackers will not slow down while carrying the ball
    public void RushTimeActivation(bool isTrue)
    {
        // set rush time status
        rushTime = isTrue;

        // adjust energy activation rate
        if (rushTime)
        {
            GameManager.instance.currentEnergyRate = GameManager.instance.rushEnergyRate;
        }
        else
        {
            GameManager.instance.currentEnergyRate = GameManager.instance.normalEnergyRate;
        }
    }

    // function when the game timer ran out before the round ends
    public void TimerOut()
    {
        // play times up audio
        AudioManager.instance.PlayDefaultAudio(AudioManager.instance.timesUp);

        roundEnded = true;

        // set game over as draw
        GameManager.instance.RoundOver(GameWinner.draw);
    }
}
