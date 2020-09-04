using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public interface IEnemyStateMachine 
    {    

        StateMachine stateMachine{get;}
        EnemyParameters EnemyParameters {get;}
        bool movingRight {get;}
        bool alive {get;set;}
        
        void Interfacinha();
    }
}
