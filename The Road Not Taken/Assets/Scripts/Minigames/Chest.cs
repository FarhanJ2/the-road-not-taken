using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Chest : MonoBehaviour
{
    [SerializeField] private AudioSource openChest;
    [SerializeField] private GameObject[] chestModels = new GameObject[2];
    [SerializeField] private GameObject item;
    [SerializeField] private bool isOpen = false;

    [SerializeField] private UnityEvent preRequToOpenChest;

    // private enum State
    // {
    //     Uninitialized,
    //     Closed,
    //     Opened,
    //     Empty,
    // }

    private void Awake()
    {
        if (isOpen)
        {
            chestModels[0].SetActive(false);
            chestModels[1].SetActive(true);
        }
        else
        {
            chestModels[0].SetActive(true);
            chestModels[1].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isOpen) return; // if chest is already open, do nothing
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with player");
            isOpen = true;

            openChest?.Play();
            chestModels[0].SetActive(false); // switch chest model to open
            chestModels[1].SetActive(true);

            if (item != null)
            {
                GameObject createdItem = Instantiate(item, transform.position - new Vector3(0, 1.25f, 0), Quaternion.identity);
                // createdItem.AddComponent<BoxCollider2D>().isTrigger = true;
                // createdItem.AddComponent<Rigidbody2D>();
            }
        }
    }
}
