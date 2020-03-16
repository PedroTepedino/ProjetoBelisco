using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class LifeBarController : MonoBehaviour
{
    [SerializeField] [Required] private Image _barFillImage;
    [SerializeField] private DOTweenAnimation _shakeAnimaiton;

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

        ShakeBar();
    }

    private void ShakeBar()
    {
        _shakeAnimaiton?.DORestart();
    }


}
