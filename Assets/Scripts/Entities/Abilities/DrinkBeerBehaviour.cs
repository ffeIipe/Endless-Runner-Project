using Scriptables.Abilities;

namespace Entities.Abilities
{
    public class DrinkBeerBehaviour : Ability
    {
        private DrinkBeerData DrinkBeerData => (DrinkBeerData)abilityData;  
        private CountdownTimer _cooldownTimer;
        
        public override void Activate()
        {
            throw new System.NotImplementedException();
        }

        public override void Deactivate()
        {
            throw new System.NotImplementedException();
        }
    }
}