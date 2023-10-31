using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : MonoBehaviour
{
    public GameObject walnutPrefab;
    public Transform genPoint;

    //public float attackRate = 4;
    //public float attackRateCalc = 0;
    private int direct;

    public bool canAtk = true;
    public bool dirRight;

    private BoxCollider2D boxC;
    private Animator animator;

    public enum Statetype { idle, atk}
    public Statetype type;

    private void Start()
    {
        boxC = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(genPoint.position.x > 0)
        {
            direct = 1;
        }
        else direct = -1;

        UpdateAnimationState();
    }

    private bool isPlayerDir()
    {
        if (transform.position.x < PlayerPos.Instance.Player.transform.position.x ? dirRight : !dirRight)
        {
            return true;
        }
        return false;
    }

    void UpdateAnimationState()
    {
        if (Vector2.Distance(transform.position, PlayerPos.Instance.Player.transform.position) < 15f && isPlayerDir())
        {
            type = Statetype.atk;
        }
        else
        {
            type = Statetype.idle;
        }

        SetAnimationState(type);
    }
    void SetAnimationState(Statetype state)
    {
        animator.SetBool("idle", state == Statetype.idle);
        animator.SetBool("atk", state == Statetype.atk);
    }

    void Shoot()
    {
        GameObject walnutClone = Instantiate(walnutPrefab, genPoint.position, transform.rotation);
        walnutClone.GetComponent<Rigidbody2D>().AddForce(transform.right * direct, ForceMode2D.Impulse);
    }
}
