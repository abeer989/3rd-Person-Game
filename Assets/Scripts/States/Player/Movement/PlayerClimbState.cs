using UnityEngine;

public class PlayerClimbState : PlayerBaseState
{
    // Animator Parameters
    private readonly int ClimbHash = Animator.StringToHash("Pull Up");

    private Vector3 climbOffset = new Vector3(0, 2.2325f, .65f);

    public PlayerClimbState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(ClimbHash, .1f);
    }

    public override void Tick(float deltaTime)
    {
        // if the climb animation hasn't finished playing, do nothing
        if (GetNormalizedAnimationTime(animator: playerStateMachine.Animator, animTag: "Climbing") < 1f) { return; }

        // we're using a climb animation that updates the position of the player right but not the transform.
        // So, when the climb anim. is done playing, the rig snaps back to where the transform was last. To
        // eliminate that, we turn off the char. controller (because it conflicts w/ translation) teleport the transform
        // to the correct position relative to the character itself (not the world) and that way, at the end of the anim.,
        // the rig and the transform are in the same pos.:
        playerStateMachine.CharacterController.enabled = false;
        playerStateMachine.transform.Translate(climbOffset, Space.Self);
        playerStateMachine.CharacterController.enabled = true;

        // and as soon as it does, revert to locomotion:
        playerStateMachine.SwitchState(new PlayerFreeLookState(playerStateMachine: playerStateMachine, shouldCFade: false));
    }

    public override void Exit()
    {
        playerStateMachine.CharacterController.Move(Vector3.zero); // remove any directional momentum
        playerStateMachine.ForceReceiver.ResetForces(); // reset all forces
    }
}
