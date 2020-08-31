using UnityEngine;

namespace RefatoramentoDoTioTepe
{
    public class RewiredPlayerInput : MonoBehaviour, IPlayerInput
    {
        private Rewired.Player _rewiredPlayer;

        public static IPlayerInput Instance { get; set; } 
        
        public float Horizontal => _rewiredPlayer.GetAxis("MoveHorizontal");

        public bool PausePressed => _rewiredPlayer.GetButtonDown("Pause");
        public bool Jump => _rewiredPlayer.GetButton("Jump");

        public bool Attack => _rewiredPlayer.GetButtonDown("BasicAttack");
        public bool StrongAttack => _rewiredPlayer.GetButtonDown("StrongAttack");
        public bool RangedAttack => _rewiredPlayer.GetButtonDown("RangedAttack");

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);   
            }
            
            _rewiredPlayer = Rewired.ReInput.players.GetPlayer(0);

            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (ReferenceEquals(Instance, this))
            {
                Instance = null;
            }
        }
    }
}