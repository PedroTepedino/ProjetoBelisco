using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private string _levelName;

        private void Awake()
        {
            this.GetComponent<Button>().onClick.AddListener(() => LoadLevel.LevelToLoad = _levelName);
        }
    }
}