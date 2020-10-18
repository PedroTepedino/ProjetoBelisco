using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public interface ILockable
    {
        void Lock();
        void Unlock();
    }
}
