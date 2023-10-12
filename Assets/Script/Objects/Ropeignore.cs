using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ropeignore : MonoBehaviour
{
    public Collider2D igCollider;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), igCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.GetComponent<Collider2D>(), igCollider, false);
        }
    }
}
