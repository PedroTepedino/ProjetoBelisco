using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public abstract class AbstractLockable : MonoBehaviour
    {
        public abstract void Lock();
        public abstract void Unlock();
    }
}
