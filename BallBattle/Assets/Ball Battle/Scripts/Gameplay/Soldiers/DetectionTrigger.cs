using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionTrigger : MonoBehaviour
{
    [Header("Soldier Components")]
    public Soldier soldier;                     // reference to the soldier's script

    private void OnTriggerEnter(Collider other)
    {
        // cancel trigger detection if no attacker is carrying the ball or if the soldier is not a defender
        if (GameManager.instance.currentBall.ballHolder == null || soldier.currentBehavior != soldier.defendBehavior)
        {
            return;
        }

        // start chasing the attacker carrying the ball if detected and if the defender currently has no target
        if (other.gameObject == GameManager.instance.currentBall.ballHolder.gameObject && soldier.defendBehavior.currentTarget == null)
        {
            soldier.defendBehavior.currentTarget = other.gameObject.GetComponent<Soldier>();
        }
    }
}
