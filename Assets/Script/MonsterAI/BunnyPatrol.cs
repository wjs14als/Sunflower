using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyPatrol : MonoBehaviour
{
    public float speed; //�ӵ�
    public float distance; //������ ����
    public int waitingTime; //���ð�
    public float Timer = 0; //�ð� ���� ����

    public enum Statetype { idle, walk } //���� ����
    public Statetype type; //���°� ����

    private bool isRight = true; //���������� Ȯ��
    private bool isMove = true; //�̵������� Ȯ��

    public Transform groundDetection;

    private Animator animator;
    [SerializeField] LayerMask layerMask;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(isMove) transform.Translate(Vector2.right * speed * Time.deltaTime); //�̵�

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, layerMask); //�𼭸� �ν�
        if(groundInfo.collider == false )
        {
            Timer += Time.deltaTime; //������ ���

            //��� �� �¿���ȯ ���ǹ�
            if (Timer < waitingTime)
            {
                type = Statetype.idle;
                isMove = false;
            }
            else if (Timer >= waitingTime)
            {
                if (isRight == true)
                {
                    transform.eulerAngles = new Vector3(0, -180, 0);
                    isRight = false;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    isRight = true;
                }
                Timer = 0;
                isMove = true;
                type = Statetype.walk;
            }
            PlayeranimationSt(type);
        }
    }

    void PlayeranimationSt(Statetype type)
    {
        animator.SetBool("idle", type == Statetype.idle);
        animator.SetBool("walk", type == Statetype.walk);
    }
}
