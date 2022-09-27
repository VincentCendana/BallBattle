using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase04MatchOver : GameState
{
    public override IEnumerator Start()
    {
        // display game over panels with the final breakdown
        if (GameManager.instance.playerWinCount > GameManager.instance.enemyWinCount)
        {
            GameManager.instance.gameMenu.DisplayGameOverPanel("Player Wins");
        }
        else if (GameManager.instance.playerWinCount < GameManager.instance.enemyWinCount)
        {
            GameManager.instance.gameMenu.DisplayGameOverPanel("Enemy Wins");
        }
        else
        {
            // announce draw result
            GameManager.instance.announcementText.text = "Match Over" + "\n" + "Draw";

            yield return new WaitForSeconds(2.0f);

            // reset announcement text
            GameManager.instance.announcementText.text = "";

            // go to penalty round
            GameManager.instance.SetState(new Phase05PenaltyGame());
        }

        yield break;
    }
}
