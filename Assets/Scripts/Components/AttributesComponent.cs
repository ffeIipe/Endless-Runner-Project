using System;
using Entities.PowerUps;

namespace Components
{
    public class AttributesComponent
    {
        public event Action OnDead = delegate { };
        public event Action<float> OnReceiveDamage = delegate { };
        public event Action OnShieldDamage = delegate { };
        public event Action OnShieldDestroyed = delegate { };
        
        private float _health;
        private float _maxHealth;
        private float _shield;
        
        public AttributesComponent(float health, float shield)
        {
            _health = health;
            _shield = shield;
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
            if (_shield > 0)
            {
                _shield -= damage;
                OnShieldDamage?.Invoke();
                
                if (_shield <= 0)
                    OnShieldDestroyed?.Invoke();
            }
            else
            {
                _health -= damage;
                OnReceiveDamage?.Invoke(_health);
                
                if (_health <= 0)
                    OnDead?.Invoke();
            }
        }

        public void IncreaseHealth(float health)
        {
            _health += health;
            if (_health > _maxHealth)
                _health = _maxHealth;
        }

        public bool IsShielded()
        {
            return _shield > 0;
        }

        public void IncreaseShield(float shield)
        {
            _shield += shield;
        }
    }
}