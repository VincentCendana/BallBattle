using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase01RoundStart : GameState
{
    public override IEnumerator Start()
    {
        // set initial values
        GameManager.instance.roundText.text = "";
        GameManager.instance.announcementText.text = "";

        // update round icons
        GameManager.instance.gameMenu.UpdateRoundIcons();

        // disable maze walls
        GameManager.instance.penaltyManager.mazeObjects.SetActive(false);

        // disable player touch input
        GameManager.instance.inputManager.SetTouchInputStatus(false);

        // disable energy point UI panel
        GameManager.instance.gamePanels.SetActive(false);

        // destroy all soldiers if exist
        GameManager.instance.DestroySoldiers();

        // destroy ball if exist 
        GameManager.instance.DestroyBall();

        // initial game start delay
        yield return new WaitForSeconds(1.0f);

        // update current round
        yield return GameManager.instance.UpdateRound();

        // start game round
        GameManager.instance.SetState(new Phase02GameRound());

        yield break;
    }
}
