using GameEngine.Components;
using System;

namespace GameLibrary.Components
{
    public class HealthComponent : ObjectComponent
    {
        public event Action OnDeath;
        public event Action<int> OnChanged;
        public event Action<int, int> OnDamaged;
        public event Action<int, int> OnHealed;

        private int _maxHealth;
        private int _health;

        public HealthComponent(int health)
        {
            _maxHealth = health;
            _health = health;
        }

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public void DealDamage(int damage)
        {
            _health -= damage;
            OnDamaged?.Invoke(_health, damage);

            if (_health <= 0)
            {
                OnDeath?.Invoke();
            }

            OnChanged?.Invoke(_health);
        }

        public void Heal(int heal)
        {
            _health += heal;
            OnHealed?.Invoke(_health, heal);

            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }

            OnChanged?.Invoke(_health);
        }
    }
}