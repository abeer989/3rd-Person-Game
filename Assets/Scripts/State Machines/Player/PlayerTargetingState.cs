using System;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    public PlayerTargetingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    // Animator Parameters
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardSpeedHash = Animator.StringToHash("TargetingForwardSpeed");
    private readonly int TargetingRightSpeedHash = Animator.StringToHash("TargetingRightSpeed");

    private const float animatorDampTime = .1f;

    public override void Enter()
    {
        playerStateMachine.Animator.Play(TargetingBlendTreeHash);
        playerStateMachine.InputReader.TargetCancelEvent += OnTargetCancel;
    }

    public override void Tick(float deltaTime)
    {
        if (playerStateMachine.Targeter.CurrentTarget == null || playerStateMachine.Targeter.GetTargetCount() <= 0)
        {
            playerStateMachine.SwitchState(new PlayerFreeLookState(playerStateMachine));
            return;
        }
        
        // Movement:
        Vector3 movement = CalculateTargetFacingMovement();
        Move(movement * playerStateMachine.TargetingMoveSpeed, deltaTime);

        UpdateAnimation(deltaTime);

        // Rotation:
        FaceTarget();
    }

    private void UpdateAnimation(float deltaTime)
    {
        switch (playerStateMachine.InputReader.MovementValue.y)
        {
            case 0:
                playerStateMachine.Animator.SetFloat(TargetingForwardSpeedHash, 0, animatorDampTime, deltaTime);
                break;

            default:
                float val = playerStateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
                playerStateMachine.Animator.SetFloat(TargetingForwardSpeedHash, val, animatorDampTime, deltaTime);
                break;
        }        
        
        switch (playerStateMachine.InputReader.MovementValue.x)
        {
            case 0:
                playerStateMachine.Animator.SetFloat(TargetingRightSpeedHash, 0, animatorDampTime, deltaTime);
                break;

            default:
                float val = playerStateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
                playerStateMachine.Animator.SetFloat(TargetingRightSpeedHash, val, animatorDampTime, deltaTime);
                break;
        }
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.TargetCancelEvent -= OnTargetCancel;
    }

    private void OnTargetCancel()
    {
        playerStateMachine.Targeter.Cancel();
        playerStateMachine.SwitchState(new PlayerFreeLookState(playerStateMachine));
    }

    private Vector3 CalculateTargetFacingMovement()
    {
        Vector3 targetFacingMovement = new Vector3();

        targetFacingMovement += playerStateMachine.transform.right * playerStateMachine.InputReader.MovementValue.x;
        targetFacingMovement += playerStateMachine.transform.forward * playerStateMachine.InputReader.MovementValue.y;

        return targetFacingMovement;
    }
}
