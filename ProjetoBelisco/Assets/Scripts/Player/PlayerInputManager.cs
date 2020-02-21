using Rewired;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

/* Enum: Inputs
 * 
 * Describes the possible inputs the player can make.
 * 
 * Null - No input registered
 * Pause - Pause Button pressed
 * Move - Register the side movement
 * Jump - Jump button pressed or holded
 */
[Flags]
public enum Inputs
{
    Null = 0,
    Pause = 1 << 0,
    Move = 1 << 1,
    Jump = 1 << 2
}

/* Class: PlayerInputManager
 *  Manages all the inputs of the player
 *  
 * About: Handling the player inputs
 *  Handle multiple inputs at the same time, an if the controller or keyboard can be used during gameplay.
 *  
 * About: Unique combinations
 *  Every cobination of cammands have a single and unique binary combination, due to the enum <Inputs>.
 *  
 * See Also:
 *  <Inputs>
 */
[RequireComponent(typeof(PlayerMovement))]
public class PlayerInputManager : MonoBehaviour
{
    // Group: Private Variables

    /* Variables: Controller Variables 
     * 
     * _playerControllerIndex - Stores the index of the player controller used to identify a player in the Rewired plugin.
     *        Value set to 0, since there is only 1 player.
     *        
     * _playerController - Stores the Player object witch is used to retrieve the controller inputs from the joystick or keyboard.
     * 
     * _controllerDeadZone - Analog inputs bellow this number are ignored.
     */
    [BoxGroup("Player Info")] [ShowInInspector] private readonly int _playerControllerIndex = 0;
    [BoxGroup("Player Info")] [ShowInInspector] private Player _playerController;
    [SerializeField] [BoxGroup("Controller Parameters")] private float _controllerDeadZone = 0.25f;

    /* Floats: Jump Parameters
     * 
     * _maxTime - The maximum time that the player can hold the jump button.
     *  After that time, the player will start to fall.
     */
    [SerializeField] [BoxGroup("Jump Parameters")] private float _maxTime = 1f;
    
    /* Variables: Input Handler Variables
     * 
     * _curentInputs - The inputs of the player during a given frame.
     */
    private Inputs _curentInputs = Inputs.Null;

    /* Variables: External Classes
     * About::
     * Classes that handle the opperations given by the player *Inputs*
     * 
     * Vars::
     * _playerMovement - Controlls the movement of the player (<PlayerMovement>)
     *  
     * _playerJump - Controlls the jumping action of the player (<PlayerJump>)
     * 
     * _playerGrounder - Checks if the player is touching the ground (<PlayerGrounder>)
     */
    private PlayerMovement _playerMovement = null;
    private PlayerJump _playerJump = null;
    private PlayerGrounder _playerGrounder = null;

    /* Variables: Controller Lock Parameters    
     * About::
     * Parameters that are used to determin if the controller can be used or not at a given frame.
     * 
     * Bools::
     * _dashing - *True* if the player is _Dashing_.
     * _isGrounded - *True* if the player is touching the ground.
     */
    #region Lock Controller Parameters
    private bool _dashing = false;
    private bool _isGrounded = true;
    #endregion

    // Group: Public Variables
    // Variable: OnPause
    // Action that gives a signal when the player pressed the pause button
    public static Action OnPause;

    // Group: Properties
    /* Properties: Helper Properties
     * 
     * About::
     *  Properties that help the player to acess certein informations faster.
     * 
     * Vars::
     * MoveDirection - Value between *-1* & *1* that determins if the player is going *Left* or *Right*.
     * PlayerId - Returns the index of the player controller.
     */
    public float MoveDirection { get; private set; } = 0f;
    public int PlayerId { get => _playerControllerIndex; }

    // Group: Unity Methods
    /* Function: Awake
     *  Handles the initialization of the class, by caling the <Setup Methods>
     */
    private void Awake()
    {
        AssociatePlayer();
        GetEssentialComponentsComponent();
        SubscribeFunctions();
    }

    /* Function: OnDestroy
     * Handles the class right before the destruction of the GameObject, by calling the <Destruction Methods>
     */
    private void OnDestroy()
    {
        UnsubscribeFunctins();
    }

    /* Function: Update
     * Handles the functions that need to be called every frame.
     * 
     * + Get the inputs. (<GetInputs>)
     * + Decide what to do. (<DecisionMaking>)
     */
    void Update()
    {
        GetInputs();
        DecisionMaking();
    }

    // Group: Setup Methods
    /* Function: SubscribeFunctions
     * Subscribe the <Listeners> Methods to their respective signals
     */
    private void SubscribeFunctions()
    {
        _playerGrounder.OnGrounded += ListenGrounder;
    }

    /* Function: AssociatePlayer
     * Associates the <Player Controller: _playerController> the the right input method, by default is the joystick.
     */
    private void AssociatePlayer()
    {
        _playerController = ReInput.players.GetPlayer(_playerControllerIndex);
    }

    /* Function: GetEssentialComponentsComponent
     * About::
     * Get the necesserie componentes from the gameObject. (Components found at <External Classes>)
     */
    private void GetEssentialComponentsComponent()
    {
        _playerMovement = this.GetComponent<PlayerMovement>();
        _playerGrounder = this.GetComponent<PlayerGrounder>();
        _playerJump = this.GetComponent<PlayerJump>();
    }

    // Group: Destruction Methods
    /* Function: UnsubscribeFunctins
     * Remove the subscription of the <Listeners>, subscribed at <SubscribeFunctions>.
     */
    private void UnsubscribeFunctins()
    {
        _playerGrounder.OnGrounded -= ListenGrounder;
    }

    // Group: Input Handling
    // Functions that get the inputs from the controller or keyboard

    /* Function: GetInputs
     * + Resetes the <curent inputs: _curentInputs>.
     * + Gather all the <Inputs> att the given frame.
     */
    private void GetInputs()
    {
        _curentInputs = Inputs.Null;

        GetMovement();
        GetPause();
    }

    /* Function: GetMovement
     * Gets the movement action from the curent input method.
     */
    private void GetMovement()
    {
        MoveDirection = _playerController.GetAxisRaw("MoveHorizontal");

        if (Mathf.Abs(MoveDirection) >= _controllerDeadZone)
        {
            _curentInputs |= Inputs.Move;
        }
    }

    /* Function: GetPause
     * Gets the Pause button action from the current input method.
     */
    private void GetPause()
    {
        if (_playerController.GetButtonDown("Pause"))
        {
            _curentInputs |= Inputs.Pause;
        }
    }

    /* Function: GetJump
     * Gets the jump action from the curent input method.
     */
    private void GetJump()
    {

    }

    // Group: Decision Making
    // Decides witch comands can or cannot be executed at a given frame.

    /* Function: DecisionMaking
     *  Runs a series of statements to decide witch actions to call at a given frame.
     */
    private void DecisionMaking()
    {
        if ((_curentInputs & Inputs.Pause) == Inputs.Pause)
        {
            this.Pause();
        }
        else
        {
            if (!IsControllerLocked())
            {
                if ((_curentInputs & Inputs.Jump) == Inputs.Jump)
                {

                }

                if ((_curentInputs & Inputs.Move) == Inputs.Move)
                {
                    this.Move();
                }
            }
        }
    }

    // Group: Calling Functions
    // Functions that redirect the commads to their specific class.
    // About:: 
    //  These are the functions called at <Decision Making>

    /* Function: Pause
     * Calls the <event : OnPause> to activate the Puase Scene.
     */
    private void Pause()
    {
        OnPause?.Invoke();
    }

    /* Function: Move
     * Calls the movement Function from <PlayerMovement>.
     */
    private void Move()
    {
        _playerMovement.MovePlayer(MoveDirection);
    }

    // Group: Controller LockDown
    // Decides if the controller can be used or not at a given frame.

    /* Function: IsControllerLocked
     * Determins if the controller can be used or not at a given moment.
     */
    private bool IsControllerLocked()
    {
        if (_dashing)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Group: Listeners
    // Methods that listen to Signals 

    /* Function: ListenDash
     * Function that Listen to the Action that detirmins if the player is dashing or not.
     * 
     * Parameters: 
     * isDashing - Boolean that recieves the info if the player is dashing or not.
     */
    private void ListenDash(bool isDashing)
    {
        this._dashing = isDashing;
    }

    /* Function: ListenGrounder
     * Function that Listen to the Action that detirmins if the player is grounded or not.
     * 
     * Parameters: 
     * isGrounded - Boolean that recieves the info if the player is grounded or not.
     */
    private void ListenGrounder(bool isGrounded)
    {
        this._isGrounded = isGrounded;
    }
}
