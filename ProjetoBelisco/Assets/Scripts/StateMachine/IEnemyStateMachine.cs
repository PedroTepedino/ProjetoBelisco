namespace Belisco
{
    public interface IEnemyStateMachine
    {
        StateMachine stateMachine { get; }
        EnemyParameters EnemyParameters { get; }
        bool movingRight { get; set; }
        bool alive { get; set; }

        void Interfacinha();
    }
}