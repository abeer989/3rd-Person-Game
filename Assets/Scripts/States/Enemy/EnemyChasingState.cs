using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    public EnemyChasingState(EnemyStateMachine enemyStateMachine) : base(enemyStateMachine) { }

    public override void Enter()
    {
        enemyStateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendHash, crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        enemyStateMachine.Animator.SetFloat(SpeedHash, 1, animatorDampTime, deltaTime);

        if (!IsInChaseRange())
        {
            enemyStateMachine.SwitchState(new EnemyIdleState(enemyStateMachine));
            return;
        }

        else if (IsInAttackRange())
        {
            enemyStateMachine.SwitchState(new EnemyAttackingState(enemyStateMachine));
            return;
        }

        FacePlayer();
        MoveToPlayer(deltaTime);
    }


    public override void Exit()
    {
        // reset agent pathing and set vel. to zero:
        enemyStateMachine.Agent.ResetPath();
        enemyStateMachine.Agent.velocity = Vector3.zero;
    }

    /// <summary>
    /// This function will calculate the path to the player and the move the enemy towards the player using
    /// the Move method.
    /// </summary>
    /// <param name="deltaTime"></param>
    private void MoveToPlayer(float deltaTime)
    {
        UnityEngine.AI.NavMeshAgent agentComp = enemyStateMachine.Agent;

        if (agentComp.isOnNavMesh)
        {
            agentComp.SetDestination(enemyStateMachine.Player.transform.position);

            // calculate the movement dir. based on the desired velocity (where the agent wants to go) multiplied by the move speed:
            Vector3 movementDirection = agentComp.desiredVelocity.normalized * enemyStateMachine.MoveSpeed;
            Move(movementDirection, deltaTime); 
        }

        // updating the NM Agent velocity by making it equal to the char. controller's vel.:
        agentComp.velocity = enemyStateMachine.CharacterController.velocity;
    }

    /// <summary>
    /// Returns if the player is within the enemy's attack range.
    /// </summary>
    /// <returns></returns>
    private bool IsInAttackRange() 
    {
        if (enemyStateMachine.Player.IsDead) { return false; }

        return Vector3.Distance(enemyStateMachine.transform.position, enemyStateMachine.Player.transform.position) <= enemyStateMachine.AttackRange; 
    }
}
