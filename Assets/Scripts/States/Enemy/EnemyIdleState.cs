using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        enemyStateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendHash, crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        enemyStateMachine.Animator.SetFloat(SpeedHash, 0, animatorDampTime, deltaTime);

        if (IsInChaseRange())
        {
            enemyStateMachine.SwitchState(new EnemyChasingState(enemyStateMachine));
            return;
        }
    }

    public override void Exit()
    {
    }
}
