using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower_bird : MonoBehaviour
{
    public float speed; //속도
    public float distance; //감지선 길이
    public float waitingTime; //대기시간
    public float Timer = 0; //시간 측정 변수

    public enum Statetype { idle, fly } //상태 모음
    public Statetype type; //상태값 변수

    private bool isRight = true; //오른쪽인지 확인
    private bool isMove = true; //이동중인지 확인

    public Transform groundDetection;

    private Animator animator;
    [SerializeField] LayerMask layerMask;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isMove) transform.Translate(Vector2.right * speed * Time.deltaTime); //이동

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, layerMask); //모서리 인식
        if (groundInfo.collider == false)
        {
            Timer += Time.deltaTime; //끝에서 대기
            
            //대기
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
                type = Statetype.fly;
            }
            PlayeranimationSt(type);
        }
    }

    void PlayeranimationSt(Statetype type)
    {
        animator.SetBool("idle", type == Statetype.idle);
        animator.SetBool("fly", type == Statetype.fly);
    }
}
