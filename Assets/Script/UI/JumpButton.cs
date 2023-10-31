using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerMain pMain;
    public void OnPointerDown(PointerEventData eventData)
    {
        pMain.jumpButton = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pMain.jumpButton = false;
    }

    void Update()
    {
        if (pMain.jumpButton == true)
        {
            pMain.JumpTo();
        }
        
        if (pMain.jumpButton == false)
        {
            pMain.isJumping = false;
            pMain.jumpNum = 0;
            pMain.jumpCounter = 0;
        }
    }
}
