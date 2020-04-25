using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Interface: IState
 * Interface that describes the essential functions for a state used with the <StateMachine>.
 */
public interface IState
{
/*Function: EnterState
 * Commands executed when enter the state.
 */
    void EnterState();

 /*Function: RunState
  * Commands executed at update time.
 */
    void RunState();

/*Function: ExitState
 * Commands executed before exit the state.
 */
    void ExitState();
}
