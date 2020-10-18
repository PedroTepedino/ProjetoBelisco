﻿using System;
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

        private void OnDestroy()
        {
            this.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}