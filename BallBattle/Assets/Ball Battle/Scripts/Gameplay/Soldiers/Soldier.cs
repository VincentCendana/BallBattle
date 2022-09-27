using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("Soldier Parameters")]
    public SkinnedMeshRenderer[] soldierMeshes;     // soldier meshes for material changes
    public Material inactiveMaterial;               // soldier material when inactive
    public Material activeMaterial;                 // soldier material when active
    public AttackingSoldier attackBehavior;         // behavior script for attacking soldiers
    public DefendingSoldier defendBehavior;         // behavior script for defending soldiers
    public SoldierAnimation soldierAnimation;       // soldier's animation controller script
    public DetectionTrigger detectionTrigger;       // soldier's trigger to detect other soldiers when set to defender

    [Header("Soldier UI")]
    public GameObject directionIndicator;           // UI showing the soldier's movement direction
    public GameObject ballHolderIndicator;          // UI highlight for ball holders
    public GameObject detectionRange;               // UI showing the detection range for defenders

    [Header("Soldier Status")]
    public bool isActive;                           // soldier current activation status
    public SoldierBehavior currentBehavior;        // current soldier behavior

    private void Start()
    {
        // set initial status as inactive
        SetActivationStatus(false);
    }

    // set activation status and update soldier's material
    // enable/disable collider depending on the soldier's activation status
    public void SetActivationStatus(bool active)
    {
        isActive = active;

        // disable UIs
        directionIndicator.SetActive(false);
        ballHolderIndicator.SetActive(false);
        detectionRange.SetActive(false);

        // set collider status
        SetColliderStatus(isActive);

        // set activation material
        if (isActive)
        {
            for (int i = 0; i < soldierMeshes.Length; i++)
            {
                soldierMeshes[i].material = activeMaterial;
            }
        }
        else
        {
            for (int i = 0; i < soldierMeshes.Length; i++)
            {
                soldierMeshes[i].material = inactiveMaterial;
            }
        }
    }

    // enable/disable collision with other objects
    public void SetColliderStatus(bool status)
    {
        GetComponent<Collider>().enabled = status;
    }

    // initial spawn process to activate soldier
    public IEnumerator InitialSpawn(SoldierBehavior behavior)
    {     
        // set soldier's behavior
        currentBehavior = behavior;

        // set detection trigger status (only enabled if it is a defender)
        detectionTrigger.gameObject.SetActive(currentBehavior == defendBehavior);

        // set detection trigger range (35% of the field's width)
        float triggerRadius = GameManager.instance.ballSpawner.playerSide.GetComponent<Collider>().bounds.size.x;
        float initialRadius = detectionTrigger.GetComponent<CapsuleCollider>().radius;
        triggerRadius = triggerRadius * 0.35f / GameManager.instance.currentScale;
        detectionTrigger.GetComponent<CapsuleCollider>().radius = triggerRadius;

        // update detection range UI image size based on the change to the detection trigger's radius
        Vector3 currentScale = detectionRange.transform.localScale;
        currentScale.x *= triggerRadius / initialRadius;
        currentScale.z *= triggerRadius / initialRadius;

        detectionRange.transform.localScale = currentScale;

        // start soldier's movement behavior
        StartCoroutine(currentBehavior.StartSoldierMovement());

        // activation delay
        yield return new WaitForSeconds(GameManager.instance.spawnTime);

        // activate soldier
        SetActivationStatus(true);

        yield break;
    }

    // deactivate soldier and stop all its movements after the round ended
    public void SetRoundEnd()
    {      
        // stop all coroutines from the soldier script and its current behavior
        StopAllCoroutines();
        currentBehavior.StopAllCoroutines();

        // deactivate soldier
        SetActivationStatus(false);
    }
}
