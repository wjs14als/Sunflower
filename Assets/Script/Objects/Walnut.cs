using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walnut : MonoBehaviour
{
    public bool isArea = false;

    private void Update()
    {
        if (isArea)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("walnutArea"))
        {
            isArea = true;
        }
    }
}
