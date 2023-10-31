using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RopeButton : MonoBehaviour, IPointerDownHandler
{
    public PlayerMain pMain;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(pMain.ropeNum == 0 && pMain.isRope)
        {
            pMain.ropeNum = 1f;
        }
        else if(pMain.ropeNum == 1 && pMain.isRope)
        {
            pMain.ropeNum = 0;
        }
    }

    void Update()
    {
        if(pMain.ropeNum == 1 && pMain.isRope)
        {
            pMain.ropeCheck = true;
        }

        if (pMain.ropeNum == 0 && pMain.isRope)
        {
            pMain.ropeCheck = false;
        }
    }
}
