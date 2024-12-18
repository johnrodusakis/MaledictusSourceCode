using System;

namespace Maledictus.StateMachine
{
    public class StateTransition
    {
        public IState From;
        public IState To;
        public Func<bool> Condition;

        public StateTransition(IState from, IState to, Func<bool> condition)
        {
            From = from;
            To = to;
            Condition = condition;
        }

        public StateTransition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}