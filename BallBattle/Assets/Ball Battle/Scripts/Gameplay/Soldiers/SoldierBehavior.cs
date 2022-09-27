using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBehavior : MonoBehaviour
{
    [Header("Soldier Components")]
    public Soldier soldier;                     // soldier status component
    public float reactivateTime;                // duration to reactivate soldier
    public float normalSpeed;                   // soldier's normal movement speed

    [Header("Soldier Status")]
    public bool movementStatus = false;         // movement script status 

    public virtual void MoveSoldier()
    {

    }

    // start movement behavior and continue moving until the behavior script is deactivated
    public IEnumerator StartSoldierMovement()
    {
        movementStatus = true;

        while (movementStatus)
        {
            MoveSoldier();

            yield return null;
        }

        yield break;
    }

    // deactivate soldier for a period of time
    protected virtual IEnumerator DeactivateSoldier()
    {
        // deactivate soldier temporarily
        soldier.SetActivationStatus(false);

        // spawn regeneration particle
        GameManager.instance.particleSpawner.SpawnParticle(
                GameManager.instance.particleSpawner.regenerationParticle,
                transform.position,
                gameObject.transform,
                reactivateTime,
                0.75f
                );

        // delay before soldier is reactivated
        yield return new WaitForSeconds(reactivateTime);

        // reactivate soldier if round has not ended
        if (!GameManager.instance.gameTimer.roundEnded)
        {
            soldier.SetActivationStatus(true);
        }

        yield break;
    }
}
