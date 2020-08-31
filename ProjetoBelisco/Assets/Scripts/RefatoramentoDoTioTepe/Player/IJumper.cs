using System;

namespace RefatoramentoDoTioTepe
{
    public interface IJumper
    {
        void Tick();
        bool Jumping { get; }
    }
}