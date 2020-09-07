﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RefatoramentoDoTioTepe
{
    public class GameStateMachine : MonoBehaviour
    {
        public static event Action<IState> OnGameStateChanged;
        
        private static GameStateMachine _instance;
        
        private StateMachine _stateMachine;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            _stateMachine = new StateMachine();
            _stateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
            
            var menu = new Menu();
            var loading = new LoadLevel();
            var play = new Play();
            var pause = new Pause();
            var options = new Options();
            var quit = new Quit();
            var win = new WinState();
            
            _stateMachine.SetState(menu);

            _stateMachine.AddTransition(menu, loading, () => LoadLevel.LevelToLoad != null);
            
            _stateMachine.AddTransition(play, loading, () => LoadLevel.LevelToLoad != null);
            _stateMachine.AddTransition(loading, play, loading.Finish);

            _stateMachine.AddTransition(play, pause, () => RewiredPlayerInput.Instance.PausePressed);
            _stateMachine.AddTransition(pause, play, () => RewiredPlayerInput.Instance.PausePressed);
            _stateMachine.AddTransition(pause, play, () => PauseButton.Pressed);
            
            _stateMachine.AddTransition(pause, menu, () => RestartButton.Pressed);
            
            _stateMachine.AddTransition(pause, options, () => OptionsButton.Pressed);
            _stateMachine.AddTransition(options, pause, () => OptionsButton.Pressed && _stateMachine.LastState is Pause);
            _stateMachine.AddTransition(options,  pause, () => RewiredPlayerInput.Instance.PausePressed && _stateMachine.LastState is Pause);
            
            _stateMachine.AddTransition(menu,  options, () => OptionsButton.Pressed);
            _stateMachine.AddTransition(options,  menu, () => OptionsButton.Pressed && _stateMachine.LastState is Menu);
            _stateMachine.AddTransition(options, menu, () => RewiredPlayerInput.Instance.PausePressed && _stateMachine.LastState is Menu);
            
            _stateMachine.AddTransition(menu, quit, () => QuitButton.Pressed);
            
            _stateMachine.AddTransition(play, win, () => WinArea.HasWon);
            _stateMachine.AddTransition(win, menu, () => RestartButton.Pressed);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }
    }

    public class Menu : IState
    {
        public void Tick()
        {
        }

        public void OnEnter()
        {
            LoadLevel.LevelToLoad = null;
            SceneManager.LoadSceneAsync("Menu");
        }

        public void OnExit()
        {
        }
    }
    
    public class Pause : IState
    {
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Time.timeScale = 0f;
        }

        public void OnExit()
        {
            Time.timeScale = 1f;
        }
    }
    
    public class Play : IState
    {
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            FadeInOutSceneTransition.Instance.FadeOut();
        }

        public void OnExit()
        {
            
        }
    }
    
    public class LoadLevel : IState
    {
        public static string LevelToLoad;
        public bool Finish() => _operations.TrueForAll(t => t.isDone && t.allowSceneActivation == true);
        
        private List<AsyncOperation> _operations = new List<AsyncOperation>();

        public void Tick()
        {
            if (FadeInOutSceneTransition.Instance.FadeInCompleted)
            {
                _operations.ForEach(t => t.allowSceneActivation = true);
            }
        }

        public void OnEnter()
        {
            FadeInOutSceneTransition.Instance.FadeIn();
            _operations.Add(SceneManager.LoadSceneAsync(LevelToLoad));
            _operations.Add(SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive));
            _operations.ForEach(t => t.allowSceneActivation = false);
        }

        public void OnExit()
        {
            LevelToLoad = null;
        }
    }

    public class Options : IState
    {
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            SaveOptions.SaveAllOptions();
        }
    }

    public class Quit : IState
    {
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Application.Quit();
        }

        public void OnExit()
        {
        }
    }

    public class WinState : IState
    {
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Time.timeScale = 0f;
        }

        public void OnExit()
        {
            Time.timeScale = 1f;
        }
    }
}