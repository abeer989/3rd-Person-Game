using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    //Animator Parameters:
    private int BlockHash = Animator.StringToHash("Block");

    public PlayerBlockingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        playerStateMachine.Animator.CrossFadeInFixedTime(BlockHash, crossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime); // keep being affected by gravity, knockback, etc.

        if (!playerStateMachine.InputReader.IsBlocking)
            RevertToPreviousState();
    }

    public override void Exit()
    {
    }
}
