using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    public PlayerHangingState(PlayerStateMachine playerStateMachine, Vector3 hangDirection, Vector3 closestPoint) : base(playerStateMachine) 
    {
        this.hangDirection = hangDirection;
        this.closestPoint = closestPoint;
    }

    // Animator Parameters
    private readonly int HangingHash = Animator.StringToHash("Hanging");

    private Vector3 hangDirection;
    private Vector3 closestPoint;

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(HangingHash, .07f);
        playerStateMachine.transform.rotation = Quaternion.LookRotation(hangDirection, Vector3.up); // snap rotation to the forward vector of the ledge
                                                                                                    // will be received via event

        playerStateMachine.CharacterController.enabled = false;
        playerStateMachine.transform.position = closestPoint - (playerStateMachine.LedgeDetector.transform.position - playerStateMachine.transform.position);
        playerStateMachine.CharacterController.enabled = true;
    }

    public override void Tick(float deltaTime)
    {
        // fall down when player presses "s":
        if (playerStateMachine.InputReader.MovementValue.y < 0f)
        {
            playerStateMachine.CharacterController.Move(Vector3.zero); // remove any directional momentum
            playerStateMachine.ForceReceiver.ResetForces(); // reset all forces
            playerStateMachine.SwitchState(new PlayerFallingState(playerStateMachine));
            return;
        }

        // climb up when player presses "w":
        else if (playerStateMachine.InputReader.MovementValue.y > 0f)
        {
            playerStateMachine.SwitchState(new PlayerClimbState(playerStateMachine));
            return;
        }
    }


    public override void Exit()
    {
    }
}
