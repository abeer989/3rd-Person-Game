using UnityEngine;

public class PlayerImpactState : PlayerBaseState
{
    public PlayerImpactState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    // Animator Parameters:
    protected readonly int ImpactHash = Animator.StringToHash("Impact");

    private float duration = .7f;

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(ImpactHash, crossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0)
            RevertToPreviousState();
    }

    public override void Exit()
    {
    }
}
