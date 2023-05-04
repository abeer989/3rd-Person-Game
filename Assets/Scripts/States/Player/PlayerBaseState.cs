using UnityEngine;

public abstract class PlayerBaseState : State
{
    protected PlayerStateMachine playerStateMachine;

    // Control Variables:
    protected const float animatorDampTime = .1f;
    protected const float crossFadeDuration = .2f;

    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }

    protected void Move(Vector3 keyPressMotion, float _deltaTime) => 
        playerStateMachine.CharacterController.Move((keyPressMotion + playerStateMachine.ForceReceiver.GravitationalMovement) * _deltaTime);

    protected void Move(float _deltaTime) => Move(Vector3.zero, _deltaTime);

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

    /// <summary>
    /// Revert to either the free look or the targeting state from any state
    /// </summary>
    protected void RevertToLocomotion()
    {
        if (playerStateMachine.Targeter.CurrentTarget != null)
            playerStateMachine.SwitchState(new PlayerTargetingState(playerStateMachine));        
        
        else
            playerStateMachine.SwitchState(new PlayerFreeLookState(playerStateMachine));
    }
}
