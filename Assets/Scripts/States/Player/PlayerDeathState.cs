public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
        // TODO: 1. add ragdoll logic -- [DONE]
        //       2. raise an event or something that causes enemies to play a victory animation (?)

        playerStateMachine.RagdollComp.ToggleRagdoll(true);
        playerStateMachine.Weapon.gameObject.SetActive(false);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }
}
