using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private UnityEvent playerEvent;
    [SerializeField] private UnityEvent enemyEvent;
    private Color gizmoColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Entered player trigger");
            playerEvent?.Invoke();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Entered enemy trigger");
            enemyEvent?.Invoke();
        }
    }
}
