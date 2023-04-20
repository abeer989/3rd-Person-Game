using UnityEngine;

public class EnemyImpactState : EnemyBaseState
{
    public EnemyImpactState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    // Animator Parameters:
    protected readonly int ImpactHash = Animator.StringToHash("Impact");

    private float duration = 1;

    public override void Enter()
    {
        enemyStateMachine.Animator.CrossFadeInFixedTime(ImpactHash, crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if (duration <= 0)
            enemyStateMachine.SwitchState(new EnemyIdleState(enemyStateMachine));
            
    }

    public override void Exit()
    {
    }
}
