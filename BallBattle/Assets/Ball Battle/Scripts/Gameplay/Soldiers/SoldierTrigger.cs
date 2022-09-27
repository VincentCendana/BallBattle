using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierTrigger : MonoBehaviour
{
    [Header("Soldier Components")]
    public Soldier soldier;                     // reference to the soldier's script

    private void OnTriggerEnter(Collider other)
    {
        if (soldier.currentBehavior == soldier.attackBehavior && GameManager.instance.currentBall != null)
        {
            // hold ball and move to target gate when the ball enters the trigger with no other attackers carrying it
            if (other.gameObject == GameManager.instance.currentBall.gameObject && GameManager.instance.currentBall.ballHolder == null)
            {
                soldier.attackBehavior.HoldBall();
            }
        }
        else if (soldier.currentBehavior == soldier.defendBehavior && soldier.defendBehavior.currentTarget != null)
        {
            // capture the attacker carrying the ball if they enter a defender's trigger
            if (other.gameObject == soldier.defendBehavior.currentTarget.gameObject)
            {
                soldier.defendBehavior.CaptureAttacker(GameManager.instance.currentBall.ballHolder.attackBehavior);
            }
        }
    }
}
