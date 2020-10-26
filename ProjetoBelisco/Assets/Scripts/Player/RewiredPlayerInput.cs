using Rewired;
using UnityEngine;

namespace Belisco
{
    public class RewiredPlayerInput : MonoBehaviour, IPlayerInput
    {
        private Rewired.Player _rewiredPlayer;

        public static IPlayerInput Instance { get; set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            _rewiredPlayer = ReInput.players.GetPlayer(0);

            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (ReferenceEquals(Instance, this)) Instance = null;
        }

        public float Horizontal => _rewiredPlayer.GetAxis("MoveHorizontal");
        public float Vertical => _rewiredPlayer.GetAxis("MoveVertical");

        public bool PausePressed => _rewiredPlayer.GetButtonDown("Pause");
        public bool Jump => _rewiredPlayer.GetButton("Jump");
        public bool TerminateJump => _rewiredPlayer.GetButtonUp("Jump");
        public bool InitiateJump => _rewiredPlayer.GetButtonDown("Jump");

        public bool Attack => _rewiredPlayer.GetButtonDown("BasicAttack");
        public bool StrongAttack => _rewiredPlayer.GetButtonDown("StrongAttack");
        public bool RangedAttack => _rewiredPlayer.GetButtonDown("RangedAttack");
        public bool Dash => _rewiredPlayer.GetButtonDown("Dash");
        public bool MovingRight => Horizontal >= 0.1f;
        public bool MovingLeft => Horizontal <= -0.1f;

        public int HorizontalInt
        {
            get
            {
                var value = _rewiredPlayer.GetAxisRaw("MoveHorizontal");
                return value > 0.2f ? 1 : value < -0.2f ? -1 : 0;
            }
        }
    }
}