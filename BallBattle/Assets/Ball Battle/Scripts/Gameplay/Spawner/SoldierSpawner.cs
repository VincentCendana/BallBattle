using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour
{
    [Header("Scene Components")]
    public Transform playerContainer;           // container for player soldier objects
    public Transform enemyContainer;            // container for enemy soldier objects
    public GameObject playerSide;               // player's side of the field
    public GameObject enemySide;                // enemy's side of the field

    [Header("Prefabs")]
    public GameObject playerSoldier;            // prefab for the player soldier's game object
    public GameObject enemySoldier;             // prefab for the enemy soldier's game object

    // spawn player object at target position
    public void SpawnPlayer(Vector3 position)
    {
        SpawnSoldier(position, playerSoldier, playerContainer, playerSide, GameManager.instance.playerStatus);
    }

    // spawn enemy object at target position
    public void SpawnEnemy(Vector3 position)
    {
        SpawnSoldier(position, enemySoldier, enemyContainer, enemySide, GameManager.instance.enemyStatus);
    }

    // spawn soldier at target position
    public void SpawnSoldier(Vector3 position, GameObject soldierPrefab, Transform soldierContainer, GameObject soldierSide, PlayerUI energyStatus)
    {
        bool isAttacker;

        // determine if the soldier to spawn is an attacker/defender based on the current attacker side
        if (GameManager.instance.ballSpawner.currentAttacker == soldierSide)
        {
            isAttacker = true;
        }
        else
        {
            isAttacker = false;
        }

        // reduce energy by spawn cost if there is enough energy points
        // otherwise, cancel the spawn
        if (energyStatus.ValidSpawn(isAttacker))
        {
            energyStatus.SpawnCost(isAttacker);
        }
        else
        {
            return;
        }

        // spawn new soldier object
        GameObject newSoldier = Instantiate(soldierPrefab, soldierContainer);
        newSoldier.transform.position = position;
        newSoldier.transform.rotation = soldierSide.transform.rotation;

        // set soldier to the field's ground
        Vector3 spawnPosition = newSoldier.transform.localPosition;
        spawnPosition.y = 0.0f;
        newSoldier.transform.localPosition = spawnPosition;

        Soldier soldierComp = newSoldier.GetComponent<Soldier>();

        // start activation process for spawned soldier
        // set current behavior script depending on if the soldier is attacking/defending
        if (isAttacker)
        {
            StartCoroutine(soldierComp.InitialSpawn(soldierComp.attackBehavior));
        }
        else
        {
            StartCoroutine(soldierComp.InitialSpawn(soldierComp.defendBehavior));
        }

        // set defender component's original position
        if (newSoldier.GetComponent<DefendingSoldier>() != null)
        {
            newSoldier.GetComponent<DefendingSoldier>().originalPosition = spawnPosition;
        }

        // play spawn audio
        AudioManager.instance.PlayDefaultAudio(AudioManager.instance.soldierSpawn);
    }
}
