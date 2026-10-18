using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int health = 3;

    public void TakeDamage(int damage)
    {
        if (health <= 0 || damage <= 0)
            return;

        health -= damage;

        Debug.Log("Vida del enemigo: " + health, this);

        if (health <= 0)
            Destroy(gameObject);
    }
}