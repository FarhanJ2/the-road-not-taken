using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Player,
    Enemy
}

public class Bullet : MonoBehaviour
{
    public int Damage { get; set; }
    public BulletType BulletType { get; set; }

    private void Update()
    {
        Debug.Log(transform.rotation);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (BulletType == BulletType.Player && col.gameObject.CompareTag("Player"))
            return;
        
        if (BulletType == BulletType.Enemy && col.gameObject.CompareTag("Enemy"))
            return;

        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log(col);
            // col.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
            Destroy(gameObject);
            return;
        }
        else if (col.gameObject.CompareTag("Wall"))
            Destroy(gameObject);
        else if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerStats>().TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
