using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public interface IPlayerInput
    {
        float Horizontal { get; }
        bool PausePressed { get; }
        bool Jump { get; }
    }
}