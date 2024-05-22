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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (BulletType == BulletType.Player && col.gameObject.CompareTag("Player"))
            return;
        
        if (BulletType == BulletType.Enemy && col.gameObject.CompareTag("Enemy"))
            return;

        if (col.gameObject.CompareTag("Enemy"))
        {
            // Debug.Log(col);
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
        else if (col.gameObject.CompareTag("Crate"))
        {
            if (BulletType == BulletType.Enemy) return;
            col.gameObject.GetComponent<Crate>().TakeDamage(Damage);
            Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
