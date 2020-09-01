using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public interface IPlayerInput
    {
        float Horizontal { get; }
        bool PausePressed { get; }
        bool Jump { get; }
        bool Attack { get; }
        bool StrongAttack { get; }
        bool RangedAttack { get; }
    }
}