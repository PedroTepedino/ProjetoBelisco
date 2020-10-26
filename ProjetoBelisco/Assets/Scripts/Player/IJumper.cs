namespace Belisco
{
    public interface IJumper
    {
        bool Jumping { get; }
        void Tick();
    }
}