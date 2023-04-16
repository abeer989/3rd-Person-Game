using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine playerStateMachine;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }

    protected void Move(Vector3 keyPressMotion, float _deltaTime) => 
        playerStateMachine.CharacterController.Move((keyPressMotion + playerStateMachine.ForceReceiver.GravitationalMovement) * _deltaTime);

    /// <summary>
    /// This method will make sure that in the targeting state and other states, the player keeps facing the CurrentTarget in its Targeter comp.
    /// </summary>
    protected void FaceTarget()
    {
        if (playerStateMachine.Targeter.CurrentTarget != null)
        {
            Vector3 faceDirection = playerStateMachine.Targeter.CurrentTarget.transform.position - playerStateMachine.transform.position;
            faceDirection.y = 0;

            playerStateMachine.transform.rotation = Quaternion.LookRotation(faceDirection);
        }
    }
}
