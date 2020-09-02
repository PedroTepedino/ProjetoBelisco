// using GameScripts.Enemies;
// using UnityEngine;

// namespace RefatoramentoDoTioTepe
// {
//     public class AlertState : IState
//     {

//         private GameObject ownerGameObject;
//         private Transform target;
//         private Controller controllerOwner;
//         private float timer;
//         private float alertAnimationTime;

//         public AlertState(GameObject owner)
//         {
//             ownerGameObject = owner;
//             controllerOwner = owner.GetComponent<Controller>();       
//         }

//         public void OnEnter()
//         {
//             controllerOwner.actualState = "alert";
//             target = controllerOwner.targeting.target;
//             alertAnimationTime = controllerOwner.alertAnimationTime;
//         }

//         public void OnExit()
//         {
       
//         }

//         public void Tick()
//         {
//             target = controllerOwner.targeting.target;
//             timer += Time.deltaTime;
        
//             if(target != null)
//             {
//                 if (timer >= alertAnimationTime)
//                 {
//                     if (controllerOwner.actualState != "chase")
//                     {
//                         Debug.Log("chase");
//                         controllerOwner.stateMachine.ChangeState(new ChaseState(ownerGameObject));
                        
//                     }
//                 }
//             }
//             else
//             {
//                 if (controllerOwner.actualState != "move")
//                 {
//                     Debug.Log("move lose target");
//                     controllerOwner.stateMachine.ChangeState(new MoveState(ownerGameObject));
//                 }
//             }
//         }
//     }
// }
