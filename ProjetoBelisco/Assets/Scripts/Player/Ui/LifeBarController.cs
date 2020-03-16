using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class LifeBarController : MonoBehaviour
{
    [SerializeField] [Required] private Image _barFillImage;

    private void Awake()
    {
        PlayerLife.OnPlayerDamage += ListenDamage;
    }

    private void OnDestroy()
    {
        PlayerLife.OnPlayerDamage -= ListenDamage;
    }

    private void ListenDamage(int curentHealth, int maxHealth)
    {
        _barFillImage.fillAmount = (float)(curentHealth) / (float)(maxHealth);
    }
}
