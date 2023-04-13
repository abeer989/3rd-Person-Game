using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine playerStateMachine) : base(playerStateMachine) { }

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3(playerStateMachine.InputReader.MovementValue.x, 0, playerStateMachine.InputReader.MovementValue.y);
        playerStateMachine.CharacterController.Move(movement * Time.deltaTime * playerStateMachine.FreeLookMoveSpeed);
    }

    public override void Exit()
    {
    }

    private void Jump()
    {
        Debug.Log("Jumping");
        playerStateMachine.SwitchState(new PlayerTestState(playerStateMachine));
    }
}