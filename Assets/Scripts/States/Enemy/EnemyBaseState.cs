using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine enemyStateMachine;

    // Animator Parameters:
    protected readonly int SpeedHash = Animator.StringToHash("Speed");
    protected readonly int LocomotionBlendHash = Animator.StringToHash("Locomotion");
    protected readonly int AttackHash = Animator.StringToHash("Attack");

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

    protected bool IsInChaseRange() { return Vector3.Distance(enemyStateMachine.transform.position, enemyStateMachine.Player.transform.position) <= enemyStateMachine.PlayerChaseRange; }
}