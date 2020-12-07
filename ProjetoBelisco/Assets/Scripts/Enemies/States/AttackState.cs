using UnityEngine;

namespace Belisco
{
    public class AttackState : IState
    {
        private readonly Attack attack;
        private readonly GameObject ownerGameObject;
        private Transform target;
        private readonly Targeting targeting;
        private float timer;

        public AttackState(GameObject owner)
        {
            ownerGameObject = owner;
            attack = ownerGameObject.GetComponent<Attack>();
            targeting = ownerGameObject.GetComponent<Targeting>();
        }

        public void OnEnter()
        {
            target = targeting.target;
            timer = attack.attackSpeed;
        }

        public void OnExit() { }

        public void Tick()
        {
            target = targeting.target;
            timer += Time.deltaTime;

            if (target != null)
                if (timer >= attack.attackSpeed)
                {
                    attack.SelectAttack();
                    timer = 0f;
                }
        }
    }
}