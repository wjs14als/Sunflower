using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool check;
    public PlayerMain pMain;
    public void OnPointerDown(PointerEventData eventData)
    {
        check = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        check = false;
    }

    void Update()
    {
        if (check == true)
        {
            pMain.JumpTo();
        }
        else if (check == false)
        {
            pMain.isJumping = false;
        }
    }
}
