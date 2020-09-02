#if UNITY_EDITOR
using System.Collections;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;


public class AnimationChooser : OdinEditorWindow
{
    [SerializeField] private Animator _animator;


    [SerializeField] [ValueDropdown("GetAllClips")] [PreviewField] private AnimationClip _animationClip;

    [SerializeField] private int _frame = -1;
    private float _time = -1f;

    [SerializeField] private string _functionName;
    [SerializeField] private AnimationEventInfoScriptableObject _animationEventInfoScriptableObject;


    [MenuItem("Tools/Animation Event Incerter", priority = -10000)]
    private static void OpenWindow()
    {
        GetWindow<AnimationChooser>().Show();
    }

    private IEnumerable GetAllClips()
    {
        return _animator != null ? _animator.runtimeAnimatorController.animationClips : null;
    }

    [Button]
    public void AddAnimationEvent()
    {
        if (_frame < 0 || _functionName == null || _animationClip == null)
            return;

        _time = (float) _frame / 60f;

        var eventInfo = new AnimationEventInfo();
        eventInfo.ClipIndex = GetClipIndex(_animationClip);
        eventInfo.Time = _time;
        eventInfo.FunctionName = _functionName;

        if (eventInfo.ClipIndex < 0)
            return;
            
        _animationEventInfoScriptableObject.AddAnimationEventToList(eventInfo);
    }

    private int GetClipIndex(AnimationClip animationClip)
    {
        for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (_animationClip == _animator.runtimeAnimatorController.animationClips[i])
                return i;
        }

        return -1;
    }
}
#endif