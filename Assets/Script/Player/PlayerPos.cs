using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    public GameObject Player;
    public static PlayerPos Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerPos>();
                if(instance == null)
                {
                    var instanceContainer = new GameObject("PlayerData");
                    instance = instanceContainer.AddComponent<PlayerPos>();
                }
            }
            return instance;
        }
    }
    private static PlayerPos instance;
}
