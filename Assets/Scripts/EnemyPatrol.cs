using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float speed = 3f;
    private bool movingRight = true;

    [Header("Detectores")]
    public Transform groundCheck;
    public float distanceToGround = 1.5f;
    public float distanceToWall = 1.5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        if (groundCheck == null)
        {
            Debug.LogError(
                "Asigna GroundCheck en el Inspector de EnemyPatrol.",
                this
            );

            enabled = false;
        }
    }

    void FixedUpdate()
    {
        Vector2 direction =
            movingRight ? Vector2.right : Vector2.left;

        RaycastHit2D groundAhead = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            distanceToGround,
            groundLayer
        );

        RaycastHit2D wallAhead = Physics2D.Raycast(
            transform.position,
            direction,
            distanceToWall,
            groundLayer
        );

        if (groundAhead.collider == null ||
            wallAhead.collider != null)
        {
            Flip();
        }

        rb.linearVelocity = new Vector2(
            movingRight ? speed : -speed,
            rb.linearVelocity.y
        );
    }

    void Flip()
    {
        movingRight = !movingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (groundCheck != null)
        {
            Gizmos.DrawRay(
                groundCheck.position,
                Vector2.down * distanceToGround
            );
        }

        Vector2 direction =
            movingRight ? Vector2.right : Vector2.left;

        Gizmos.DrawRay(
            transform.position,
            direction * distanceToWall
        );
    }
}