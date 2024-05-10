using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int Health { get; private set; }
    public int Hunger { get; private set;}
    private int maxHealth = 100;
    private int damage = 10;
    private int score = 0;

    private void Awake() {
        Health = maxHealth;
        Hunger = 100;
    }

    private void Update() {

    }

}
