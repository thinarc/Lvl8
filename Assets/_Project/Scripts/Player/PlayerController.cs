using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptablePlayer stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;

        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Flew;

        #endregion

        private float _time;

        private bool _sitdown = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();

            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
        }
        
        private Vector3? _targetPosition;

        private void GatherInput()
        {
            // Игнорируем клики по UI
            bool isClick = UnityEngine.Input.GetKeyDown(KeyCode.Mouse0);
            bool isOverUI = EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();

            if (isClick && !isOverUI)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                mouseWorldPos.z = transform.position.z;

                const float clickThreshold = 0.05f;

                if (!_targetPosition.HasValue ||
                    (mouseWorldPos - _targetPosition.Value).sqrMagnitude > clickThreshold * clickThreshold)
                {
                    _targetPosition = mouseWorldPos;
                }
            }

            Vector2 move = Vector2.zero;

            if (_targetPosition.HasValue)
            {
                Vector2 direction = _targetPosition.Value - transform.position;

                const float stopDistance = 0.05f;

                if (direction.sqrMagnitude <= stopDistance * stopDistance)
                {
                    _targetPosition = null;
                }
                else
                {
                    move = direction.normalized;
                }
            }

            _frameInput = new FrameInput
            {
                Move = move
            };

            if (_frameInput.Move != Vector2.zero)
                _sitdown = false;
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleFly();
            HandleDirection();
            HandleGravity();
            
            ApplyMovement();
        }

        #region Collisions
        
        private bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;
            
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, stats.grounderDistance, stats.groundLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, stats.grounderDistance, ~stats.playerLayer);
            
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
            
            switch (_grounded)
            {
                case false when groundHit:
                    _grounded = true;
                    GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
                    break;
                case true when !groundHit:
                    _grounded = false;
                    GroundedChanged?.Invoke(false, 0);
                    break;
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Flying

        private void HandleFly()
        {
            if (_frameInput.Move.y == 0)
            {
                var deceleration = !_grounded ? stats.groundDeceleration : stats.airDeceleration;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, _frameInput.Move.y * stats.flyPower, stats.acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? stats.groundDeceleration : stats.airDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * stats.maxSpeed, stats.acceleration * Time.fixedDeltaTime);
            }
        }

        #endregion

        #region Gravity

        private void HandleGravity()
        {
            // if (_grounded && _frameVelocity.y <= 0f)
            // {
            //     print("f");
            //     _frameVelocity.y = stats.groundingForce;
            // }
            // else
            // {
            //     var inAirGravity = stats.fallAcceleration;
            //     if (_frameVelocity.y > 0) inAirGravity *= stats.flyEndEarlyGravityModifier;
            //     _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -stats.maxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            // }
        }

        #endregion

        private void ApplyMovement()
        {
            _rb.linearVelocity = _frameVelocity;
            if (_sitdown)
                _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            else _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public struct FrameInput
    {
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Flew;
        public Vector2 FrameInput { get; }
    }
}