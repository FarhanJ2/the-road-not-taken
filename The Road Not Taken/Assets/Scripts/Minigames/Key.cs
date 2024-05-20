using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (!enabled) return;

        if (ForestSearch.Instance.GameState == ForestSearch.State.FoundKey || ForestSearch.Instance.GameState == ForestSearch.State.FoundAnotherKey)
            ForestSearch.Instance.GameState = ForestSearch.State.FoundAnotherKey;
        else
            ForestSearch.Instance.GameState = ForestSearch.State.FoundKey;

        pickUpSound?.Play();
        StartCoroutine(FadeOut());
    }
}
