using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    public PlayerFreeLookState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    // Animator Parameters
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    
    // Control Variables:
    private const float animatorDampTime = .1f;
    private const float crossFadeDuration = .2f;

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, crossFadeDuration);

        // Subscribing the OnTarget method to the TargetEvent in the player's InputReader comp. here because the FreeLookState is the
        // default starting state and now when the player presses the "target" key, the playerStateMachine will switch states to Targeting:
        playerStateMachine.InputReader.TargetEvent += OnTarget;
    }

    public override void Tick(float deltaTime)
    {
        if (playerStateMachine.InputReader.IsAttacking)
        {
            playerStateMachine.SwitchState(new PlayerAttackingState(playerStateMachine: playerStateMachine, attackIndex: 0));
            return;
        }

        // Player Movement:
        Vector3 movement = CalculateCameraRelativeMovement();
        Move(movement * playerStateMachine.FreeLookMoveSpeed, deltaTime);

        // Rotation (in the direction of movement)
        if (playerStateMachine.InputReader.MovementValue != Vector2.zero)
        {
            playerStateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, animatorDampTime, deltaTime);
            FaceMovementDirection(movement, deltaTime);
        }

        else
            playerStateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, animatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        playerStateMachine.InputReader.TargetEvent -= OnTarget;
    }

    /// <summary>
    /// Make the player always face the direction of their movement
    /// </summary>
    /// <param name="_movement"></param>
    /// <param name="_deltaTime"></param>
    private void FaceMovementDirection(Vector3 _movement, float _deltaTime)
    {
        playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation,
                                                                Quaternion.LookRotation(_movement),
                                                                _deltaTime * playerStateMachine.RotationDamping);
    }

    /// <summary>
    /// This function gets the Camera's forward and right vectors, normalizes them and returns a vector that causes the player to face whatever direction
    /// the Camera is facing.
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateCameraRelativeMovement()
    {
        Vector3 camForward = playerStateMachine.mainCameraTransform.forward;
        Vector3 camRight = playerStateMachine.mainCameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 camRelativeMovement = camForward * playerStateMachine.InputReader.MovementValue.y +
                                      camRight * playerStateMachine.InputReader.MovementValue.x;

        return camRelativeMovement;
    }

    /// <summary>
    /// This func. switches to the targeting state as soon as a target is set in the Targeter comp. on the player
    /// </summary>
    private void OnTarget()
    {
        if(!playerStateMachine.Targeter.SelectTarget()) { return; }

        playerStateMachine.SwitchState(new PlayerTargetingState(playerStateMachine));
    }
}