using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SunGoat : MonoBehaviour
{
    [SerializeField] private float rushSpeed = 10;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float distance = 4; // raycast �� ����
    
    // �غ� Ÿ�̸�
    [SerializeField] private float waitTime = 1;
    public float waitTimer = 0;

    // ���� Ÿ�̸�
    [SerializeField] private int attackRate = 4;
    public float attackTimer = 0;

    public enum Statetype { rush, atk};
    Statetype type;

    public bool dirRight; // ���� ����
    public bool isRush = false; // ���� ����
    public bool canAtk; // ���� ����
    public bool isCool = false; // ���� �� ��� ����

    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D CC;

    public LayerMask layerMask;
    public Transform groundDetection;
    private Transform player;
    public GameObject hitBox;

    private void Start()
    {
        player = PlayerPos.Instance.Player.transform; // �÷��̾� ��ġ
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
        if (groundInfo.collider == false) // �¿���� ����
        {
            Filp();
            animator.SetBool("rush", false);
            isRush = false;
        }

        // ���� ����
        if (canAtk && Vector2.Distance(transform.position, player.position) < 7f && Mathf.Abs(transform.position.y - player.position.y) < 2)
        {
            if(isPlayerDir())
            {
                isRush = true;
            }
            else isRush = false;
        }
        else isRush = false;

        if (isRush) // ����
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

        if (isCool) // ����-���� �� ���
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

        // ��Ʈ��
        if (!isRush && !isCool) rb.velocity = new Vector2(transform.localScale.x * walkSpeed, rb.velocity.y);
        if (isCool) rb.velocity = Vector2.zero;

        if(animator.GetBool("ready") && !animator.GetBool("rush"))
        {
            rb.velocity = Vector2.zero;
        }

        if (transform.localScale.x > 0) dirRight = true;
        if (transform.localScale.x < 0) dirRight = false;
    }

    private void Filp() // �¿����
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

    private bool isPlayerDir() // �þ߹���� �÷��̾� ��ġ ��ġ
    {
        if (transform.position.x < player.position.x ? dirRight : !dirRight)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision) // ���� ��Ÿ���� ���� ����
    {
        if (collision.CompareTag("Player"))
        {
            isCool = true;
        }
    }
}
