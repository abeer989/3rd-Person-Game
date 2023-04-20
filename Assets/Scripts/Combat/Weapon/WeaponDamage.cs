using UnityEngine;
using System.Collections.Generic;
using ScriptableEvents.Events;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider ownCollider;

    private int damageDealt;
    private float knockback;
    private List<Collider> alreadyHit = new List<Collider>();

    // Event:
    [SerializeField] private IntScriptableEvent damagePlayerEvent;

    private void OnEnable()
    {
        alreadyHit.Clear();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other == ownCollider) { return; } // do nothing if the weapon collides with player's own body

        if (alreadyHit.Contains(other)) { return; } // do nothing if the collider has already been hit

        alreadyHit.Add(other); // else, add the collider to the alreadyHit list, so it doesn't get mult. times

        if (other.TryGetComponent(out EnemyHealth enemyHealth))
            enemyHealth.TakeDamage(damageDealt);

        else if (other.CompareTag("Player"))
            damagePlayerEvent?.Raise(damageDealt);

        if (other.TryGetComponent(out ForceReceiver forceReceiver))
        {
            Vector3 dir = (other.transform.position - ownCollider.transform.position).normalized;
            forceReceiver.AddImpactForce(dir * knockback);
        }
    }

    /// <summary>
    /// Will receive a damage amount and set it to be the damage dealt by the weapon
    /// </summary>
    /// <param name="num"></param>
    public void SetAttackProperties(int dmg, float knockback)
    {
        damageDealt = dmg;
        this.knockback = knockback;
    }
}
