using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SunGoat : MonoBehaviour
{
    [SerializeField] private float rushSpeed = 10;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float distance = 4; // raycast 선 길이
    
    // 준비 타이머
    [SerializeField] private float waitTime = 1;
    public float waitTimer = 0;

    // 공격 타이머
    [SerializeField] private int attackRate = 4;
    public float attackTimer = 0;

    public enum Statetype { rush, atk};
    Statetype type;

    public bool dirRight; // 방향 변수
    public bool isRush = false; // 돌진 변수
    public bool canAtk; // 공격 변수
    public bool isCool = false; // 공격 후 대기 변수

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D CC;

    public LayerMask layerMask;
    public Transform groundDetection;
    private Transform player;
    public GameObject hitBox;

    private void Start()
    {
        player = PlayerPos.Instance.Player.transform; // 플레이어 위치
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CC = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, layerMask);
        if (groundInfo.collider == false) // 좌우반전 실행
        {
            Filp();
            animator.SetBool("rush", false);
            isRush = false;
        }

        // 돌진 조건
        if (canAtk && Vector2.Distance(transform.position, player.position) < 7f && Mathf.Abs(transform.position.y - player.position.y) < 2)
        {
            if(isPlayerDir())
            {
                isRush = true;
            }
            else isRush = false;
        }
        else isRush = false;

        if (isRush) // 돌진
        {
            hitBox.SetActive(true);
            animator.SetBool("ready", true);
            waitTimer += Time.deltaTime;
            if (waitTimer > waitTime)
            {
                animator.SetBool("ready", false);
                animator.SetBool("rush", true);
                rb.velocity = new Vector2(transform.localScale.x * rushSpeed, rb.velocity.y);
                waitTimer = 0;
            }
        }
        else
        {
            waitTimer = 0;
            animator.SetBool("rush", false);
            animator.SetBool("ready", false);
        }

        if (isCool) // 돌진-공격 후 대기
        {
            animator.SetBool("atk", true);
            hitBox.SetActive(false);
            attackTimer += Time.deltaTime;
            canAtk = false;
            if (attackTimer > attackRate)
            {
                animator.SetBool("atk", false);
                canAtk = true;
                isCool = false;
                attackTimer = 0;
            }
        }

        // 패트롤
        if (!isRush && !isCool) rb.velocity = new Vector2(transform.localScale.x * walkSpeed, rb.velocity.y);
        if (isCool) rb.velocity = Vector2.zero;

        if(animator.GetBool("ready") && !animator.GetBool("rush"))
        {
            rb.velocity = Vector2.zero;
        }

        if (transform.localScale.x > 0) dirRight = true;
        if (transform.localScale.x < 0) dirRight = false;
    }

    private void Filp() // 좌우반전
    {
        Vector3 thisScale = transform.localScale;
        if (dirRight)
        {
            thisScale.x = -Mathf.Abs(thisScale.x);
        }
        else
        {
            thisScale.x = Mathf.Abs(thisScale.x);
        }
        transform.localScale = thisScale;
        rb.velocity = Vector2.zero;
    }

    private bool isPlayerDir() // 시야방향과 플레이어 위치 일치
    {
        if (transform.position.x < player.position.x ? dirRight : !dirRight)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision) // 공격 쿨타임을 위한 변수
    {
        if (collision.CompareTag("Player"))
        {
            isCool = true;
        }
    }
}
