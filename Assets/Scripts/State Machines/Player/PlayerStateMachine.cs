using UnityEngine;
using SmartData.SmartFloat;

public class PlayerStateMachine : StateMachine
{
    [SerializeField] private FloatWriter playerHP;
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }

    [HideInInspector] public Transform mainCameraTransform;

    [field: SerializeField] public float FreeLookMoveSpeed { get; private set; }
    [field: SerializeField] public float TargetingMoveSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }


    private void Start()
    {
        playerHP.SetToDefault();
        mainCameraTransform = Camera.main.transform;
        SwitchState(new PlayerFreeLookState(this));
    }
}
