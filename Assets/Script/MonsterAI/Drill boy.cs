using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drillboy : MonoBehaviour
{
    public bool isShake = false;

    public float shakeCool = 2;
    public float shakeTimer = 0;

    private Transform player;
    private CinemachineImpulseSource impulseSource;
    private void Start()
    {
        player = PlayerPos.Instance.Player.transform;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (isShake)
        {
            shakeTimer += Time.deltaTime;
            if (shakeTimer > shakeCool)
            {
                CameraShake.instance.cameraShake(impulseSource);
                shakeTimer = 0;
            }
        }
        else shakeTimer = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isShake = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isShake = false;
        }
    }
}
