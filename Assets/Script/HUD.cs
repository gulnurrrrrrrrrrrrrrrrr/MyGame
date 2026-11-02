using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Player2D player;
    public Health health;
    public TMP_Text coinText;
    public Image[] hearts;

    void Update()
    {
        coinText.text = player.Coins + " / " + player.CoinsTotal;
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < health.hp;
    }
}