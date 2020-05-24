using GameScripts.Enemies;
using UnityEngine;

namespace GameScripts.StateMachine.States
{
    public class IddleState : IState
    {
        private GameObject ownerGameObject;
        private Controller controllerOwner;

        public IddleState(GameObject owner)
        {
            ownerGameObject = owner;
            controllerOwner = owner.GetComponent<Controller>();  
        }

        public void EnterState()
        {
            controllerOwner.actualState = "iddle";
        }

        public void ExitState()
        {
        
        }

        public void RunState()
        {
        
        }

    }
}
