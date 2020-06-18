using UnityEngine;
using UnityEngine.UI;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour
    {
        public static string LevelToLoad;

        [SerializeField] private string _levelName;

        private void Awake()
        {
            this.GetComponent<Button>().onClick.AddListener(() => LevelToLoad = _levelName);
        }
    }
}