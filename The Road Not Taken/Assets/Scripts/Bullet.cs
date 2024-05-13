using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        // maybe to fix check who shot
        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(col);
            col.gameObject.GetComponent<Enemy>().TakeDamage(10);

            Destroy(gameObject);
        }
        else if (col.gameObject.CompareTag("Wall"))
            Destroy(gameObject);
        else if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerStats>().TakeDamage(10);
        }
    }
}
