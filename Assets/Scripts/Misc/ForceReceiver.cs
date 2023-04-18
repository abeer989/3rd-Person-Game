using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    public Vector3 GravitationalMovement => impactForce + Vector3.up * verticalVelocity;

    [SerializeField] private float drag = .3f;
    
    private float verticalVelocity;
    private Vector3 impactForce;
    private Vector3 dampingVelocity;
    private CharacterController characterController;


    private void OnEnable()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();

        // whenever an impact force is added, we don't want it just stay, we want want dampen it down to zero after a while:
        impactForce = Vector3.SmoothDamp(current: impactForce, target: Vector3.zero, currentVelocity: ref dampingVelocity, smoothTime: drag);
    }

    /// <summary>
    /// This function will apply gravity to the object the script is attached to on the Y Axis
    /// </summary>
    private void ApplyGravity()
    {
        if (verticalVelocity < 0 && characterController.isGrounded)
        {
            // if we set verticalVelocity to 0, the player will fall from even the slightest of declines:
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }

        else
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
    }

    /// <summary>
    /// Add an impact for the player's body
    /// </summary>
    /// <param name="force"></param>
    public void AddImpactForce(Vector3 force) => impactForce += force;
}