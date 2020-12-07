using UnityEngine;
using UnityEngine.UI;

namespace Belisco
{
    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private string _levelName;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => LoadLevel.LevelToLoad = _levelName);
        }

        private void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}