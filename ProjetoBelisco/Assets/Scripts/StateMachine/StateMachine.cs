using System;
using System.Collections.Generic;
using UnityEngine;

namespace Belisco
{
    public class StateMachine
    {
        private readonly List<StateTransition> _anyStateTransitions = new List<StateTransition>();

        private readonly Dictionary<IState, List<StateTransition>> _stateTransitions =
            new Dictionary<IState, List<StateTransition>>();

        public IState LastState { get; private set; }

        public IState CurrentState { get; private set; }

        public event Action<IState> OnStateChanged;

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            StateTransition stateTransition = new StateTransition(null, to, condition);
            _anyStateTransitions.Add(stateTransition);
        }

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            if (_stateTransitions.ContainsKey(from) == false)
                _stateTransitions[from] = new List<StateTransition>();

            StateTransition stateTransition = new StateTransition(from, to, condition);
            _stateTransitions[from].Add(stateTransition);
        }

        public void SetState(IState state)
        {
            if (CurrentState == state)
                return;

            CurrentState?.OnExit();

            LastState = CurrentState;
            CurrentState = state;
            Debug.Log($"Change to {state}");
            CurrentState.OnEnter();

            OnStateChanged?.Invoke(CurrentState);
        }

        public void Tick()
        {
            StateTransition transition = CheckForTransition();

            if (transition != null) SetState(transition.To);

            CurrentState.Tick();
        }

        private StateTransition CheckForTransition()
        {
            foreach (StateTransition transition in _anyStateTransitions)
                if (transition.Condition())
                    return transition;

            if (_stateTransitions.ContainsKey(CurrentState))
                foreach (StateTransition transition in _stateTransitions[CurrentState])
                    if (transition.From == CurrentState && transition.Condition())
                        return transition;

            return null;
        }
    }
}