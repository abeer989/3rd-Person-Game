using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    public EnemyAttackingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    // Animator Parameters:
    private readonly int AttackHash = Animator.StringToHash("Attack");

    public override void Enter()
    {
        enemyStateMachine.Animator.CrossFadeInFixedTime(AttackHash, crossFadeDuration);
        enemyStateMachine.Weapon.SetAttackProperties(enemyStateMachine.AttackDamage, enemyStateMachine.Knockback);
    }

    public override void Tick(float deltaTime)
    {
        // if the attack animation has completed playing:
        if (GetNormalizedAttackAnimationTime(enemyStateMachine.Animator) >= 1)
            enemyStateMachine.SwitchState(new EnemyChasingState(enemyStateMachine));
    }

    public override void Exit()
    {
    }
}
