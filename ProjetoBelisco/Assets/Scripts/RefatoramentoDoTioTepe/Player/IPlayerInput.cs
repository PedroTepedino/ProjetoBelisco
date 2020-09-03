using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public interface IPlayerInput
    {
        float Horizontal { get; }
        float Vertical { get; }
        bool PausePressed { get; }
        bool Jump { get; }
        bool Attack { get; }
        bool StrongAttack { get; }
        bool RangedAttack { get; }
    }
}