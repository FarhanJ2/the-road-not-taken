using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled) return;

        col.GetComponent<PlayerStats>().Heal(scorePerCollection);
        pickUpSound.Play();
        StartCoroutine(FadeOut());
    }
}
