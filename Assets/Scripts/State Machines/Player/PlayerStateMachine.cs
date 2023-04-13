using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [Title("Properties")]
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }

    [Title("Control Floats")]
    [field: SerializeField] public float FreeLookMoveSpeed { get; private set; }

    private void Start()
    {
        SwitchState(new PlayerTestState(this));
    }
}
