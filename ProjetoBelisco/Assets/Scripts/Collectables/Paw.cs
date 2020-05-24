using GameScripts.Player;
using GameScripts.PoolingSystem;
using UnityEngine;

namespace GameScripts.Collectables
{
    public class Paw : BaseObject, IPooledObject
    {
        [SerializeField] private int _pawsToAdd = 1;

        public override void PickUp()
        {
            OnPickUp?.Invoke();
            this.gameObject.SetActive(false);
        }

        public override void Effect()
        {
            PawStorage.AddPaws(_pawsToAdd);
        }

        public void OnObjectSpawn()
        {
            this.gameObject.SetActive(true);
        }
    }
}
