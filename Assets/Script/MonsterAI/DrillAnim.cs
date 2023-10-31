using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillAnim : MonoBehaviour
{
    public Drillboy drill;
    private Animator animator;

    public enum Statetype { idle, ready, shake};
    public Statetype type;

    public void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (drill.isShake)
        {
            type = Statetype.shake;
        }
        else type = Statetype.idle;

        SetAnimationState(type);
    }

    private void SetAnimationState(Statetype state)
    {
        animator.SetBool("idle", state == Statetype.idle);
        animator.SetBool("shake", state == Statetype.shake);
    }
}
