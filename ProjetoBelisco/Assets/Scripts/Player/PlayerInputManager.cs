using Rewired;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

/* Enum: Inputs
 * 
 * Describes the possible inputs the player can make.
 * 
 * Null - No input registered
 * Pause - Pause Button pressed
 * Move - Register the side movement
 * JumpStart - Jump button pressed
 * JumpFollowUp - Jump button holded
 * JumpRelease - Jump button has been released
 */
[Flags]
public enum Inputs
{
    Null            = 0,
    Pause           = 1,
    Move            = 1 << 1,
    JumpStart       = 1 << 2,
    JumpFollowUp    = 1 << 3,
    JumpRelease     = 1 << 4,
    Attack          = 1 << 5
}

public enum Directions
{
    Null  = 0,
    Up    = 1,
    Down  = 1 << 1,
    Right = 1 << 2,
    Left  = 1 << 3
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
public class PlayerInputManager : SerializedMonoBehaviour
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
    [BoxGroup("Player Info")] [ShowInInspector] [ReadOnly] private readonly int _playerControllerIndex = 0;
    private Player _playerController;
    [SerializeField] [BoxGroup("Controller Parameters")] private float _controllerDeadZone = 0.25f;

    /* Floats: Movement Parameters
     * _movementAxisTimeCounter - Counts the time the movement axis have been active or inactive.
     */
    private float _movementAxisTimeCounter = 0f;

    /* Floats: Jump Parameters
     * 
     * _jumpActionTime - The timestamp in witch the player held the jump button down.
     * _jumpActionStorageTime - Time in seconds that the jump action is stored so that it is executed later.
     * _jumpActionTimer - Timer to delete the action.
     * _jumpCicle - Indicates if the player is going throgh a jump cicle.
     */
    private float _jumpActionTimestamp = 0f;
    [SerializeField] [BoxGroup("Controller Parameters")] private float _jumpActionStorageTime = 0.2f;
    private float _jumpActionTimer = 0f;
    private bool _jumpCicle = false;
    
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
    private PlayerAttackSystem _playerAttack = null;

    /* Variable: _directionsXYDistances
     *    Determins the min and max angles a direction can have <Directions>.
     */
    [SerializeField]
    private Dictionary<Directions, Vector2> _directionsAngles;

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
    public Vector2 MoveDirection { get; private set; } = Vector2.zero;
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
     * + Decide what to do. (<Decision Making>)
     */
    private void Update()
    {
        GetInputs();
        DecisionMakingUpdate();
    }

    /* Function: FixedUpdate
     * Handles the functions that need to be called every frame.
     */
    private void FixedUpdate()
    {
        DecisionMakingFixedUpdate();
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
        _playerAttack = this.GetComponent<PlayerAttackSystem>();
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
        GetJump();
        GetAttack();
        InputTimerHandler();
        InputStorageHandler();
    }

    /* Function: GetMovement
     * Gets the movement action from the curent input method.
     */
    private void GetMovement()
    {
        MoveDirection = _playerController.GetAxis2DRaw("MoveHorizontal", "MoveVertical");

        if (Mathf.Abs(MoveDirection.x) >= _controllerDeadZone)
        {
            _curentInputs |= Inputs.Move;
            _movementAxisTimeCounter = _playerController.GetAxisTimeActive("MoveHorizontal");
        }
        else
        {
            _movementAxisTimeCounter = _playerController.GetAxisTimeInactive("MoveHorizontal");
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
        if (_playerController.GetButtonDown("Jump"))
        {
            _curentInputs |= Inputs.JumpStart;
            _jumpActionTimestamp = 0f;
            _jumpActionTimer = _jumpActionStorageTime;
            if (_playerJump.CanJump())
            {
                _jumpCicle = true;
            }
        }
        else if (_playerController.GetButton("Jump"))
        {
            _curentInputs |= Inputs.JumpFollowUp;
            _jumpActionTimestamp = _playerController.GetButtonTimePressed("Jump");
        }
        else if (_playerController.GetButtonUp("Jump"))
        {
            _curentInputs |= Inputs.JumpRelease;
            _jumpCicle = false;
        }
    }

    private void GetAttack()
    {
        if (_playerController.GetButtonDown("Attack"))
        {
            _curentInputs |= Inputs.Attack;
        }
    }

    /* Function: InputTimerHandler
     * Handles the countdown of the input Storage timers
     * About:: 
     * Called Every Frame
     */
    private void InputTimerHandler()
    {
        _jumpActionTimer -= Time.deltaTime;
    }

    /* Function: InputStorageHandler
     * Handles the Storage of the inputs
     */
    private void InputStorageHandler()
    {
        if (_jumpActionTimer >= 0)
        {
            _curentInputs |= Inputs.JumpStart;
        }
    }

    // Group: Decision Making
    // Decides witch commands can or cannot be executed at a given frame.

    /* Function: DecisionMakingUpdate
     *  Runs a series of statements to decide witch actions to call at a given frame.
     */
    private void DecisionMakingUpdate()
    {
        if ((_curentInputs & Inputs.Attack) == Inputs.Attack)
        {
            this.Attack();
        }

        if ((_curentInputs & Inputs.Pause) == Inputs.Pause)
        {
            this.Pause();
        }
    }

    /* Function: DecisionMakingFixedUpdate
     * Decides witch Action the player will make, in every stamp of the fixed update.
     */
    private void DecisionMakingFixedUpdate()
    {
        if (IsControllerLocked()) return;
        
        if (((_curentInputs & Inputs.JumpStart) == Inputs.JumpStart))
        {
            if (_playerJump.CanJump())
            {
                this.Jump();
            }
        }

        if (((_curentInputs & Inputs.JumpFollowUp) == Inputs.JumpFollowUp))
        {
            if (_jumpCicle)
            {
                if (_playerJump.CanRaiseJump(_jumpActionTimestamp))
                {
                    this.Jump();
                }
            }
        }

        if ((_curentInputs & Inputs.Move) == Inputs.Move)
        {
            this.Move();
        }
        else
        {
            this.StopMovement();
        }
    }

    // Group: Calling Functions
    // Functions that redirect the commands to their specific class.
    // About:: 
    //  These are the functions called at <Decision Making>

    /* Function: Pause
     * Calls the <event : OnPause> to activate the Pause Scene.
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
        _playerMovement.MovePlayer(MoveDirection.x, _movementAxisTimeCounter);
    }

    /* Function: StopMovement
     * Calls the stop movement Function from <PlayerMovement>.
     */
    private void StopMovement()
    {
        _playerMovement.StopMovement(_movementAxisTimeCounter);
    }

    /* Function: Jump
     * Calls the Jump Function from <PlayerJump>
     */
    private void Jump()
    {
        _playerJump.Jump();
    }

    private void Attack()
    {
        _playerAttack.Attack(CalculateDirections(MoveDirection));
    }

    // Group: Controller LockDown
    // Decides if the controller can be used or not at a given frame.

    /* Function: IsControllerLocked
     * Determines if the controller can be used or not at a given moment.
     */
    private bool IsControllerLocked()
    {
        return _dashing;
    }

    // Group: Listeners
    // Methods that listen to Signals 

    /* Function: ListenDash
     * Function that Listen to the Action that determines if the player is dashing or not.
     * 
     * Parameters: 
     * isDashing - Boolean that receives the info if the player is dashing or not.
     */
    private void ListenDash(bool isDashing)
    {
        this._dashing = isDashing;
    }

    /* Function: ListenGrounder
     * Function that Listen to the Action that determines if the player is grounded or not.
     * 
     * Parameters: 
     * isGrounded - Boolean that receives the info if the player is grounded or not.
     */
    private void ListenGrounder(bool isGrounded)
    {
        this._isGrounded = isGrounded;
    }
    
    private Directions CalculateDirections(Vector2 input)
    {
        if (input.magnitude <= _controllerDeadZone)
        {
            return Directions.Null;
        }

        Directions currentDirection = Directions.Null;
        float angle = 720;

        foreach (var pair in _directionsAngles)
        {
            var aux = Mathf.Abs(Vector2.SignedAngle(pair.Value.normalized, input.normalized));

            if (!(aux <= angle)) continue;
            
            angle = aux;
            currentDirection = pair.Key;
        }

        return currentDirection;
    }
}
