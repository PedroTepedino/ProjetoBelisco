using UnityEngine;

public class SceneNameObserver : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        this.gameObject.name = this.gameObject.scene.name;
    }
#endif
}