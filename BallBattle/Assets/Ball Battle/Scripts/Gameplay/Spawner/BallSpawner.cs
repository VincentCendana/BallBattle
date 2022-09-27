using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Spawn Parameters")]
    public GameObject ballPrefab;               // ball object to spawn
    public GameObject ballParent;               // parent transform to spawn the ball in
    public GameObject ballObject;               // currently spawned ball

    [Header("Spawn Positions")]
    public GameObject playerSide;               // player side of the field
    public GameObject enemySide;                // enemy side of the field
    public GameObject currentAttacker = null;   // current attacker's side of the field
    public float xSpan;                         // range of x position from the spawn side's center
    public float zSpan;                         // range of z position from the spawn side's center

    public void SetBallSpawner()
    {
        // destroy previous ball object if it exist
        if (ballObject != null)
        {
            Destroy(ballObject);
        }

        // set current attacker's side to determine ball spawn location, target gate, and target fence
        // 1st match player gets to be the attacker
        // in the next rounds the attacker's side switches
        if (currentAttacker == null)
        {
            currentAttacker = playerSide;
            GameManager.instance.targetGate = GameManager.instance.enemyGate;
            GameManager.instance.targetFence = GameManager.instance.enemyFence;
        }
        else if (currentAttacker == enemySide)
        {
            currentAttacker = playerSide;
            GameManager.instance.targetGate = GameManager.instance.enemyGate;
            GameManager.instance.targetFence = GameManager.instance.enemyFence;
        }
        else if (currentAttacker == playerSide)
        {
            currentAttacker = enemySide;
            GameManager.instance.targetGate = GameManager.instance.playerGate;
            GameManager.instance.targetFence = GameManager.instance.playerFence;
        }

        // update player and enemy's role texts
        GameManager.instance.UpdateRoleText();

        // spawn ball object
        SpawnBall();
    }

    public void SpawnBall()
    {
        // update field span
        float currentXSpan = xSpan * GameManager.instance.currentScale;
        float currentZSpan = zSpan * GameManager.instance.currentScale;

        // get spawn position (random x and z position based on the assigned range)
        Vector3 spawnPosition = currentAttacker.transform.position;
        spawnPosition.x += Random.Range(-currentXSpan, currentXSpan);
        spawnPosition.z += Random.Range(-currentZSpan, currentZSpan);

        // spawn ball object at spawn position
        ballObject = Instantiate(ballPrefab, ballParent.transform);
        ballObject.transform.position = spawnPosition;
        ballObject.transform.rotation = Quaternion.identity;

        // snap y position to the ground
        Vector3 spawnLocalPosition = ballObject.transform.localPosition;
        spawnLocalPosition.y = 0.0f;
        ballObject.transform.localPosition = spawnLocalPosition;

        // set game's current ball
        GameManager.instance.currentBall = ballObject.GetComponent<Ball>();
    }
}
