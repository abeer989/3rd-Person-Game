using UnityEngine;
using SmartData.SmartFloat;
using Sirenix.OdinInspector;

public class PlayerStateMachine : StateMachine
{
    [HideInInspector] public Transform mainCameraTransform;
    [SerializeField] private FloatWriter playerHP;

    // Components:
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController CharacterController { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Ragdoll RagdollComp { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    
    [field: SerializeField] public Vector3 ClimbOffset { get; private set; }

    [field: SerializeField] public float FreeLookMoveSpeed { get; private set; }
    [field: SerializeField] public float TargetingMoveSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }

    [field: SerializeField] public float DodgeTime { get; private set; }
    [field: SerializeField] public float DodgeLength { get; private set; }

    [field: SerializeField] public float JumpForce { get; private set; }


    public bool IsDead => playerHP.value <= 0;

    private bool IsInvincible = false;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        if (IsInvincible) { return; }

        if (!InputReader.IsBlocking)
        {
            SwitchState(new PlayerImpactState(this));
            playerHP.value -= dmg;
        }

        else
            playerHP.value -= (dmg * .2f); // take 20% damage when blocking

        Debug.Log("Player Health:" + playerHP.value);

        if (playerHP.value <= 0)
        {
            playerHP.value = 0;
            SwitchState(new PlayerDeathState(this));
        }
    }

    /// <summary>
    /// Toggle the IsInvincible bool and if it's on, the player won't take damage (see TakeDamage func.)
    /// </summary>
    /// <param name="state"></param>
    public void ToggleInvincibility(bool state) => IsInvincible = state;
}
