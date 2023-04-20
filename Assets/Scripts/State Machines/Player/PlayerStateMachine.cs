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

    /// <summary>
    /// Will decrease player health. Called when the damagePlayerEvent gets raised in the WeaponDamage script:
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(int dmg)
    {
        SwitchState(new PlayerImpactState(this));

        playerHP.value -= dmg;
        Debug.Log("Player Health:" + playerHP.value);

        if (playerHP.value <= 0)
        {
            playerHP.value = 0;
            SwitchState(new PlayerDeathState(this));
        }
    }
}
