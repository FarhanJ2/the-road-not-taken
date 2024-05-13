using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject[] hungerIndicators;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject scoreScreen;

    private void Awake()
    {
        scoreText.text = PlayerStats.Score.ToString();
    }

    private void OnEnable()
    {
        PlayerStats.onPlayerDamage += UpdateHealth;
        PlayerStats.onPlayerDeath += ToggleDeathScreen;
        PlayerStats.onScoreChange += UpdateScore;
    }

    private void OnDisable()
    {
        PlayerStats.onPlayerDamage -= UpdateHealth;
        PlayerStats.onPlayerDeath -= ToggleDeathScreen;
        PlayerStats.onScoreChange -= UpdateScore;
    }

    private void UpdateScore()
    {
        scoreText.text = PlayerStats.Score.ToString();
    }

    private void UpdateHealth()
    {
        float healthPerHeart = PlayerStats.MAX_HEALTH / hearts.Length;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (PlayerStats.Health >= healthPerHeart * (i + 1))
            {
                hearts[i].SetActive(true);
                hearts[i].GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            }
            else
            {
                hearts[i].SetActive(true);
                float alpha = Mathf.Lerp(0f, 1f, PlayerStats.Health / (healthPerHeart * (i + 1)));
                hearts[i].GetComponent<Image>().color = new Color(255, 255, 255, alpha);
            }
        }
    }

    private void UpdateHunger()
    {
        float hungerPerCapsule = PlayerStats.MAX_HUNGER / hungerIndicators.Length;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (PlayerStats.Hunger >= hungerPerCapsule * (i + 1))
            {
                hearts[i].SetActive(true);
                hearts[i].GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            }
            else
            {
                hearts[i].SetActive(true);
                float alpha = Mathf.Lerp(0f, 1f, PlayerStats.Hunger / (hungerPerCapsule * (i + 1)));
                hearts[i].GetComponent<Image>().color = new Color(255, 255, 255, alpha);
            }
        }
    }

    private void ToggleDeathScreen()
    {
        bool menuOpen = !deathScreen.activeSelf;
        deathScreen.SetActive(menuOpen);
    }

    public void ToggleScoreScreen()
    {
        bool menuOpen = !scoreScreen.activeSelf;
        PlayerMovement.disabled = menuOpen;
        scoreScreen.SetActive(menuOpen);
    }

    public void ContinueGame()
    {
        deathScreen.SetActive(false);
        playerStats.Reload();
    }
}