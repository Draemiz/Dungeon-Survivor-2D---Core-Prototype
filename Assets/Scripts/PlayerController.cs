
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 28f;

    [Header("Salto")]
    public float jumpForce = 54f;
    [Min(1f)] public float fallGravityMultiplier = 1.25f;
    [Min(1f)] public float releasedJumpGravityMultiplier = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float move;
    private bool jumpRequested;
    private bool jumpHeld;
    private readonly HashSet<Collider2D> groundContacts = new HashSet<Collider2D>();
    private bool isGrounded;
    private bool hasSpeedParameter;
    private bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == "Speed" && parameter.type == AnimatorControllerParameterType.Float)
                hasSpeedParameter = true;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        // Movimiento horizontal
        move = Input.GetAxis("Horizontal");
        bool jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        if (jumpPressed && isGrounded) jumpRequested = true;
        animator.SetBool("isGrounded", isGrounded);

        // Animaciones Idle y Walk
        if (hasSpeedParameter) animator.SetFloat("Speed", Mathf.Abs(move));

        // Girar al personaje
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

    }

    void FixedUpdate()
    {
        // Consumir la pulsación una sola vez en el paso de física.
        if (jumpRequested && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );

            groundContacts.Clear();
            isGrounded = false;
        }
        jumpRequested = false;

        // Arco completo incluso con un toque; caída ligeramente más rápida.
        float gravityMultiplier = rb.linearVelocity.y < 0f
            ? fallGravityMultiplier
            : (rb.linearVelocity.y > 0f && !jumpHeld ? releasedJumpGravityMultiplier : 1f);
        rb.linearVelocity += Physics2D.gravity * rb.gravityScale
            * (gravityMultiplier - 1f) * Time.fixedDeltaTime;
        // Movimiento horizontal
        rb.linearVelocity = new Vector2(
            move * moveSpeed,
            rb.linearVelocity.y
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateGroundContact(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        UpdateGroundContact(collision);
    }

    void UpdateGroundContact(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;

        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.5f && rb.linearVelocity.y <= 0.1f)
            {
                groundContacts.Add(collision.collider);
                isGrounded = true;
                return;
            }
        }
        groundContacts.Remove(collision.collider);
        isGrounded = groundContacts.Count > 0;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        groundContacts.Remove(collision.collider);
        isGrounded = groundContacts.Count > 0;
    }

    void OnDisable()
    {
        groundContacts.Clear();
        isGrounded = false;
        move = 0f;
        jumpRequested = false;
        jumpHeld = false;
    }

    void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
