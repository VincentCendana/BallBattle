using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBall : MonoBehaviour
{
    [Header("Ball Components")]
    public Transform ballObject;            // ball 3D model transform
    public float rollSpeed;                 // speed of the ball rolling animation

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject == GameManager.instance.enemyGate)
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

            // penalty game won
            GameManager.instance.penaltyManager.GameOver(true);

            // destroy ball
            Destroy(this.gameObject);
        }
    }

    // rotate ball model
    public void RollAnimation()
    {
        Vector3 rotateDirection = Vector3.zero;
        rotateDirection.x += rollSpeed * GameManager.instance.currentScale * Time.deltaTime;
        ballObject.Rotate(rotateDirection);
    }
}
