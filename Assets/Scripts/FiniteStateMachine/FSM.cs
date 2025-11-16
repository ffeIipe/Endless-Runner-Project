using System.Collections.Generic;

namespace FiniteStateMachine
{
    public class FSM
    {
        public bool Enabled = true;
    
        private Dictionary<string, BaseState> _states;
        private BaseState _actualState;

        public FSM()
        {
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
