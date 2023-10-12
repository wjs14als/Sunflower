using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CapsuleCollider2D))]
public class PlayerMain : MonoBehaviour
{
    [SerializeField] private float movespeed = 5;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float jumpTime;
    [SerializeField] private float fallMul;
    [SerializeField] private float jumpMul;
    [SerializeField] private float knockbackPow = 10;
    [SerializeField] private float hitRecovery = 0.2f;
    [SerializeField] private float ropeSpeed = 3f;

    private bool isHurt;
    private bool invincible;
    public GameObject hitBoxCollider;
    public GameObject crouchHitBox;

    private LayerMask collisionLayer;
    private Vector2 direct;
    private float jumpCounter;

    private Vector2 Gravity;

    private bool isRope = false;
    private bool isGrounded;
    private Vector2 footPosition;
    public bool isJumping = false;
    private bool isCrouch = false;
    public enum Statetype { idle, walk, rope, jump, knockback, crouch, GG }
    public Statetype type;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private SpriteRenderer sr;
    private Animator animator;
    public FixedJoystick joy;

    [SerializeField] private int hp = 3;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();

        Gravity = new Vector2(0, -Physics2D.gravity.y);

        StartCoroutine(ResetCollider());
    }

    void Update()
    {
        direct.x = joy.Horizontal;
        direct.y = joy.Vertical;

        filp();

        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        Bounds bounds = capsule.bounds;
        footPosition = new Vector2(bounds.center.x, bounds.min.y);
        isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, LayerMask.GetMask("Platform"));

        if (!isHurt && !isCrouch) MoveTo();

        if (isCrouch) rb.velocity = Vector2.zero;

        Crouch();
        if(rb.velocity.y == 0)
        {
            animator.SetBool("Jump", false);
        }

        if (isRope)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(0, direct.y * ropeSpeed);
        }
        else
        {
            rb.gravityScale = 1f;


            if (rb.velocity.y < 0)
            {
                rb.velocity -= Gravity * fallMul * Time.deltaTime;
            }

            if (rb.velocity.y > 0 && !isHurt && isJumping)
            {
                jumpCounter += Time.deltaTime;
                if (jumpCounter > jumpTime) isJumping = false;

                float t = jumpCounter / jumpTime;
                float currentJumpM = jumpMul;

                if(t > 0.5f)
                {
                    currentJumpM = jumpMul * (1 - t);
                }

                rb.velocity += Gravity * jumpMul * Time.deltaTime;
            }
        }
    }

    public void MoveTo()
    {
        rb.velocity = new Vector2(direct.x * movespeed, rb.velocity.y);
    }

    public void JumpTo()
    {
        if (isGrounded == true && isCrouch == false && animator.GetBool("Jump") == false) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCounter = 0;
            isJumping = true;
            animator.SetBool("Jump", true);
        }
    }

    public void Crouch()
    {
        if(direct.y < -0.8f && isGrounded && !isRope && !isHurt)
        {
            animator.SetBool("Crouch", true);
            isCrouch = true;
            hitBoxCollider.SetActive(false);
            crouchHitBox.SetActive(true);
        }
        else if(direct.y >= -0.8f && isGrounded && !isRope && !isHurt)
        {
            animator.SetBool("Crouch", false);
            isCrouch = false;
            hitBoxCollider.SetActive(true);
            crouchHitBox.SetActive(false);
        }
    }

    void UpdateAnimationState()
    {
        if (!isHurt)
        {
            if(isCrouch)
            {
                type = Statetype.crouch;
            }
            else if (isRope)
            {
                type = Statetype.rope;
            }
            else if(animator.GetBool("Jump"))
            {
                type = Statetype.jump;
            }
            else if(direct.x != 0)
            {
                type = Statetype.walk;
            }
            else
            {
                type = Statetype.idle;
            }
        }
        else
        {
            type = Statetype.knockback;
        }

        SetAnimationState(type);
    }
    void SetAnimationState(Statetype state)
    {
        animator.SetBool("Walk", state == Statetype.walk);
        animator.SetBool("Jump", state == Statetype.jump);
        animator.SetBool("Rope", state == Statetype.rope);
        animator.SetBool("Knockback", state == Statetype.knockback);
        animator.SetBool("Crouch", state == Statetype.crouch);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(footPosition, 0.1f);
    }

    private void filp()
    {
        if (direct.x < -0.01f) transform.eulerAngles = new Vector3(0, -180, 0);
        if (direct.x > 0.01f) transform.eulerAngles = new Vector3(0, 0, 0);
    }

    IEnumerator ResetCollider()
    {
        while (true)
        {
            yield return null;
            if (!hitBoxCollider.activeInHierarchy)
            {
                yield return new WaitForSeconds(hitRecovery);
                hitBoxCollider.SetActive(true);
            }
        }
    }

    IEnumerator InvincibleEffect()
    {
        invincible = true;
        sr.color = new Color(1f, 1f, 1f, 0.4f);
        yield return new WaitForSeconds(1f);

        invincible = false;
        sr.color = new Color(1f, 1f, 1f, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Monster") || collision.transform.CompareTag("Projectile"))
        {
            if (collision.transform.CompareTag("Projectile"))
            {
                Destroy(collision.gameObject, 0.02f);
            }
            if (!invincible)
            {
                StartCoroutine(InvincibleEffect());
                hp--;
            }

            rb.velocity = Vector2.zero;
            isHurt = true;
            Invoke("isHurtReset", 0.5f);
            hitBoxCollider.SetActive(false);

            if (transform.position.x > collision.transform.position.x)
            {
                rb.AddForce(new Vector2(knockbackPow, knockbackPow * 2), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-knockbackPow, knockbackPow * 2), ForceMode2D.Impulse);
            }
        }

        if (collision.CompareTag("Rope"))
        {
            isRope = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            isRope = false;
        }
    }

    void isHurtReset()
    {
        isHurt = false;
    }
}
