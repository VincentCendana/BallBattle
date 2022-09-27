using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayerTrigger : MonoBehaviour
{
    [Header("Player Components")]
    public MazePlayer mazePlayer;           // player's movement component

    private void OnTriggerEnter(Collider other)
    {
        // set maze ball to be carried by the player if it entered the trigger
        if (other.gameObject == GameManager.instance.penaltyManager.currentBall.gameObject)
        {
            MazeBall ballComp = other.GetComponent<MazeBall>();
            mazePlayer.heldBall = ballComp;

            // play carry ball audio
            AudioManager.instance.PlayDefaultAudio(AudioManager.instance.carryBall);
        }
    }
}
