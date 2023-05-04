using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine enemyStateMachine;

    // Animator Parameters:
    protected readonly int SpeedHash = Animator.StringToHash("Speed");
    protected readonly int LocomotionBlendHash = Animator.StringToHash("Locomotion");

    // Control Floats:
    protected const float animatorDampTime = .1f;
    protected const float crossFadeDuration = .2f;

    public EnemyBaseState(EnemyStateMachine enemyStateMachine)
    {
        this.enemyStateMachine = enemyStateMachine;
    }

    protected void Move(Vector3 keyPressMotion, float _deltaTime) =>
        enemyStateMachine.CharacterController.Move((keyPressMotion + enemyStateMachine.ForceReceiver.GravitationalMovement) * _deltaTime);

    protected void Move(float _deltaTime) => Move(Vector3.zero, _deltaTime);

    /// <summary>
    /// Always rotate to face player.
    /// </summary>
    protected void FacePlayer()
    {
        if (enemyStateMachine.Player != null)
        {
            Vector3 faceDirection = enemyStateMachine.Player.transform.position - enemyStateMachine.transform.position;
            faceDirection.y = 0;

            enemyStateMachine.transform.rotation = Quaternion.LookRotation(faceDirection);
        }
    }

    /// <summary>
    /// Returns if the player is within the enemy's chase range.
    /// </summary>
    /// <returns></returns>
    protected bool IsInChaseRange()
    {
        if (enemyStateMachine.Player.IsDead) { return false; }

        return Vector3.Distance(enemyStateMachine.transform.position, enemyStateMachine.Player.transform.position) <= enemyStateMachine.PlayerChaseRange;
    }
}
