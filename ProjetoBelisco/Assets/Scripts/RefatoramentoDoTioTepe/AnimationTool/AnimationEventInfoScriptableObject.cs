using System.Collections.Generic;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Create AnimationEventInfoScriptableObject", fileName = "AnimationEventInfoScriptableObject", order = 0)]
[System.Serializable]
public class AnimationEventInfoScriptableObject : ScriptableObject
{
    public List<AnimationEventInfo> _animationEventInfos;

    public void AddAnimationEventToList(AnimationEventInfo eventInfo)
    {
        if (_animationEventInfos == null)
        {
            _animationEventInfos = new List<AnimationEventInfo>();
        }
        
        _animationEventInfos.Add(eventInfo);
    }
}