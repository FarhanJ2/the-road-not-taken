using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject[] hungerIndicators;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip song;

    [Header("Dialogue UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Screens")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private GameObject dialogueScreen;

    [Header("Animations")]
    [SerializeField] private Animator heartAnimator;
    [SerializeField] private Animator dialogueAnimator;

    private void Awake()
    {
        scoreText.text = PlayerStats.Score.ToString();
    }

    private void Start()
    {
        // this is in start because the player stats are not initialized in awake
        heartAnimator.SetFloat("health", PlayerStats.Health); 
    }

    private void OnEnable()
    {
        PlayerStats.onPlayerDamage += UpdateHealth;
        PlayerStats.onPlayerDeath += ToggleDeathScreen;
        PlayerStats.onScoreChange += UpdateScore;

        DialogueManager.onDialogueChange += UpdateDialogue;
        DialogueManager.onDialogueEnd += DisableDialogue;
    }

    private void OnDisable()
    {
        PlayerStats.onPlayerDamage -= UpdateHealth;
        PlayerStats.onPlayerDeath -= ToggleDeathScreen;
        PlayerStats.onScoreChange -= UpdateScore;

        DialogueManager.onDialogueChange -= UpdateDialogue;
        DialogueManager.onDialogueEnd -= DisableDialogue;
    }

    private void UpdateScore()
    {
        scoreText.text = PlayerStats.Score.ToString();
    }

    private void UpdateDialogue(string name, string dialogue)
    {
        dialogueScreen.SetActive(true);
        dialogueAnimator.Play("SizeOut");
        nameText.text = name;
        
        dialogueText.text = "";
        StartCoroutine(WaitForChar(dialogue));
    }

    private void DisableDialogue()
    {
        dialogueAnimator.Play("SizeIn");
        StartCoroutine(WaitForAnim());
    }

    IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(1f);
        dialogueScreen.SetActive(false);
    }

    IEnumerator WaitForChar(string dialogue)
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            dialogueText.text += dialogue[i];
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateHealth()
    {
        if (PlayerStats.Health <= 30)
        {
            if (!music.isPlaying)
            {
                music.clip = song;
                music.Play();
            }
        }

        heartAnimator.SetFloat("health", PlayerStats.Health);
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