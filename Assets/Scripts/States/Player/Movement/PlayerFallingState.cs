using System;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    // Animator Parameters
    private readonly int JumpHash = Animator.StringToHash("Fall");

    private Vector3 fallingMomentum;

    public PlayerFallingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(JumpHash, crossFadeDuration);

        // preserve the momentum and the direction of movement while falling:
        fallingMomentum = playerStateMachine.CharacterController.velocity;
        fallingMomentum.y = 0;

        playerStateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetection;
    }

    public override void Tick(float deltaTime)
    {
        Move(fallingMomentum, deltaTime); // move the player based on the momentum and the direction of movement while falling

        // if the player is now grounded, return to locomotion (freelook or targeting):
        if (playerStateMachine.CharacterController.isGrounded)
        {
            RevertToLocomotion();
            return;
        }

        FaceTarget(); // face target while jumping if in Targeting State
    }

    public override void Exit()
    {
        playerStateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetection;
    }

    private void HandleLedgeDetection(Vector3 hangDirection, Vector3 closestPoint)
    {
        playerStateMachine.SwitchState(new PlayerHangingState(playerStateMachine: playerStateMachine, hangDirection: hangDirection, closestPoint: closestPoint));
    }
}
