using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendingSoldier : SoldierBehavior
{
    [Header("Defender Component")]
    public float returnSpeed;                       // soldier speed when returning to original position
    public Vector3 originalPosition;                // soldier's original spawn position
    public Soldier currentTarget = null;            // defender's current chase target

    public override void MoveSoldier()
    {
        // stop soldier if inactive
        if (!soldier.isActive)
        {
            return;
        }

        // enable soldier collision if inactive
        if (!GetComponent<Collider>().enabled)
        {
            soldier.SetColliderStatus(true);
        }

        // Defender chase target if detected
        if (currentTarget != null)
        {
            // enable direction indicator if disabled
            if (!soldier.directionIndicator.activeSelf)
            {
                soldier.directionIndicator.SetActive(true);

                // play running animation
                soldier.soldierAnimation.PlayAnimation(soldier.soldierAnimation.runningClip);
            }

            // disable detection range if active
            if (soldier.detectionRange.activeSelf)
            {
                soldier.detectionRange.SetActive(false);
            }

            // chase attacker holding the ball if it is within the defender's detection range
            Vector3 targetPosition = currentTarget.transform.position;
            targetPosition.y = transform.position.y;

            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * normalSpeed * GameManager.instance.currentScale * Time.deltaTime;
            transform.LookAt(targetPosition);

            // stop chasing if the target is deactivated
            if (!currentTarget.isActive)
            {
                currentTarget = null;
            }
        }
        else
        {
            // play idle clip if defender is not moving
            if (soldier.soldierAnimation.currentClip != soldier.soldierAnimation.idleClip)
            {
                soldier.soldierAnimation.PlayAnimation(soldier.soldierAnimation.idleClip);
            }

            // enable detection range if inactive
            if (!soldier.detectionRange.activeSelf)
            {
                soldier.detectionRange.SetActive(true);
            }
        }
    }

    // deactivate the attacker currently holding the ball when caught
    public void CaptureAttacker(AttackingSoldier attackSoldier)
    {
        // disable chase movement
        currentTarget = null;

        // play catch animation
        soldier.soldierAnimation.PlayAnimation(soldier.soldierAnimation.tackleClip);

        // capture attacker soldier
        attackSoldier.CatchSoldier();

        // deactivate the defender soldier
        StartCoroutine(DeactivateSoldier());
    }

    // deactivate soldier for a period of time and move it to its original position
    protected override IEnumerator DeactivateSoldier()
    {
        // return to original position
        StartCoroutine(ReturnToOrigin());

        // deactivate soldier
        yield return base.DeactivateSoldier();

        yield break;
    }

    // defender returns to its original position when deactivated
    private IEnumerator ReturnToOrigin()
    {
        // move to original position
        while (Vector3.Distance(transform.localPosition, originalPosition) > (0.1f * GameManager.instance.currentScale))
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition, 
                originalPosition, 
                returnSpeed * GameManager.instance.currentScale * Time.deltaTime);

            yield return null;
        }

        transform.localPosition = originalPosition;

        // in rush time mode, soldier is immediately reactivated after reaching its original position
        if (GameManager.instance.gameTimer.rushTime && !soldier.isActive && !GameManager.instance.gameTimer.roundEnded)
        {
            soldier.SetActivationStatus(true);
        }

        yield break;
    }
}
