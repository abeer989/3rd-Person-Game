using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
    // Action events:
    public event Action JumpEvent;
    public event Action DodgeEvent;
    public event Action TargetEvent;
    public event Action TargetCancelEvent;

    private bool targeting = false;

    public Vector2 MovementValue { get; private set; }

    private Controls controls;

    void Start()
    {
        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    private void OnDestroy()
    {
        controls.Player.Disable();
    }

    #region Keypress Events
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            JumpEvent?.Invoke();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed)
            DodgeEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // read the vector 2 value coming in whenever movement input is sent through:
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnTarget(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!targeting)
            {
                targeting = true;
                TargetEvent?.Invoke();
            }

            else
            {
                targeting = false;
                TargetCancelEvent?.Invoke();
            }
        }
    }
    #endregion
}
