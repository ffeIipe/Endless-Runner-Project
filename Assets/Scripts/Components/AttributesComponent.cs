using System;

namespace Components
{
    public class AttributesComponent
    {
        public event Action OnDead = delegate { };
        
        private float _health;
        private float _maxHealth;
        /*private float _shield;*/
        
        public AttributesComponent(float health, float shield)
        {
            _health = health;
            /*_shield = shield;*/
        }

        public bool IsAlive()
        {
            return _health > 0;
        }

        public float GetHealthPercentage()
        {
            return _health / _maxHealth;
        }

        public void ReceiveDamage(float damage)
        {
            _health -= damage;

            if (_health <= 0)
                OnDead.Invoke();
        }
    }
}