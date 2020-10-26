namespace Belisco
{
    public class LifeSystem
    {
        private readonly Player _player;

        public LifeSystem(Player player)
        {
            _player = player;
            MaxHealth = player.PlayerParameters.MaxHealth;
            CurrentLife = MaxHealth;
        }

        public bool StillAlive => CurrentLife > 0;
        public int MaxHealth { get; }

        public int CurrentLife { get; private set; }

        public void Damage(int damage)
        {
            CurrentLife -= damage;

            _player.DamageDealt(CurrentLife, MaxHealth);

            if (!StillAlive) Die();
        }

        private void Die()
        {
            _player.Died();
            _player.gameObject.SetActive(false);
        }
    }
}