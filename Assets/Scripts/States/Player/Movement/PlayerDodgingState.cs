using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    // Animator Parameters
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeForwardSpeedHash = Animator.StringToHash("DodgeForwardSpeed");
    private readonly int DodgeRightSpeedHash = Animator.StringToHash("DodgeRightSpeed");

    private Vector3 dodgingDirectionInput;

    private float remainingDodgeTime;

    public PlayerDodgingState(PlayerStateMachine playerStateMachine, Vector3 dodgingDirectionInput) : base(playerStateMachine) 
    {
        this.dodgingDirectionInput = dodgingDirectionInput;    
    }

    public override void Enter()
    {
        remainingDodgeTime = playerStateMachine.DodgeTime;

        playerStateMachine.Animator.SetFloat(DodgeForwardSpeedHash, dodgingDirectionInput.y);
        playerStateMachine.Animator.SetFloat(DodgeRightSpeedHash, dodgingDirectionInput.x);
        playerStateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, crossFadeDuration);

        playerStateMachine.ToggleInvincibility(state: true);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += playerStateMachine.transform.right * dodgingDirectionInput.x * playerStateMachine.DodgeLength / playerStateMachine.DodgeTime;
        movement += playerStateMachine.transform.forward * dodgingDirectionInput.y * playerStateMachine.DodgeLength / playerStateMachine.DodgeTime;

        Move(movement, deltaTime);
        FaceTarget();

        remainingDodgeTime -= deltaTime;

        if (remainingDodgeTime <= 0) RevertToLocomotion();
    }

    public override void Exit()
    {
        playerStateMachine.ToggleInvincibility(state: false);
    }
}
