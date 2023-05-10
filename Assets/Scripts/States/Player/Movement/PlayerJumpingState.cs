using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    // Animator Parameters
    private readonly int JumpHash = Animator.StringToHash("Jump");

    private Vector3 jumpingMomentum;

    public PlayerJumpingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        playerStateMachine.ForceReceiver.AddJumpForce(playerStateMachine.JumpForce); // call the add jump force function as soon as the player enters jump state

        // preserve the momentum and the direction of movement while jumping:
        jumpingMomentum = playerStateMachine.CharacterController.velocity;
        jumpingMomentum.y = 0;

        playerStateMachine.Animator.CrossFadeInFixedTime(JumpHash, crossFadeDuration);

        playerStateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetection;
    }

    public override void Tick(float deltaTime)
    {
        Move(jumpingMomentum, deltaTime); // move the player based on the momentum and the direction of movement while jumping

        // if the vertical velocity is -ve, that means the player is now falling down:
        if (playerStateMachine.CharacterController.velocity.y <= 0)
        {
            // if so, switch to the falling state:
            playerStateMachine.SwitchState(new PlayerFallingState(playerStateMachine));
            return;
        }

        FaceTarget(); // face target while jumping if in Targeting State
    }

    public override void Exit()
    {
        playerStateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetection;
    }

    private void HandleLedgeDetection(Vector3 hangDirection, Vector3 closestPoint)
    {
        playerStateMachine.SwitchState(new PlayerHangingState(playerStateMachine: playerStateMachine, hangDirection: hangDirection, closestPoint: closestPoint));
    }
}
