using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public interface IPlayerInput
    {
        float Horizontal { get; }
        float Vertical { get; }
        bool PausePressed { get; }
        bool Jump { get; }
        bool TerminateJump { get; }
        bool InitiateJump { get; }
        bool Attack { get; }
        bool StrongAttack { get; }
        bool RangedAttack { get; }
        bool Dash { get; }
    }
}