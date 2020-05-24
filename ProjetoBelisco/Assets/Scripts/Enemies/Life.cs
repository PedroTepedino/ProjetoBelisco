using GameScripts.LivingBeingSystems;

namespace GameScripts.Enemies
{
    public class Life : BaseLifeSystem
    {
        public System.Action<int, int> OnEnemyDamage;
        public System.Action OnEnemyDie;
        public System.Action<int, int> OnEnemyHeal;

        public override void Damage(int damagePoints = 1)
        {
            if (damagePoints <= 0) return;
        
            _curentHealthPoints -= damagePoints;
            OnEnemyDamage?.Invoke(_curentHealthPoints, _maximumHealth);

            if (IsDead)
            {
                Die();
            }
        }

        public override void RestoreHealth(int healPoints = 1)
        {
            if (healPoints <= 0) return;
        
            _curentHealthPoints += healPoints;
            OnEnemyHeal?.Invoke(_curentHealthPoints, _maximumHealth);

            if (IsHealthFull)
            {
                _curentHealthPoints = _maximumHealth;
            }
        }

        protected override void Die(){
            OnEnemyDie?.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}
