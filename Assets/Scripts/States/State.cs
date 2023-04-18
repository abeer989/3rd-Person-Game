/// <summary>
/// The very base blueprint for a state that contains Enter, Tick and Exit functions.
/// </summary>
public abstract class State
{
    public abstract void Enter();
    public abstract void Tick(float deltaTime);
    public abstract void Exit();
}
