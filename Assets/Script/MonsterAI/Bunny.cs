using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : MonoBehaviour
{
    public float speed;

    public int nextMove;

    private Rigidbody2D rb;
    private Animator animator;
    public LayerMask layerMask;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxC;
    private Transform tran;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxC = GetComponent<BoxCollider2D>();
        tran = GetComponent<Transform>();

        Invoke("Think", 1);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(nextMove * speed, rb.velocity.y);

        Vector2 frontVec = new Vector2(rb.position.x + nextMove * 1, rb.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, layerMask);
        if (rayHit.collider == null)
        {
            filp();
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        animator.SetInteger("Walk", nextMove);

        if (nextMove != 0) spriteRenderer.flipX = nextMove == -1;

        float nextThink = Random.Range(2f, 4f);
        Invoke("Think", nextThink);
    }

    void filp()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == -1;

        CancelInvoke();
        Invoke("Think", 1);
    }
}