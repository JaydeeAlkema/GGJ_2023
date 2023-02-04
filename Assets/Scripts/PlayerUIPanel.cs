using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerUIPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private Image healthbarFillImage;
    [SerializeField] private List<Image> livesImages = new List<Image>();


    private int playerNumber = 0;
    private int health = 100;
    private int lives = 3;

    public TextMeshProUGUI PlayerText { get => playerText; set => playerText = value; }
    public int Health { get => health; set => health = value; }
    public int Lives { get => lives; set => lives = value; }
}
