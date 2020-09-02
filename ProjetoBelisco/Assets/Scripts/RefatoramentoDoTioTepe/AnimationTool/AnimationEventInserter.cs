using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEventInserter : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] [InlineEditor(InlineEditorObjectFieldModes.Hidden)] private AnimationEventInfoScriptableObject _eventInfos;
    
    private void Awake()
    {
        for (int i = 0; i < _eventInfos._animationEventInfos.Count; i++)
        {
            AddAnimationEvent(_eventInfos._animationEventInfos[i]);
        }
    }

    private void AddAnimationEvent(AnimationEventInfo eventInfo)
    {
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = eventInfo.FunctionName;
        animationEvent.time = eventInfo.Time;
        var clip = _animator.runtimeAnimatorController.animationClips[eventInfo.ClipIndex];
        clip.AddEvent(animationEvent);
    }
}