using System.Collections.Generic;
using Entities.Enemies;

namespace FiniteStateMachine
{
    public class StateMachine
    {
        public bool Enabled = true;
    
        public readonly Enemy Owner;
        
        private readonly Dictionary<string, BaseState> _states;
        private BaseState _actualState;

        public StateMachine(Enemy owner)
        {
            Owner = owner;
            _states = new Dictionary<string, BaseState>();
        }
    
        public void CreateState(string name, BaseState state)
        {
            _states.TryAdd(name, state);
        }

        public void Execute()
        {
            if (Enabled)
            {
                _actualState.UpdateState();
            }
        }

        public void ChangeState(string name)
        {
            if (!_states.ContainsKey(name) || !Enabled) return;
        
            _actualState?.ExitState();

            _actualState = _states[name];
            _actualState.EnterState();
        }
    }
}
