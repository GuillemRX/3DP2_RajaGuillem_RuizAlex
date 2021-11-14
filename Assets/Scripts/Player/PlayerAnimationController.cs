using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [SerializeField] [ReadOnly] private AnimationState _currentState;

        private GroundDetector _groundDetector;

        private readonly AnimationHashes _animations = new AnimationHashes
        {
            Idle = Animator.StringToHash("idle"),
            WalkForward = Animator.StringToHash("walk forward"),
            WalkBackward = Animator.StringToHash("walk backward"),
            WalkRight = Animator.StringToHash("walk right"),
            WalkLeft = Animator.StringToHash("walk left"),
            WalkForwardRight = Animator.StringToHash("walk forward right"),
            WalkForwardLeft = Animator.StringToHash("walk forward left"),
            WalkBackwardRight = Animator.StringToHash("walk backward right"),
            WalkBackwardLeft = Animator.StringToHash("walk backward left"),
            Jump = Animator.StringToHash("jump loop")
        };

        private float _crossFadeTime = 0.25f;
        private Vector2 _playerDirection;

        private StateMachine<AnimationState> _stateMachine;

        private void Awake()
        {
            _groundDetector = GetComponentInChildren<GroundDetector>();
            _playerDirection = Vector2.zero;
            _stateMachine = InitStateMachine();
            _stateMachine.CurrentState = AnimationState.Idle;
        }

        private void Update()
        {
            _crossFadeTime = _stateMachine.CurrentState == AnimationState.Jump ? 0.05f : 0.2f;
            _stateMachine.CurrentState = GetNextState();
            _currentState = _stateMachine.CurrentState;
            _stateMachine.Update();
        }

        private void OnEnable()
        {
            EventManager.StartListening(Events.Instance.inputActions.onMove, OnMove);
        }

        private void OnDisable()
        {
            EventManager.StopListening(Events.Instance.inputActions.onMove, OnMove);
        }

        private void OnMove(Dictionary<string, object> message)
        {
            _playerDirection = (Vector2) message["value"];
        }

        private AnimationState GetNextState()
        {
            const float threshold = 0.1f;
            var velocity = _playerDirection;

            return _groundDetector.IsGrounded switch
            {
                false => AnimationState.Jump,
                _ => (velocity.y > threshold) switch
                {
                    true when velocity.x > threshold => AnimationState.WalkForwardRight,
                    true when velocity.x < -threshold => AnimationState.WalkForwardLeft,
                    true => AnimationState.WalkForward,
                    _ => (velocity.y < -threshold) switch
                    {
                        true when velocity.x > threshold => AnimationState.WalkBackwardRight,
                        true when velocity.x < -threshold => AnimationState.WalkBackwardLeft,
                        true => AnimationState.WalkBackward,
                        _ => (velocity.x > threshold) switch
                        {
                            false when velocity.x < -threshold => AnimationState.WalkLeft,
                            true => AnimationState.WalkRight,
                            _ => AnimationState.Idle
                        }
                    }
                }
            };
        }

        private StateMachine<AnimationState> InitStateMachine()
        {
            var sm = new StateMachine<AnimationState>();

            sm.OnStatePhase(AnimationState.Idle, StatePhase.Enter, () => PlayAnimation(_animations.Idle));
            sm.OnStatePhase(AnimationState.WalkForward, StatePhase.Enter, () => PlayAnimation(_animations.WalkForward));
            sm.OnStatePhase(AnimationState.WalkBackward, StatePhase.Enter,
                () => PlayAnimation(_animations.WalkBackward));
            sm.OnStatePhase(AnimationState.WalkRight, StatePhase.Enter, () => PlayAnimation(_animations.WalkRight));
            sm.OnStatePhase(AnimationState.WalkLeft, StatePhase.Enter, () => PlayAnimation(_animations.WalkLeft));
            sm.OnStatePhase(AnimationState.WalkForwardRight, StatePhase.Enter,
                () => PlayAnimation(_animations.WalkForwardRight));
            sm.OnStatePhase(AnimationState.WalkForwardLeft, StatePhase.Enter,
                () => PlayAnimation(_animations.WalkForwardLeft));
            sm.OnStatePhase(AnimationState.WalkBackwardRight, StatePhase.Enter,
                () => PlayAnimation(_animations.WalkBackwardRight));
            sm.OnStatePhase(AnimationState.WalkBackwardLeft, StatePhase.Enter,
                () => PlayAnimation(_animations.WalkBackwardLeft));
            sm.OnStatePhase(AnimationState.Jump, StatePhase.Enter, () => PlayAnimation(_animations.Jump));

            return sm;
        }

        private void PlayAnimation(int animationHash)
        {
            animator.CrossFade(animationHash, _crossFadeTime);
        }

        private enum AnimationState
        {
            Idle,
            WalkForward,
            WalkForwardRight,
            WalkForwardLeft,
            WalkRight,
            WalkLeft,
            WalkBackward,
            WalkBackwardRight,
            WalkBackwardLeft,
            Jump
        }

        private struct AnimationHashes
        {
            public int Idle { get; set; }
            public int WalkForward { get; set; }
            public int WalkBackward { get; set; }
            public int WalkRight { get; set; }
            public int WalkLeft { get; set; }
            public int WalkForwardRight { get; set; }
            public int WalkForwardLeft { get; set; }
            public int WalkBackwardRight { get; set; }
            public int WalkBackwardLeft { get; set; }
            public int Jump { get; set; }
        }
    }
}