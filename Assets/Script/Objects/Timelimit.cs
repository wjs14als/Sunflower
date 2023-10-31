using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Timelimit : MonoBehaviour
{
    public float platformTime = 4;
    public float currentTime = 0;
    public float ableNum = 0;
    public bool isPlayer;
    public bool enable;

    private void Start()
    {
        enable = true;
    }

    private void Update()
    {
        if(isPlayer && ableNum < 2)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= platformTime)
            {
                currentTime = 0;
                togglePlatform();
            }
        }
        else
        {
            ableNum = 0;
            isPlayer = false;
        }
    }

    void togglePlatform()
    {
        enable = !enable;
        foreach(Transform child in gameObject.transform)
        {
            if(child.tag != "Player")
            {
                child.gameObject.SetActive(enabled);
            }
        }
        ableNum++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayer = true;
        }
    }
}
