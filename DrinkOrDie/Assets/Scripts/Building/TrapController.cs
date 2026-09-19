using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : Building
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().MoveSpeed *= 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().MoveSpeed *= 2f;
        }
    }
}
