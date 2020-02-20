using Rewired;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Flags]
public enum Inputs
{
    Null = 0,
    Pause = 1 << 0,
    Move = 1 << 1,
    Jump = 1 << 2
}

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInputManeger : MonoBehaviour
{
    [BoxGroup("Player Info")] [ShowInInspector] private readonly int _playerControllerIndex = 0;
    [BoxGroup("Player Info")] [ShowInInspector] private Player _playerController;
    public int PlayerId { get => _playerControllerIndex; }

    [SerializeField] [BoxGroup("Controller Parameters")] private float _controllerDeadZone = 0.25f;

    [SerializeField] [BoxGroup("Jump Parameters")] private float _jumpVelocity = 10f;
    [SerializeField] [BoxGroup("Jump Parameters")] private float _maxTime = 1f;
    [SerializeField] [BoxGroup("Jump Parameters")] private float _gravityMultiplier = 3f;

    public static Action OnPause;

    private Inputs _curentInputs = Inputs.Null;
    private PlayerMovement _playermovement = null;
    private PlayerJump _playerJump = null;
    private PlayerGrounder _playerGrounder = null;

    public float MoveDirection { get; private set; } = 0f;

    #region Lock Controller Parameters
    private bool _dashing = false;
    private bool _isGrounded = true;
    #endregion

    private void Awake()
    {
        AssociatePlayer();
        GetEssentialComponentsComponent();
        SubscribeFunctions();
    }

    private void OnDestroy()
    {
        UnsubscribeFunctins();
    }

    void Update()
    {
        GetInputs();
        DecisionMaking();
    }

    /// Setup
    private void SubscribeFunctions()
    {
        _playerGrounder.OnGrounded += ListenGrounder;
    }

    private void UnsubscribeFunctins()
    {
        _playerGrounder.OnGrounded -= ListenGrounder;
    }

    /// Controller Sethings
    private void AssociatePlayer()
    {
        _playerController = ReInput.players.GetPlayer(_playerControllerIndex);
    }

    private void GetEssentialComponentsComponent()
    {
        _playermovement = this.GetComponent<PlayerMovement>();
        _playerGrounder = this.GetComponent<PlayerGrounder>();
        _playerJump = this.GetComponent<PlayerJump>();
    }

    /// Inputs
    private void GetInputs()
    {
        _curentInputs = Inputs.Null;

        GetMovement();
        GetPause();
    }

    private void GetMovement()
    {
        MoveDirection = _playerController.GetAxisRaw("MoveHorizontal");

        if (Mathf.Abs(MoveDirection) >= _controllerDeadZone)
        {
            _curentInputs |= Inputs.Move;
        }
    }

    private void GetPause()
    {
        if (_playerController.GetButtonDown("Pause"))
        {
            _curentInputs |= Inputs.Pause;
        }
    }

    private void GetJump()
    {

    }

    /// Decision Making
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

    /// Calling Functions
    private void Pause()
    {
        OnPause?.Invoke();
    }

    private void Move()
    {
        _playermovement.MovePlayer(MoveDirection);
    }

    /// Controller LockDown

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

    /// Listeners
    private void ListenDash(bool isDashing)
    {
        this._dashing = isDashing;
    }

    private void ListenGrounder(bool isGrounded)
    {
        this._isGrounded = isGrounded;
    }
}
