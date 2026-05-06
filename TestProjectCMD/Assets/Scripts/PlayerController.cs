using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveInput;
    public float speed = 5f;

    private Rigidbody2D rb2d;
    Animator animator => GetComponent<Animator>();
    CombatEntity combatEntity => GetComponent<CombatEntity>();

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float horizontalJumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform point;
    public float fallMultiplier = 15f;
    public float fallSpeed = 3f;
    public float maxFallSpeed = 6f;
    public float maxJumpSpeed = 10f;

    private float originalGravity = 9.8f;
    [SerializeField] private bool isGrounded = true;

    private bool isPunching;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput.x != 0) transform.localScale = new Vector3(moveInput.x, 1, 1);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb2d.velocity = new Vector2(moveInput.x * horizontalJumpForce, jumpForce);
            //rb2d.velocity = new Vector2(context.ReadValue<Vector2>().x * horizontalJumpForce, context.ReadValue<Vector2>().y * jumpForce);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isPunching)
        {
            AudioManager.Instance.PlaySFX("Slash");
            print("attacking");
            animator.SetBool("isMoving", false);
            isPunching = true;
            StartCoroutine(Slash());
        }
    }

    public IEnumerator Slash()
    {
        //float tempMagnitude = camShake.magnitudeVal;
        //float tempDuration = camShake.durationVal;

        //AudioManager.instance.environment.PlayOneShot(punchSound);
        animator.SetBool("isPunching", true);

        //camShake.magnitudeVal = 0.1f;
        //camShake.durationVal = 0.15f;
        //camShake.Shake();

        yield return new WaitForSecondsRealtime(0.5f);
        animator.SetBool("isPunching", false);

        //camShake.magnitudeVal = tempMagnitude;
        //camShake.durationVal = tempDuration;

        isPunching = false;
    }

    private void OnFall()
    {
        //checks for fall speed
        if (!isGrounded && rb2d.velocity.y < fallSpeed)
        {
            rb2d.gravityScale = fallMultiplier;
        }

        if (rb2d.velocity.y < maxFallSpeed && !isGrounded)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxFallSpeed);
        }
    }

    private void FixedUpdate()
    {
        if (combatEntity.isKnockedBack) return;

        //Vector2 move = new Vector2(moveInput.x, moveInput.y);
        if (!isPunching) rb2d.velocity = new Vector2(moveInput.x * speed, rb2d.velocity.y);
        else rb2d.velocity = new Vector2(0, 0);

        if (moveInput.x == 0 && !isPunching) animator.SetBool("isMoving", false);
        else animator.SetBool("isMoving", true);

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(point.position, 0.05f, groundLayer);

        if (isGrounded)
        {
            rb2d.gravityScale = originalGravity;
        }

        OnFall();
    }
}
