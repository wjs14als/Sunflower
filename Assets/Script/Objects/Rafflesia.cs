using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rafflesia : MonoBehaviour
{
    private BoxCollider2D boxC;
    private Animator animator;
    private bool coll;
    void Awake()
    {
        boxC = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("boom");
        }
    }
}
