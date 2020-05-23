using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyDeadCourpse : MonoBehaviour, IPooledObject
{
    [SerializeField] private List<SpriteRenderer> _parts;
    [SerializeField] private List<DOTweenAnimation> _animations;
    
    public void OnObjectSpawn()
    {
        foreach (var part in _parts)
        {
            part.transform.localPosition = Vector3.zero;
        }

        foreach (var animation in _animations)
        {
            animation.DORestart();
        }
    }

    public void FadeDisable()
    {
        this.gameObject.SetActive(false);
    }
}
