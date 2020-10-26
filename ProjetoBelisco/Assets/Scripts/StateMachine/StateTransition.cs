using System;

namespace Belisco
{
    public class StateTransition
    {
        public readonly Func<bool> Condition;
        public readonly IState From;
        public readonly IState To;

        public StateTransition(IState from, IState to, Func<bool> condition)
        {
            From = from;
            To = to;
            Condition = condition;
        }
    }
}