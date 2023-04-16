using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    public Vector3 GravitationalMovement => Vector3.up * verticalVelocity;

    private float verticalVelocity;
    private CharacterController characterController;

    private void OnEnable()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (verticalVelocity < 0 && characterController.isGrounded)
        {
            // if we set verticalVelocity to 0, the player will fall from even the slightest of declines:
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }

        else
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
    }
}
