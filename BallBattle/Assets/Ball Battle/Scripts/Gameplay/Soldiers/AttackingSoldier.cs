using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingSoldier : SoldierBehavior
{
    [Header("Attacker Component")]
    public float carryingSpeed;                     // soldier movement speed when carrying ball
    public float ballSpeed;                         // soldier ball passing speed

    [Header("Ball Hold Component")]
    public Transform ballHoldTransform;             // ball position when held by the soldier

    public override void MoveSoldier()
    {
        // stop soldier if inactive
        if (!soldier.isActive)
        {
            return;
        }

        // enable direction indicator if disabled
        if (!soldier.directionIndicator.activeSelf)
        {
            soldier.directionIndicator.SetActive(true);

            // play running animation
            soldier.soldierAnimation.PlayAnimation(soldier.soldierAnimation.runningClip);
        }

        // attacker movement behaviors
        if (GameManager.instance.currentBall.ballHolder == null)
        {
            // enable soldier collision
            if (!GetComponent<Collider>().enabled)
            {
                soldier.SetColliderStatus(true);
            }

            // chase ball if no soldiers are currently holding it
            Vector3 targetPosition = GameManager.instance.currentBall.transform.position;
            targetPosition.y = transform.position.y;

            Vector3 direction = (targetPosition -transform.position).normalized;
            transform.position += direction * normalSpeed * GameManager.instance.currentScale * Time.deltaTime;
            transform.LookAt(targetPosition);
        }
        else if (GameManager.instance.currentBall.ballHolder == soldier)
        {
            // enable soldier collision
            if (!GetComponent<Collider>().enabled)
            {
                soldier.SetColliderStatus(true);
            }

            // move to target (defender's gate) if soldier is currently holding the ball
            Vector3 targetPosition = GameManager.instance.targetGate.transform.position;
            targetPosition.y = transform.position.y;

            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.LookAt(targetPosition);

            // adjust movement speed based on rush time status when holding the ball
            if (GameManager.instance.gameTimer.rushTime)
            {
                transform.position += direction * normalSpeed * GameManager.instance.currentScale * Time.deltaTime;
            }
            else
            {
                transform.position += direction * carryingSpeed * GameManager.instance.currentScale * Time.deltaTime;
            }

            // update ball transform to follow the soldier
            if (GameManager.instance.currentBall != null)
            {
                // move ball position
                GameManager.instance.currentBall.MoveBall(ballHoldTransform.position);

                // rotate ball to follow the soldier's rotation
                GameManager.instance.currentBall.transform.rotation = ballHoldTransform.rotation;
            }
        }
        else
        {
            // disable soldier collision
            if (GetComponent<Collider>().enabled)
            {
                soldier.SetColliderStatus(false);
            }

            // set move direction based on the current target fence
            Quaternion rotation = Quaternion.identity;

            if (GameManager.instance.targetFence == GameManager.instance.playerFence)
            {
                rotation = GameManager.instance.ballSpawner.enemySide.transform.rotation;
            }
            else
            {
                rotation = GameManager.instance.ballSpawner.playerSide.transform.rotation;
            }

            // move straight to opponent's end of the field if no ball is available to chase/hold
            transform.rotation = rotation;
            transform.position += transform.forward * normalSpeed * GameManager.instance.currentScale * Time.deltaTime;

            // destroy soldier object if it reached the end of the opponent's field
            if ((GameManager.instance.targetFence == GameManager.instance.playerFence && 
                transform.localPosition.z - GameManager.instance.targetFence.transform.localPosition.z < (0.1f * GameManager.instance.currentScale))
                ||
                (GameManager.instance.targetFence == GameManager.instance.enemyFence &&
                GameManager.instance.targetFence.transform.localPosition.z - transform.localPosition.z < (0.1f * GameManager.instance.currentScale))
                )
            {
                soldier.isActive = false;
                StartCoroutine(SoldierHitFence());
            }
        }
    }

    // soldier reached end of opponent's field without carrying a ball
    private IEnumerator SoldierHitFence()
    {
        // play hit fence animation
        soldier.soldierAnimation.PlayAnimation(soldier.soldierAnimation.headerClip);

        yield return new WaitForSeconds(1.0f);

        // spawn hit fence particle
        GameManager.instance.particleSpawner.SpawnParticle(
                GameManager.instance.particleSpawner.fenceParticle,
                transform.position,
                GameManager.instance.particleSpawner.particleSpawnParent,
                1.0f,
                0.5f
                );

        // play reach fence audio
        AudioManager.instance.PlayDefaultAudio(AudioManager.instance.reachFence);

        Destroy(this.gameObject);

        yield break;
    }

    // set ball to follow soldier
    public void HoldBall()
    {
        // enable ball holder highlight
        soldier.ballHolderIndicator.SetActive(true);

        // set current ball holder
        GameManager.instance.currentBall.ballHolder = soldier;

        // play carry ball audio
        AudioManager.instance.PlayDefaultAudio(AudioManager.instance.carryBall);
    }

    // soldier get caught by opponent's soldier
    public void CatchSoldier()
    {
        // pass ball if the soldier is carrying a ball
        if (GameManager.instance.currentBall.ballHolder == soldier)
        {
            // find nearest attacker soldier to pass the ball to
            AttackingSoldier passSoldier = null;
            float closestDistance = Mathf.Infinity;

            foreach (var fieldSoldiers in FindObjectsOfType<AttackingSoldier>())
            {
                // find other attacker soldiers closest to the current ball holder
                if (Vector3.Distance(fieldSoldiers.transform.position, transform.position) < closestDistance &&
                    fieldSoldiers != this &&
                    fieldSoldiers.GetComponent<Soldier>().currentBehavior == fieldSoldiers &&
                    fieldSoldiers.GetComponent<Soldier>().isActive)
                {
                    passSoldier = fieldSoldiers;
                    closestDistance = Vector3.Distance(fieldSoldiers.transform.position, transform.position);
                }
            }

            // pass ball to nearest attacker if available
            if (passSoldier != null)
            {
                StartCoroutine(PassBall(passSoldier));

                // set soldier to inactive for a period of time
                StartCoroutine(DeactivateSoldier());
            }
            else
            {
                // play trip animation
                soldier.soldierAnimation.PlayAnimation(soldier.soldierAnimation.tripClip);

                // play caught audio
                AudioManager.instance.PlayDefaultAudio(AudioManager.instance.catchAttacker);

                // defending player wins the round if no other attackers are available to pass
                if (GameManager.instance.targetGate == GameManager.instance.playerGate)
                {
                    GameManager.instance.RoundOver(GameWinner.player);
                }
                else if (GameManager.instance.targetGate == GameManager.instance.enemyGate)
                {
                    GameManager.instance.RoundOver(GameWinner.enemy);
                }
            }
        }
    }

    private IEnumerator PassBall(AttackingSoldier targetSoldier)
    {
        if (GameManager.instance.currentBall == null)
        {
            yield break;
        }

        // play pass animation
        soldier.soldierAnimation.PlayAnimation(soldier.soldierAnimation.passClip);

        // play pass ball audio
        AudioManager.instance.PlayDefaultAudio(AudioManager.instance.passBall);

        // get ball parameters
        Ball passedBall = GameManager.instance.currentBall;
        Transform ballTransform = passedBall.transform;
        Vector3 originalPosition = ballTransform.position;

        // set ball's holder to nothing
        passedBall.ballHolder = null;

        // set target position to the target soldier's ball hold position
        Vector3 targetPosition = targetSoldier.ballHoldTransform.position;
        targetPosition.y = ballTransform.position.y;

        // move ball to target position
        while (passedBall != null && Vector3.Distance(ballTransform.position , targetPosition) > 0.1f * (GameManager.instance.currentScale))
        {
            // animate ball rolling
            passedBall.RollAnimation();

            // move ball to target position
            ballTransform.position = Vector3.MoveTowards(
                ballTransform.position, 
                targetPosition, 
                ballSpeed * GameManager.instance.currentScale *  Time.deltaTime);

            // rotate ball to look at the target position
            ballTransform.LookAt(targetPosition);

            yield return null;
        }

        yield break;
    }
}
