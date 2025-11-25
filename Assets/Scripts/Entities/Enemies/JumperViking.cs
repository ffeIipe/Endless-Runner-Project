using FiniteStateMachine.States;

namespace Entities.Enemies
{
    public class JumperViking : Viking
    {
        protected override AttackState CreateAttackState()
        {
            return new JumpAttackState(GetStateMachine());
        }
    }
}