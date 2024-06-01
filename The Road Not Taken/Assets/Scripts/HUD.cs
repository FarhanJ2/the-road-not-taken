using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreText2;
    [SerializeField] private TMP_Text topScore;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject[] hungerIndicators;
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioClip song;

    [Header("Dialogue UI")]
    // [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Screens")]
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject scoreScreen;
    [SerializeField] private GameObject dialogueScreen;
    [SerializeField] private GameObject levelScreen;
    [SerializeField] private GameObject bloodVignette;

    [Header("Animations")]
    [SerializeField] private Animator heartAnimator;
    [SerializeField] private Animator dialogueAnimator;
    [SerializeField] private Animator levelAnimator;
    private Color bloodVignetteColor;

    private void Awake()
    {
        scoreText.text = PlayerStats.Score.ToString();
    }

    private void Start()
    {
        // this is in start because the player stats are not initialized in awake
        heartAnimator.SetFloat("health", PlayerStats.Health);
        Time.timeScale = 0;

        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].SetActive(false);
        }
        for (int i = 0; i < hungerIndicators.Length; i++) {
            hungerIndicators[i].SetActive(false);
        }
    }
    private void Update()
    {
        bloodVignette.GetComponent<Image>().color = Color.Lerp(bloodVignette.GetComponent<Image>().color, bloodVignetteColor, Time.deltaTime * 2f);
    }

    private void OnEnable()
    {
        PlayerStats.onPlayerDamage += UpdateHealth;
        PlayerStats.onPlayerDeath += ToggleDeathScreen;
        PlayerStats.onScoreChange += UpdateScore;

        DialogueManager.onDialogueChange += UpdateDialogue;
        DialogueManager.onDialogueEnd += DisableDialogue;

        StoryManager.onLevelChange += UpdateLevel;
    }

    private void OnDisable()
    {
        PlayerStats.onPlayerDamage -= UpdateHealth;
        PlayerStats.onPlayerDeath -= ToggleDeathScreen;
        PlayerStats.onScoreChange -= UpdateScore;

        DialogueManager.onDialogueChange -= UpdateDialogue;
        DialogueManager.onDialogueEnd -= DisableDialogue;

        StoryManager.onLevelChange -= UpdateLevel;
    }

    private void UpdateScore()
    {
        scoreText.text = PlayerStats.Score.ToString();
        scoreText2.text = PlayerStats.Score.ToString();
        // topScore.text = playerPrefs.GetInt("HighScore", 0);
    }

    private void UpdateDialogue(string name, string dialogue)
    {
        dialogueScreen.SetActive(true);
        dialogueAnimator.Play("SizeOut");
        // nameText.text = name;
        
        StopAllCoroutines();
        StartCoroutine(WaitForChar(dialogue));
    }

    IEnumerator WaitForChar(string dialogue)
    {
        // for (int i = 0; i < dialogue.Length; i++)
        // {
        //     dialogueText.text += dialogue[i];
        //     yield return new WaitForSeconds(0.1f);
        // }

        dialogueText.text = "";
        foreach (char letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            // yield return null; doesnt work???? why
            yield return new WaitForSeconds(.05f);
        }
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

    private void UpdateHealth()
    {
        if (PlayerStats.Health <= 30)
        {
            // if (!music.isPlaying)
            // {
            //     music.clip = song;
            //     music.Play();
            // }

            music.clip = song;
            music.Play();
        }

        heartAnimator.SetFloat("health", PlayerStats.Health);
        bloodVignetteColor = new Color(255, 255, 255, .75f - (float)((float)PlayerStats.Health / (float)PlayerStats.MAX_HEALTH));
        // bloodVignette.GetComponent<Image>().color = new Color(255, 255, 255, .75f - (float)((float)PlayerStats.Health / (float)PlayerStats.MAX_HEALTH));
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

    private void UpdateLevel(string levelName)
    {
        levelScreen.SetActive(true);
        levelText.text = levelName;
        levelAnimator.Play("SlideIn");
        StartCoroutine(WaitForLevelAnim());
    }

    private IEnumerator WaitForLevelAnim()
    {
        yield return new WaitForSeconds(1.5f);
        levelScreen.SetActive(false);
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

    public void StartGame()
    {
        Time.timeScale = 1f;
        menuScreen.SetActive(false);
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].SetActive(true);
        }
        for (int i = 0; i < hungerIndicators.Length; i++) {
            hungerIndicators[i].SetActive(true);
        }
    }
}