using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase02GameRound : GameState
{
    public override IEnumerator Start()
    {
        // reset current timer 
        GameManager.instance.gameTimer.SetInitialTimer();

        // ball spawner setup
        GameManager.instance.ballSpawner.SetBallSpawner();

        // reset player and enemy's energy bars
        GameManager.instance.playerStatus.ResetEnergy();
        GameManager.instance.enemyStatus.ResetEnergy();

        // enable energy point UI panel
        GameManager.instance.gamePanels.SetActive(true);

        // timer start delay
        yield return new WaitForSeconds(1.0f);

        // enable player touch input
        GameManager.instance.inputManager.SetTouchInputStatus(true);

        // timer countdown
        while (!GameManager.instance.gameTimer.roundEnded)
        {
            // update timer
            GameManager.instance.gameTimer.UpdateTimer();

            // player and enemy's energy bar refills
            GameManager.instance.playerStatus.FillEnergy();
            GameManager.instance.enemyStatus.FillEnergy();

            yield return null;
        }

        // go to round over state
        GameManager.instance.SetState(new Phase03RoundOver());

        yield break;
    }
}
