using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase06PenaltyOver : GameState
{
    public override IEnumerator Start()
    {
        // announce penalty game's end
        if (GameManager.instance.penaltyManager.playerWon)
        {
            GameManager.instance.announcementText.text = "GOAL";
        }
        else
        {
            GameManager.instance.announcementText.text = "Time's Up";
        }

        yield return new WaitForSeconds(2.0f);

        // reset announcement text
        GameManager.instance.announcementText.text = "";

        // display game over panels with the final breakdown
        if (GameManager.instance.penaltyManager.playerWon)
        {
            GameManager.instance.gameMenu.DisplayGameOverPanel("Player Wins");
        }
        else
        {
            GameManager.instance.gameMenu.DisplayGameOverPanel("Enemy Wins");
        }

        yield break;
    }
}
