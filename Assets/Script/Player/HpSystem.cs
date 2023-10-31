using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpSystem : MonoBehaviour
{
    public static int hp;

    public GameObject[] hearts;

    private void Start()
    {
        hp = 3;
    }

    private void Update()
    {
        foreach(GameObject sunflower in hearts)
        {
            sunflower.SetActive(false);
        }
        for(int i = 0; i < hp; i++)
        {
            hearts[i].SetActive(true);
        }
    }
}
