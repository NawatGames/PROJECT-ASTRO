using UnityEngine;
using UnityEngine.Serialization;

namespace Player.StateMachine
{
    public class PlayerStateMachine : MonoBehaviour
    {
        [Header("DEBUG (não preencher)")]
        [SerializeField] private PlayerState _currentState;
        [Space(8)]
        
        public bool isAstro = true;
        
        public FreeMovePlayerState freeMoveState;
        public TaskPlayerState taskState;
        public GameOverPlayerState gameOverState;
        public DecontaminatePlayerState decontaminateState;
        public GoToTaskPlayerState goToTaskState;
        public GoToDecontaminationPlayerState goToDecontaminationPlayerState;
        
        private PlayerState _initialPlayerState;

        public InteractionHint interactionHint;
        
        // TODO Parar de usar OLD_VARS
        #region OLD_VARS
        public GameEvent startedDecontaminationEvent;
        public GameEvent stoppedDecontaminationEvent;
        public bool GameIsOver { get; private set; }
        #endregion

        private void Awake()
        {
            /*walkState = GetComponentInChildren<WalkPlayerState>();
            taskState = GetComponentInChildren<TaskPlayerState>();
            gameOverState = GetComponentInChildren<GameOverPlayerState>();
            decontaminateState = GetComponentInChildren<DecontaminatePlayerState>();
            goToTaskState = GetComponentInChildren<GoToTaskPlayerState>();
            
            if(!(walkState&& taskState && gameOverState && decontaminateState && goToTaskState))
            {
                Debug.LogWarning("Falta um/mais estados no Player");
            }*/
            
            _initialPlayerState = freeMoveState;
        }

        private void Start()
        {
            _currentState = _initialPlayerState;
            _currentState.EnterState();
        }

        public void SetState(PlayerState nextState)
        {
            _currentState.LeaveState();
            _currentState = nextState;
            _currentState.EnterState();
        }

        private void Update()
        {
            _currentState.StateUpdate();
        }
    }
}