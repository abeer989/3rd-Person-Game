using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    private Collider[] ragdollColliders;
    private Rigidbody[] ragdollRigidBodies;

    void Start()
    {
        ragdollColliders = GetComponentsInChildren<Collider>(true);
        ragdollRigidBodies = GetComponentsInChildren<Rigidbody>(true);

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool state)
    {
        foreach (Collider collider in ragdollColliders)
        {
            if (collider.CompareTag("Ragdoll"))
                collider.enabled = state;
        }        
        
        foreach (Rigidbody rb in ragdollRigidBodies)
        {
            if (rb.CompareTag("Ragdoll"))
            {
                rb.isKinematic = !state;
                rb.useGravity = state;
            }
        }

        characterController.enabled = !state;
        animator.enabled = !state;
    }
}
