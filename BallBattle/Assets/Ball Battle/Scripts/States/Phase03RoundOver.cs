using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase03RoundOver : GameState
{
    public override IEnumerator Start()
    {
        // disable player touch input
        GameManager.instance.inputManager.SetTouchInputStatus(false);

        // disable energy point UI panel
        GameManager.instance.gamePanels.SetActive(false);

        // announce round's winner
        if (GameManager.instance.roundWinner == GameWinner.player)
        {
            GameManager.instance.announcementText.text = "Round Over" + "\n" + "Player Wins";
        }
        else if (GameManager.instance.roundWinner == GameWinner.enemy)
        {
            GameManager.instance.announcementText.text = "Round Over" + "\n" + "Enemy Wins";
        }
        else
        {
            GameManager.instance.announcementText.text = "Draw";
        }

        yield return new WaitForSeconds(2.0f);

        // reset announcement text
        GameManager.instance.announcementText.text = "";

        // end match if current round has reached the match's total round, otherwise start a new round
        if (GameManager.instance.currentRound >= GameManager.instance.totalRound)
        {
            GameManager.instance.SetState(new Phase04MatchOver());
        }
        else
        {
            GameManager.instance.SetState(new Phase01RoundStart());
        }

        yield break;
    }
}
