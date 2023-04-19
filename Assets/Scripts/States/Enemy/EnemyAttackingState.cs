public class EnemyAttackingState : EnemyBaseState
{
    public EnemyAttackingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    private float transitionDuration = .1f;

    public override void Enter()
    {
        enemyStateMachine.Animator.CrossFadeInFixedTime(AttackHash, transitionDuration);
        enemyStateMachine.Weapon.SetAttackDamage(enemyStateMachine.AttackDamage);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
