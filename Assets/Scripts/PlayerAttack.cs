using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAttack : MonoBehaviour
{
    [Header("Ataque")]
    public Transform attackPoint;
    public float attackRange = 4f;
    public LayerMask enemyLayer;
    public int damage = 1;
    public float attackCooldown = 0.35f;
    private Animator animator;

    private SpriteRenderer spriteRenderer;
    private Vector3 rightAttackPosition;
    private float nextAttackTime;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (attackPoint == null)
        {
            Debug.LogError(
                "Asigna AttackPoint en PlayerAttack.",
                this
            );

            enabled = false;
            return;
        }

        rightAttackPosition = attackPoint.localPosition;
        rightAttackPosition.x =
            Mathf.Abs(rightAttackPosition.x);
    }

    void LateUpdate()
    {
        // Acompaña el giro que realiza PlayerController con flipX.
        Vector3 position = rightAttackPosition;
        position.x *= spriteRenderer.flipX ? -1f : 1f;
        attackPoint.localPosition = position;

        bool attackPressed =
            Input.GetKeyDown(KeyCode.Z) ||
            Input.GetMouseButtonDown(0);

        if (attackPressed && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Attack();
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayer
        );

        // Un golpe por enemigo, aunque tenga varios colliders.
        HashSet<EnemyHealth> damagedEnemies =
            new HashSet<EnemyHealth>();

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy =
                hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null && damagedEnemies.Add(enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}
