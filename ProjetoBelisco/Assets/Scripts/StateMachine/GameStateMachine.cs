using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Belisco
{
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private bool _testingOptions = false;
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

            Menu menu = new Menu();
            LoadLevel loading = new LoadLevel();
            Play play = new Play();
            Pause pause = new Pause();
            Options options = new Options();
            Quit quit = new Quit();
            WinState win = new WinState();

            if (!_testingOptions)
            {
                _stateMachine.SetState(menu);
            }
            else
            {
                _stateMachine.SetState(play);
            }

            _stateMachine.AddTransition(menu, loading, () => LoadLevel.LevelToLoad != null);

            _stateMachine.AddTransition(play, loading, () => LoadLevel.LevelToLoad != null);
            _stateMachine.AddTransition(loading, play, loading.Finish);

            _stateMachine.AddTransition(play, pause, () => RewiredPlayerInput.Instance.PausePressed);
            _stateMachine.AddTransition(pause, play, () => RewiredPlayerInput.Instance.PausePressed);
            _stateMachine.AddTransition(pause, play, () => PauseButton.Pressed);

            _stateMachine.AddTransition(pause, menu, () => RestartButton.Pressed);

            _stateMachine.AddTransition(pause, options, () => OptionsButton.Pressed);
            _stateMachine.AddTransition(options, pause,
                () => OptionsButton.Pressed && _stateMachine.LastState is Pause);
            _stateMachine.AddTransition(options, pause,
                () => RewiredPlayerInput.Instance.PausePressed && _stateMachine.LastState is Pause);

            _stateMachine.AddTransition(menu, options, () => OptionsButton.Pressed);
            _stateMachine.AddTransition(options, menu, () => OptionsButton.Pressed && _stateMachine.LastState is Menu);
            _stateMachine.AddTransition(options, menu,
                () => RewiredPlayerInput.Instance.PausePressed && _stateMachine.LastState is Menu);

            _stateMachine.AddTransition(menu, quit, () => QuitButton.Pressed);

            _stateMachine.AddTransition(play, win, () => WinArea.HasWon);
            _stateMachine.AddTransition(win, menu, () => RestartButton.Pressed);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }

        public static event Action<IState> OnGameStateChanged;
    }

    public class Menu : IState
    {
        public void Tick()
        {
        }

        public void OnEnter()
        {
            Time.timeScale = 1f;
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
            Time.timeScale = 1f;
            FadeInOutSceneTransition.Instance.FadeOut();
        }

        public void OnExit()
        {
            PlayerRespawner.CurrentSpawner = null;
        }
    }

    public class LoadLevel : IState
    {
        public static string LevelToLoad;

        private readonly List<AsyncOperation> _operations = new List<AsyncOperation>();

        public void Tick()
        {
            if (FadeInOutSceneTransition.Instance.FadeInCompleted)
                _operations.ForEach(t => t.allowSceneActivation = true);
        }

        public void OnEnter()
        {
            FadeInOutSceneTransition.Instance.FadeIn();
            
            _operations.Add(SceneManager.LoadSceneAsync(LevelToLoad));
            _operations.Add(SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive));
            _operations.Add(SceneManager.LoadSceneAsync("MapSetup", LoadSceneMode.Additive));
 
            _operations.ForEach(t => t.allowSceneActivation = false);
        }

        public void OnExit()
        {
            LevelToLoad = null;
        }

        public bool Finish()
        {
            return _operations.TrueForAll(t => t.isDone && t.allowSceneActivation);
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