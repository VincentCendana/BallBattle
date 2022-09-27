using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Components")]
    public Transform ballObject;            // ball 3D model transform
    public float rollSpeed;                 // speed of the ball rolling animation

    [Header("Ball Status")]
    public Soldier ballHolder = null;       // reference to the attacking soldier currently holding the ball

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject == GameManager.instance.targetGate)
        {
            // play goal audio
            AudioManager.instance.PlayDefaultAudio(AudioManager.instance.goal);

            // spawn goal particles
            GameManager.instance.particleSpawner.SpawnParticle(
                GameManager.instance.particleSpawner.goalParticle,
                transform.position,
                GameManager.instance.particleSpawner.particleSpawnParent,
                1.0f,
                0.35f
                );

            // set round over with winner based on the opponent of the target gate's owner
            if (GameManager.instance.targetGate == GameManager.instance.playerGate)
            {
                GameManager.instance.RoundOver(GameWinner.enemy);
            }
            else if (GameManager.instance.targetGate == GameManager.instance.enemyGate)
            {
                GameManager.instance.RoundOver(GameWinner.player);
            }

            // destroy ball
            Destroy(this.gameObject);
        }
    }

    // move ball and add rolling animation
    public void MoveBall(Vector3 targetPosition)
    {
        // move to target position
        transform.position = targetPosition;

        // rotate ball
        RollAnimation();
    }

    // rotate ball model
    public void RollAnimation()
    {
        Vector3 rotateDirection = Vector3.zero;
        rotateDirection.x += rollSpeed * GameManager.instance.currentScale * Time.deltaTime;
        ballObject.Rotate(rotateDirection);
    }
}
