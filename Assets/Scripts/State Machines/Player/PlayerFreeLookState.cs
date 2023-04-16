using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    public PlayerFreeLookState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    // Animator Parameters
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    
    // Control Variables:
    private const float animatorDampTime = .1f;

    public override void Enter()
    {
        playerStateMachine.Animator.Play(FreeLookBlendTreeHash);

        // Subscribing the OnTarget method to the TargetEvent in the player's InputReader comp. here because the FreeLookState is the
        // default starting state and now when the player presses the "target" key, the playerStateMachine will switch states to Targeting:
        playerStateMachine.InputReader.TargetEvent += OnTarget;
    }

    public override void Tick(float deltaTime)
    {
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

    private void FaceMovementDirection(Vector3 _movement, float _deltaTime)
    {
        playerStateMachine.transform.rotation = Quaternion.Lerp(playerStateMachine.transform.rotation,
                                                                Quaternion.LookRotation(_movement),
                                                                _deltaTime * playerStateMachine.RotationDamping);
    }

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

    private void OnTarget()
    {
        if(!playerStateMachine.Targeter.SelectTarget()) { return; }

        playerStateMachine.SwitchState(new PlayerTargetingState(playerStateMachine));
    }
}