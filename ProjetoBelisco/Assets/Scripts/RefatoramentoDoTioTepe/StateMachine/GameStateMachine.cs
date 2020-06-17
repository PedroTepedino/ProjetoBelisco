using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

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
            
            _stateMachine.SetState(menu);
            
            _stateMachine.AddTransition(menu, loading, () => PlayButton.LevelToLoad != null);
            _stateMachine.AddTransition(loading, play, loading.Finish);
            _stateMachine.AddTransition(play, pause, () => RewiredPlayerInput.Instance.PausePressed);
            _stateMachine.AddTransition(pause, play, () => RewiredPlayerInput.Instance.PausePressed);
            _stateMachine.AddTransition(pause, menu, () => RestartButton.Pressed);
        }

        private void Update()
        {
            _stateMachine.Tick();
            Debug.Log(PlayButton.LevelToLoad);
        }
    }

    public class Menu : IState
    {
        public void Tick()
        {
        }

        public void OnEnter()
        {
            PlayButton.LevelToLoad = null;
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void OnExit()
        {
        }
    }
    
    public class Pause : IState
    {
        public static bool Active { get; private set; }
        
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Active = true;
            Time.timeScale = 0f;
        }

        public void OnExit()
        {
            Active = false;
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
            
        }

        public void OnExit()
        {
            
        }
    }
    
    public class LoadLevel : IState
    {
        public bool Finish() => _operations.TrueForAll(t => t.isDone);
        
        private List<AsyncOperation> _operations = new List<AsyncOperation>();

        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            _operations.Add(SceneManager.LoadSceneAsync(PlayButton.LevelToLoad));
            _operations.Add(SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive));
        }

        public void OnExit()
        {
            
        }
    }
}