using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public EnemyDeathState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        // TODO: add ragdoll logic
        enemyStateMachine.Weapon.gameObject.SetActive(false);
        Object.Destroy(enemyStateMachine.Target);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
