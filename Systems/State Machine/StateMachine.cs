using System;
using System.Collections.Generic;

namespace Maledictus.StateMachine
{
    public class StateMachine
    {
        private readonly List<StateTransition> stateTransitions = new();
        private readonly List<StateTransition> anyStateTransitions = new();

        private IState currentState;

        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            var stateTransition = new StateTransition(from, to, condition);
            stateTransitions.Add(stateTransition);
        }
        public void AddTransition(IState to, Func<bool> condition)
        {
            var stateTransition = new StateTransition(null, to, condition);
            anyStateTransitions.Add(stateTransition);
        }

        public void SetState(IState state)
        {
            if (currentState == state)
                return;

            currentState?.OnExit();

            currentState = state;
            // Debug.Log($"Changed to state {state}");
            currentState.OnEnter();
        }

        public void Tick()
        {
            var transition = CheckForTransition();

            if (transition != null)
                SetState(transition.To);

            currentState.Tick();
        }

        private StateTransition CheckForTransition()
        {
            foreach (var transition in anyStateTransitions)
            {
                if (transition.Condition())
                    return transition;
            }

            foreach (var transition in stateTransitions)
            {
                if (transition.From == currentState && transition.Condition())
                    return transition;
            }

            return null;
        }
    }
}