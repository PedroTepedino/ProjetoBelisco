using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class AttackDummy : BaseLifeSystem
{
    [SerializeField] private Light2D _light;

    Tween animation;

    public System.Action<int, int> OnLifeChange;

    [Sirenix.OdinInspector.Button]
    public override void Damage(int damagePoints = 1)
    {
        DamageFeedBack();
        base.Damage(damagePoints);
        OnLifeChange?.Invoke(_curentHealthPoints, _maximumHealth);
    }

    protected override void Die()
    {
        RestoreHealth(_maximumHealth);
        OnLifeChange?.Invoke(_curentHealthPoints, _maximumHealth);
    }

    private void DamageFeedBack()
    {
        if (animation == null)
        {
            animation = DOTween.To(() => _light.intensity, x => _light.intensity = x, 1f, 0.1f).SetLoops(2,LoopType.Yoyo).From(0).SetAutoKill(false);
        }

        animation.Restart();
    }
}
