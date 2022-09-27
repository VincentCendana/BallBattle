using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase05PenaltyGame : GameState
{
    public override IEnumerator Start()
    {
        // destroy all soldiers if exist
        GameManager.instance.DestroySoldiers();

        // destroy ball if exist 
        GameManager.instance.DestroyBall();

        // disable energy point UI panel
        GameManager.instance.gamePanels.SetActive(false);

        // announce penalty round
        GameManager.instance.announcementText.text = "Penalty Game" + "\n" + "Maze Runner";

        yield return new WaitForSeconds(2.0f);

        // reset announcement text
        GameManager.instance.announcementText.text = "";

        // initialize penalty game
        GameManager.instance.penaltyManager.StartPenaltyGame();

        // penalty game phase
        while (!GameManager.instance.penaltyManager.penaltyEnded)
        {
            // timer countdown
            GameManager.instance.penaltyManager.TimeCountdown();

            // player movement input
            GameManager.instance.penaltyManager.currentPlayer.MovementInput();

            yield return null;
        }

        // end penalty game and announce result
        GameManager.instance.SetState(new Phase06PenaltyOver());

        yield break;
    }
}
