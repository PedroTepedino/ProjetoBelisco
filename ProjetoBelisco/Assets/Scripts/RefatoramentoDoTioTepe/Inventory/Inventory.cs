using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    [RequireComponent(typeof(Collider2D))]
    public class PawnInventory : MonoBehaviour
    {
        private int _pawnBalance;

        private void Awake()
        {
            _pawnBalance = 0;
        }

        public void PickUp(Pawn pawn)
        {
            _pawnBalance += pawn.Value;
            pawn.gameObject.SetActive(false);
        }
    }
}