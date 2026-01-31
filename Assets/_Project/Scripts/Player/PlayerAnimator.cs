using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        [SerializeField] private SpriteRenderer sprite;
        
        [SerializeField, Range(1f, 3f)] private float maxIdleSpeed = 2;
        [SerializeField] private float maxTilt = 5;
        [SerializeField] private float tiltSpeed = 20;
        
        [SerializeField] private ParticleSystem flyParticles;
        [SerializeField] private ParticleSystem launchParticles;
        [SerializeField] private ParticleSystem moveParticles;
        [SerializeField] private ParticleSystem landParticles;
        
        private IPlayerController _player;
        private bool _grounded;
        
        private ParticleSystem.MinMaxGradient _currentGradient;

        private void Awake()
        {
            _player = GetComponentInParent<IPlayerController>();
        }
        
        private void OnEnable()
        {
            _player.Flew += OnFlew;
            _player.GroundedChanged += OnGroundedChanged;
        
            moveParticles.Play();
        }
        
        private void OnDisable()
        {
            _player.Flew -= OnFlew;
            _player.GroundedChanged -= OnGroundedChanged;
        
            moveParticles.Stop();
        }
        
        private void Update()
        {
            DetectGroundColor();
            HandleSpriteFlip();
            HandleIdleSpeed();
            HandleCharacterTilt();
            if (_player.FrameInput.y > 0) OnFlew();
            else if (_player.FrameInput.y == 0) anim.SetTrigger("Idled");
            else if (_player.FrameInput.y < 0 && !_grounded) OnFlew();
            
            if (_player.FrameInput.y > 0) anim.SetFloat(FlewSpeedKey, 1.25f);
            else anim.SetFloat(FlewSpeedKey, 0.62f);
        }
        
        private void HandleSpriteFlip()
        {
            if (_player.FrameInput.x != 0) sprite.flipX = _player.FrameInput.x < 0;
        }
        
        private void HandleIdleSpeed()
        {
            var inputStrength = Mathf.Abs(_player.FrameInput.x);
            anim.SetFloat(IdleSpeedKey, Mathf.Lerp(1, maxIdleSpeed, inputStrength));
            moveParticles.transform.localScale = Vector3.MoveTowards(moveParticles.transform.localScale, Vector3.one * inputStrength, 2 * Time.deltaTime);
        }
        
        private void HandleCharacterTilt()
        {
            var runningTilt = Quaternion.Euler(0, 0, maxTilt * _player.FrameInput.x);
            anim.transform.up = Vector3.RotateTowards(anim.transform.up, runningTilt * Vector2.up, tiltSpeed * Time.deltaTime, 0f);
        }
        
        private void OnFlew()
        {
            anim.SetTrigger(FlewKey);
            anim.ResetTrigger(GroundedKey);
            
            if (_grounded)
            {
                SetColor(flyParticles);
                SetColor(launchParticles);
                flyParticles.Play();
            }
        }
        
        private void OnGroundedChanged(bool grounded, float impact)
        {
            _grounded = grounded;
            
            if (grounded)
            {
                DetectGroundColor();
                SetColor(landParticles);
        
                anim.SetTrigger(GroundedKey);
                moveParticles.Play();
        
                landParticles.transform.localScale = Vector3.one * Mathf.InverseLerp(0, 40, impact);
                landParticles.Play();
            }
            else
            {
                // moveParticles.Stop();
            }
        }
        
        private void DetectGroundColor()
        {
            var hit = Physics2D.Raycast(transform.position, Vector3.down, 2);
        
            if (!hit || hit.collider.isTrigger || !hit.transform.TryGetComponent(out SpriteRenderer r)) return;
            var color = r.color;
            _currentGradient = new ParticleSystem.MinMaxGradient(color * 0.9f, color * 1.2f);
            SetColor(moveParticles);
        }
        
        private void SetColor(ParticleSystem ps)
        {
            var main = ps.main;
            main.startColor = _currentGradient;
        }
        
        private static readonly int GroundedKey = Animator.StringToHash("Grounded");
        private static readonly int IdleSpeedKey = Animator.StringToHash("IdleSpeed");
        private static readonly int FlewKey = Animator.StringToHash("Flew");
        private static readonly int FlewSpeedKey = Animator.StringToHash("FlewSpeed");
    }
}