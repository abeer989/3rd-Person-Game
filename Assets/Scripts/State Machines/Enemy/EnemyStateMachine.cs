using SmartData.SmartFloat;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public NavMeshAgent Agent { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float PlayerChaseRange { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public int AttackDamage { get; private set; }

    public GameObject Player { get; private set; }

    private void Start()
    {
        // Disable NM Agent movement and rot. control:
        Agent.updatePosition = false;
        Agent.updateRotation = false;

        Player = GameObject.FindGameObjectWithTag("Player");
        SwitchState(new EnemyIdleState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChaseRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
