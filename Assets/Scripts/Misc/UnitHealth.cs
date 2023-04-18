using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int health;

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (health > 0)
        {
            health -= damageAmount;
            Debug.Log(health);

            if (health <= 0 )
                Destroy(gameObject);
        }
    }
}
