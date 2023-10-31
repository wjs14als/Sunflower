using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    [SerializeField] private float globalShakeForce = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void cameraShake(CinemachineImpulseSource source)
    {
        source.GenerateImpulseWithForce(globalShakeForce);
    }
}
