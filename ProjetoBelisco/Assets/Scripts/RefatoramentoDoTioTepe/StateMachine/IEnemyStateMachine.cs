using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public interface IEnemyStateMachine 
    {    
        void Interfacinha();

        StateMachine stateMachine{get;set;}

        bool alive {get;set;}
    }
}
