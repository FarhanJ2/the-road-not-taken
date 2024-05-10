using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private RawImage hungerIndicator;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject[] hearts;
}