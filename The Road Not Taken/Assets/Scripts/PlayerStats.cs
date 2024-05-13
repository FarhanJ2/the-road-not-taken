using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static int Health { get; private set; }
    public static int Hunger { get; private set; }
    public static int Score { get; set; }
    public static int MAX_HEALTH = 100;
    public static int MAX_HUNGER = 30;

    [SerializeField] private AudioSource deathGroan;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioClip[] hitSounds;

    // events out!!!
    public delegate void OnScoreChange();
    public static event OnScoreChange onScoreChange;

    public delegate void OnPlayerDamage();
    public static event OnPlayerDamage onPlayerDamage;

    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath onPlayerDeath;

    private Transform transform;

    private void OnEnable(){}
    private void OnDisable(){}

    private void Awake()
    {
        Health = MAX_HEALTH;
        Hunger = MAX_HUNGER;
        Score = 0;

        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Debug.Log("Key down");
        //     TakeDamage();
        // }
    }

    private void Attack()
    {
        // attack player
        if (PlayerMovement.disabled)
            return;


    }

    private void HungerManager()
    {
        if (Hunger / MAX_HUNGER < 0.3)
        {
            while (Hunger < MAX_HUNGER)
            {
                StartCoroutine(WaitForHungerIncrease());
                IEnumerator WaitForHungerIncrease()
                {
                    Debug.Log("Yielding...");
                    yield return new WaitForSeconds(3);
                    Debug.Log("Damaging Player for Hunger");
                    TakeDamage(5);
                }
            }
        }
    }

    private void ChangeScore(int scoreChange)
    {
        Score += scoreChange;
        onScoreChange?.Invoke();
    }

    public void TakeDamage(int attackDamage = 10)
    {
        if (Health <= 0)
            return;

        if (hitSound != null && hitSounds.Length > 0)
        {
            hitSound.clip = hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)];
            hitSound.Play();
        }

        Health -= attackDamage;
        Debug.Log(Health);
        onPlayerDamage?.Invoke();

        if (Health <= 0)
        {
            Die();
            Health = 0;
        }
    }

    private void Die()
    {
        if (deathGroan != null)
            deathGroan.Play();

        onPlayerDeath?.Invoke();
    }

    public void Reload()
    {
        // transform.position = PlayerMovement.deathPos;
        PlayerMovement.disabled = false;
        Health = MAX_HEALTH;
        Hunger = MAX_HUNGER;
        onPlayerDamage?.Invoke(); // update health bar on reload
        Score = 0;

        transform.position = PlayerMovement.spawnPoint;
    }

}
