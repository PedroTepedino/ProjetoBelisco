﻿using System;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace RefatoramentoDoTioTepe
{
    public class FadeInOutSceneTransition : MonoBehaviour
    {
        public static FadeInOutSceneTransition Instance;

        [SerializeField] private float _fadeInOutTime = 1.0f;
        [SerializeField] private Ease _ease;
        [SerializeField] private Volume _volume;

        private Tween FadeAnimation;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            FadeAnimation = DOTween.To(() => _volume.weight, value => _volume.weight = value, 1f, _fadeInOutTime)
                .SetAutoKill(false).SetEase(_ease).From(0f);
            FadeAnimation.Rewind();
        }

        public static void LoadFadeScene()
        {
            SceneManager.LoadSceneAsync("FadeInOutScene", LoadSceneMode.Additive);
        }

        public void FadeIn()
        {
            FadeAnimation.Restart();
        }

        public void FadeOut()
        {
            FadeAnimation.SmoothRewind();
        }

        private void OnValidate()
        {
            if (_volume == null)
            {
                _volume = this.GetComponent<Volume>();
            }
        }
    }
}