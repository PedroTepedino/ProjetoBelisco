using Sirenix.OdinInspector;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class PawInventory : MonoBehaviour
    {
        [ShowInInspector] private int _pawBalance;

        public int PawBalance => _pawBalance;

        private void Awake()
        {
            _pawBalance = 0;
        }

        public void PickUp(Paw paw)
        {
            _pawBalance += paw.Value;
            paw.gameObject.SetActive(false);
        }

        public void Initialize(int balance)
        {
            if (balance < 0)
            {
                _pawBalance = 0;
            }
            else
            {
                _pawBalance = balance;
            }
        }
    }
}