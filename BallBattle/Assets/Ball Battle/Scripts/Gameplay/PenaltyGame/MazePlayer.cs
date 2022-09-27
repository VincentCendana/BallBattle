using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePlayer : MonoBehaviour
{
    [Header("UI Component")]
    public PenaltyGameInputManager penaltyInput;    // reference to the input manager

    [Header("Movement Parameters")]
    public float moveSpeed;                         // player movement speed
    private Vector3 movement;                       // player's movement values

    [Header("Player Component")]
    public Rigidbody rb;                            // player' rigidbody component
    public Transform ballCarryTransform;            // position for maze ball when carried by the player
    public SoldierAnimation soldierAnimation;       // player's animation controller script

    [Header("Player Status")]
    public MazeBall heldBall = null;                // ball currently carried by the player

    // set touch input component
    public void InputSetup(PenaltyGameInputManager input)
    {
        penaltyInput = input;
    }

    // set player movements based on touch input
    public void MovementInput()
    {
        // get touch input
        movement = Vector3.zero;
        movement.x = penaltyInput.InputHorizontal();
        movement.z = penaltyInput.InputVertical();

        // adjust movement direction based on the camera to the player's direction
        Vector3 movementDir = Vector3.zero;

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0.0f;
        camRight.y = 0.0f;
        camForward.Normalize();
        camRight.Normalize();

        movementDir = camForward * movement.z + camRight * movement.x;

        // move player based on the touch input
        rb.velocity = movementDir * moveSpeed * GameManager.instance.currentScale;

        // rotate player based on the movement direction
        if (movement != Vector3.zero)
        {
            // snap player y position to the ground
            Vector3 currentPosition = transform.localPosition;
            currentPosition.y = 0.0f;

            transform.localPosition = currentPosition;

            // rotate player based on the movement direction
            transform.rotation = Quaternion.LookRotation(movementDir);

            // play running animation
            if (soldierAnimation.currentClip != soldierAnimation.runningClip)
            {
                soldierAnimation.PlayAnimation(soldierAnimation.runningClip);
            }
        }
        else
        {
            // play idle animation
            if (soldierAnimation.currentClip != soldierAnimation.idleClip)
            {
                soldierAnimation.PlayAnimation(soldierAnimation.idleClip);
            }
        }

        // if the player is currently holding the ball, move the ball alongside the player
        if (heldBall != null)
        {
            // set target position
            Vector3 targetPosition = ballCarryTransform.position;
            targetPosition.y = heldBall.transform.position.y;

            // move and rotate ball alongside the player's carrying position
            heldBall.transform.position = targetPosition;
            heldBall.transform.rotation = ballCarryTransform.rotation;

            // animate ball when carried by the player
            if (movement != Vector3.zero)
            {
                heldBall.RollAnimation();
            }
        }
    }
}
