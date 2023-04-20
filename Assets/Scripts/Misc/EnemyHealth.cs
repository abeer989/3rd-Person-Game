using UnityEngine;
using ScriptableEvents.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private SimpleScriptableEvent impactEvent;
    [SerializeField] private SimpleScriptableEvent deathEvent;

    private int health;

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        impactEvent.Raise();

        health -= damageAmount;
        Debug.Log("Enemy Health: " + health);

        if (health <= 0)
        {
            health = 0;
            deathEvent.Raise();
        }
    }
}
