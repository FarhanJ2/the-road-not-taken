using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject item;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource destroySound;
    [SerializeField] private int health;
    private bool hasItem = false;
    public int Health {get; private set; }
    // public int Health;
    private void Start()
    {
        Health = health;
        if (item != null)
            hasItem = true;
    }

    public void TakeDamage(int attackDamage)
    {
        Debug.Log("Crate hit with " + attackDamage + " damage");

        if (Health <= 0)
            return;

        if (hitSound != null && hitSounds.Length > 0)
        {
            hitSound.clip = hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)];
            hitSound.Play();
        }

        Health -= attackDamage;
        if (Health <= 0)
        {
            Break();
            Health = 0;
        }
    }

    private void Break()
    {
        Debug.Log("Crate broken");

        if (hasItem)
        {
            var createdItem = Instantiate(item, transform.position, Quaternion.identity);
            createdItem.AddComponent<BoxCollider2D>().isTrigger = true;
            // createdItem.AddComponent<Rigidbody2D>();
            createdItem.AddComponent<Key>();
        }

        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        destroySound.Play();
        Destroy(gameObject, 1.5f);
    }
}
