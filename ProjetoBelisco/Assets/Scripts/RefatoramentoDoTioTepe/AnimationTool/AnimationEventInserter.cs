using System;
using Sirenix.OdinInspector;
using UnityEngine;

    
[RequireComponent(typeof(Animator))]
public class AnimationEventInserter : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [SerializeField] private AnimationClip _animationClip;
    [ShowInInspector, ReadOnly] private int _clipIndex = -1;
    [SerializeField] private float _time;
    [SerializeField] private string _functionName;
    
    private void Awake()
    {
        for (var index = 0; index < _animator.runtimeAnimatorController.animationClips.Length; index++)
        {
            AnimationClip animationClip = _animator.runtimeAnimatorController.animationClips[index];
            Debug.Log(animationClip.name + "   " + index);
        }
        
    }

    private void AddAnimationEvent(int Clip, float time, string functionName)
    {
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = functionName;
    }

    private void OnValidate()
    {
        if (_animationClip != null && _animator != null)
        {
            for (var index = 0; index < _animator.runtimeAnimatorController.animationClips.Length; index++)
            {
                var clip = _animator.runtimeAnimatorController.animationClips[index];
                if (clip.name == _animationClip.name)
                {
                    _clipIndex = index;
                }
            }
        }
    }
}
