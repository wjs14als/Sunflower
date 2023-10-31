using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Nepenthes : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform genPoint;

    public float attackRate = 5f;
    public float attackRateCalc = 5f;
    public float fireDir;
    
    public bool dirRight;
    //public bool visible = false;
    

    private BoxCollider2D boxC;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public enum Statetype { idle, awake, attack};
    public Statetype type;

    void Start()
    {
        boxC = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateAnimationState();

        if(spriteRenderer.flipX)
        {
            dirRight = true;
            fireDir = 1;
        }
        if(!spriteRenderer.flipX)
        {
            dirRight = false;
            fireDir = -1;
        }
    }

    void UpdateAnimationState()
    {
        if (isPlayerDir())
        {
            if (Vector2.Distance(transform.position, PlayerPos.Instance.Player.transform.position) < 7f)
            {
                if(attackRate == attackRateCalc) type = Statetype.attack;
                else type = Statetype.idle;
                attackRateCalc -= Time.deltaTime;
                if(attackRateCalc <= 0)
                {
                    attackRateCalc = 5f;
                }
            }
            else
            {
                type = Statetype.idle;
                attackRateCalc = 5f;
            }
        }
        else
        {
            type = Statetype.idle;
            attackRateCalc = 5f;
        }

        SetAnimationState(type);
    }
    void SetAnimationState(Statetype state)
    {
        animator.SetBool("Idle", state == Statetype.idle);
        animator.SetBool("Awake", state == Statetype.awake);
        animator.SetBool("Attack", state == Statetype.attack);
    }

    private bool isPlayerDir()
    {
        if(transform.position.x < PlayerPos.Instance.Player.transform.position.x ? dirRight : !dirRight)
        {
            return true;
        }
        return false;
    }

    void Fire()
    {
        GameObject bulletClone = Instantiate (bulletPrefab, genPoint.position, transform.rotation);
        bulletClone.GetComponent<Rigidbody2D>().velocity = transform.right * fireDir * 7f;
        if (fireDir == 1) bulletClone.transform.localScale = new Vector3(-2f, 1.5f, 1f);
    }
}
