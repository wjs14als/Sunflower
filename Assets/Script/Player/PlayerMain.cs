using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(CapsuleCollider2D))]
public class PlayerMain : MonoBehaviour
{
    [SerializeField] private float movespeed = 5; // 이동속도
    [SerializeField] private float jumpForce = 10; // 점프력
    [SerializeField] private float jumpTime; // 
    [SerializeField] private float fallMul;
    [SerializeField] private float jumpMul;
    [SerializeField] private float knockbackPow = 10;
    [SerializeField] private float knockbackDuration = 1.5f;
    [SerializeField] private float ropeSpeed = 3f;

    public bool isHurt;
    private bool invincible;
    public GameObject crouchHitBox;

    private Vector2 pos;
    private Vector3 prePos;
    private Vector3 airPos;
    public bool isFall;

    private LayerMask collisionLayer;
    private Vector2 direct;
    public float jumpCounter;
    public float jumpNum = 0;
    public bool jumpButton;

    private Vector2 Gravity;

    public bool isRope = false;
    public bool ropeCheck = false;
    public float ropeNum = 0;
    private float ropeCenter;
    [SerializeField] private bool isGrounded;
    private Vector2 footPosition;
    public bool isJumping = false;
    [SerializeField] private bool isCrouch = false;
    public enum Statetype { idle, walk, rope, jump, knockback, crouch, GG, jumpfall, fall, land }
    public Statetype type;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private SpriteRenderer sr;
    private Animator animator;
    public FloatingJoystick joy;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();

        Gravity = new Vector2(0, -Physics2D.gravity.y);
    }

    void Update()
    {
        direct.x = joy.Horizontal;
        direct.y = joy.Vertical;

        filp();

        UpdateAnimationState();

        isHurtReset();

        if(rb.velocity.y >= 0)
        {
            prePos = transform.position;
        }
        if(rb.velocity.y < 0)
        {
            airPos = transform.position;
        }
    }

    private void FixedUpdate()
    {
        Bounds bounds = capsule.bounds;
        footPosition = new Vector2(bounds.center.x, bounds.min.y);
        isGrounded = Physics2D.OverlapCircle(footPosition, 0.1f, LayerMask.GetMask("Platform"));

        bool down = Physics2D.OverlapCircle(transform.position + new Vector3(0, -1.5f, 0), 0.15f, LayerMask.GetMask("Rope"));

        if (animator.GetBool("Knockback") == true && isHurt)
        {
            rb.velocity *= 0.98f;
            return;
        }

        if (!isHurt && !isCrouch) MoveTo();

        if (isCrouch) rb.velocity = Vector2.zero;

        Crouch();

        if (ropeCheck && isRope)
        {
            pos = new Vector2(ropeCenter, transform.position.y);
            transform.position = pos;
            rb.isKinematic = true;
            rb.velocity = new Vector2(0, direct.y * ropeSpeed);
            if(direct.y == 0) animator.speed = 0f;
            else animator.speed = 1f;
        }
        else
        {
            rb.isKinematic = false;
            animator.speed = 1f;

            if (rb.velocity.y < 0)
            {
                rb.velocity -= Gravity * fallMul * Time.deltaTime;
            }

            if(rb.velocity.y <= -17)
            {
                rb.velocity = new Vector2(rb.velocity.x, -17);
            }

            if (rb.velocity.y > 0 && !isHurt && isJumping && !isRope)
            {
                jumpCounter += Time.deltaTime;
                if (jumpCounter > jumpTime) isJumping = false;

                float t = jumpCounter / jumpTime;
                float currentJumpM = jumpMul;

                if (t > 0.5f)
                {
                    currentJumpM = jumpMul * (1 - t);
                }

                rb.velocity += Gravity * currentJumpM * Time.deltaTime;
            }

            if(rb.velocity.y > 0 && !jumpButton)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.6f);
            }
        }

        if (!down && isGrounded) rb.isKinematic = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(footPosition, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(0, -1.5f, 0), 0.15f);
    }

    public void MoveTo()
    {
        rb.velocity = new Vector2(direct.x * movespeed, rb.velocity.y);
    }

    public void JumpTo()
    {
        if (isGrounded && !isCrouch && jumpNum == 0 && !isHurt) 
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCounter = 0;
            isJumping = true;
            jumpNum++;
        }
    }

    public void Crouch()
    {
        if(direct.y < -0.5f)
        {
            if(isGrounded && !ropeCheck && !isHurt)
            {
                isCrouch = true;
                capsule.enabled = false;
                crouchHitBox.SetActive(true);
            }
        }
        else
        {
            isCrouch = false;
            capsule.enabled = true;
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
            else if(isRope && ropeCheck)
            {
                type = Statetype.rope;
            }
            else if(!isGrounded)
            {
                if (isJumping) type = Statetype.jump;
                if (rb.velocity.y < 0) type = Statetype.jumpfall;
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

        if (!isGrounded)
        {
            if (prePos.y - airPos.y > 5) type = Statetype.fall;
        }
        //else type = Statetype.land;

        SetAnimationState(type);
    }
    void SetAnimationState(Statetype state)
    {
        animator.SetBool("Walk", state == Statetype.walk);
        animator.SetBool("Jump", state == Statetype.jump);
        animator.SetBool("Rope", state == Statetype.rope);
        animator.SetBool("Knockback", state == Statetype.knockback);
        animator.SetBool("Crouch", state == Statetype.crouch);
        animator.SetBool("JumpFall", state == Statetype.jumpfall);
        animator.SetBool("Fall", state == Statetype.fall);
        animator.SetBool("Land", state == Statetype.land);
    }

    private void filp()
    {
        if (direct.x < -0.01f && !isHurt) transform.eulerAngles = new Vector3(0, -180, 0);
        if (direct.x > 0.01f && !isHurt) transform.eulerAngles = new Vector3(0, 0, 0);
    }

    IEnumerator InvincibleEffect()
    {
        Physics2D.IgnoreLayerCollision(6, 8);
        Physics2D.IgnoreLayerCollision(6, 9);
        Physics2D.IgnoreLayerCollision(6, 16);
        invincible = true;
        sr.color = new Color(1f, 1f, 1f, 0.4f);
        yield return new WaitForSeconds(1.5f);

        Physics2D.IgnoreLayerCollision(6, 8, false);
        Physics2D.IgnoreLayerCollision(6, 9, false);
        Physics2D.IgnoreLayerCollision(6, 16, false);
        invincible = false;
        sr.color = new Color(1f, 1f, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Monster") || collision.transform.CompareTag("Projectile") || collision.transform.CompareTag("Trap"))
        {
            if (collision.transform.CompareTag("Projectile"))
            {
                Destroy(collision.gameObject, 0.02f);
            }
            if (!invincible)
            {
                StartCoroutine(InvincibleEffect());
            }

            rb.velocity = Vector2.zero;
            isHurt = true;
            HpSystem.hp--;

            if(transform.position.x > collision.transform.position.x)
            {
                rb.AddForce(new Vector2(knockbackPow, knockbackPow * 2), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-knockbackPow, knockbackPow * 2), ForceMode2D.Impulse);
            }
        }

        if (collision.CompareTag("SunGoat"))
        {
            if (!invincible)
            {
                StartCoroutine (InvincibleEffect());
            }

            rb.velocity = Vector2.zero;
            isHurt = true;
            HpSystem.hp--;

            if (transform.position.x > collision.transform.position.x)
            {
                rb.AddForce(new Vector2(25, knockbackPow * 2), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-25, knockbackPow * 2), ForceMode2D.Impulse);
            }
        }

        if (collision.CompareTag("RopeCheck"))
        {
            isRope = true;
            ropeCenter = collision.GetComponent<Collider2D>().bounds.center.x;
        }
    }

    private void isHurtReset()
    {
        if (isGrounded && rb.velocity.y == 0)
        {
            isHurt = false;
            animator.SetBool("Knockback", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("RopeCheck"))
        {
            isRope = false;
            ropeNum = 0;
        }
    }
}
