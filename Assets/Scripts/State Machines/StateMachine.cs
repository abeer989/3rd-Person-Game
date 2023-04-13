using UnityEngine;

/// <summary>
/// This class will handle state manipulation/switching logic.
/// </summary>
public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    private void Update()
    {
        // call the tick method on every frame, as long as it's not null:
        currentState?.Tick(deltaTime: Time.deltaTime);
    }

    public void SwitchState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
}
