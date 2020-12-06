namespace Belisco
{
    public class LifeSystem
    {
        private readonly Player _player;
        
        public readonly int MaxHealth;
        public readonly float InvincibilityTime;
        
        public bool Invincible { get; private set; }

        public LifeSystem(Player player)
        {
            _player = player;
            MaxHealth = player.PlayerParameters.MaxHealth;
            CurrentLife = MaxHealth;
            InvincibilityTime = player.PlayerParameters.InvincibilityTime;
        }

        public bool StillAlive => CurrentLife > 0;

        public int CurrentLife { get; private set; }

        public void Damage(int damage)
        {
            if (!Invincible)
            {
                CurrentLife -= damage;

                _player.DamageDealt(CurrentLife, MaxHealth);

                if (!StillAlive) Die();
            }
        }

        private void Die()
        {
            _player.Died();
            _player.gameObject.SetActive(false);
        }

        public void SetInvincible(bool setInvincible)
        {
            Invincible = setInvincible;
        }
    }
}