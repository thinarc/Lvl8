using System;
using UnityEngine;

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
        public bool FlyHeld => _frameInput.FlyHeld;

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

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                FlyHeld = UnityEngine.Input.GetButton("Jump"),
                Move = new Vector2(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"))
            };

            if (stats.snapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < stats.horizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < stats.verticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            _flyToConsume = _frameInput.FlyHeld;

            if (_frameInput.FlyHeld || _frameInput.Move.x != 0 || _frameInput.Move.y != 0) _sitdown = false;
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
        
        private float _frameLeftGrounded = float.MinValue;
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
                    _flewInvoked = false;
                    GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
                    break;
                case true when !groundHit:
                    _grounded = false;
                    _frameLeftGrounded = _time;
                    GroundedChanged?.Invoke(false, 0);
                    break;
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Flying

        private bool _flyToConsume;
        private bool _flewInvoked;

        private void HandleFly()
        {
            if (!_flyToConsume) return;

            _frameVelocity.y = stats.flyPower;
            if (_flewInvoked) return;
            Flew?.Invoke();
            _flewInvoked = true;
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
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = stats.groundingForce;
            }
            else
            {
                var inAirGravity = stats.fallAcceleration;
                if (_frameVelocity.y > 0) inAirGravity *= stats.flyEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -stats.maxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
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
        public bool FlyHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Flew;
        public Vector2 FrameInput { get; }
        public bool FlyHeld { get; }
    }
}