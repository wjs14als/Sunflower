using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightLeft : MonoBehaviour
{
    public ScrollMenu menu;
    public float scrollvalue;

    public void OnClickLeft()
    {
        menu.scroll_pos -= scrollvalue;
    }

    public void OnClickRight()
    {
        menu.scroll_pos += scrollvalue;
    }
}
