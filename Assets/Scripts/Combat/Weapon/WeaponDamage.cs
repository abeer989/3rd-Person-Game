using UnityEngine;
using System.Collections.Generic;
using SmartData.SmartFloat;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider ownCollider;
    [SerializeField] private FloatWriter PlayerHP;

    private int damageDealt;
    private List<Collider> alreadyHit = new List<Collider>();

    private void OnEnable()
    {
        alreadyHit.Clear();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other == ownCollider) { return; } // do nothing if the weapon collides with player's own body

        if (alreadyHit.Contains(other)) { return; } // do nothing if the collider has already been hit

        alreadyHit.Add(other); // else, add the collider to the alreadyHit list, so it doesn't get mult. times

        if (other.TryGetComponent(out UnitHealth enemyHealth))
            enemyHealth.TakeDamage(damageDealt);

        else if (other.CompareTag("Player"))
        {
            if (PlayerHP != null)
            {
                PlayerHP.value -= damageDealt;
                Debug.Log("Player HP: " + PlayerHP.value);
            }
        }
    }

    /// <summary>
    /// Will receive a damage amount and set it to be the damage dealt by the weapon
    /// </summary>
    /// <param name="num"></param>
    public void SetAttackDamage(int num)
    {
        damageDealt = num;
    }
}
