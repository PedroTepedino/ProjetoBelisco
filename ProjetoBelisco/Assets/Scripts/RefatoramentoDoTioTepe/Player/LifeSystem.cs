﻿namespace RefatoramentoDoTioTepe
{
    public class LifeSystem
    {
        private readonly int _maxLife;
        private int _currentLife;
        private readonly Player _player;

        public bool StillAlive => _currentLife > 0;
        public int MaxHealth => _maxLife;
        public int CurrentLife => _currentLife;

        public LifeSystem(Player player)
        {
            _player = player;
            _maxLife = player.PlayerParameters.MaxHealth;
            _currentLife = _maxLife;
        }

        public void Damage(int damage)
        {
            _currentLife -= damage;

            _player.DamageDealt(_currentLife, _maxLife);
            
            if (!StillAlive)
            {
                Die();
            }
        }

        private void Die()
        {
            _player.Died();
            _player.gameObject.SetActive(false);
        }
    }
}