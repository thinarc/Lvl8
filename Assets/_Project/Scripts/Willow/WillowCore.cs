using System;
using _Project.Scripts.Game;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using WaypointSystem;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Willow
{
    public class WillowCore : MonoBehaviour
    {
        [SerializeField] private float startHp = 100f;
        [SerializeField, Header("Model")] private float hp;
        private float _animHp;

        [SerializeField, Space(5)] private float intensityAmp = 0.3f;
        [SerializeField] private float radiusAmp = 0.4f;
        [SerializeField] private float speedPulse = 1.5f;
        
        [SerializeField] private BranchCore[] branches;
        [SerializeField] private WillowForm form;

        private Light2D _glow;
        private float _glowIntensity;
        private float _glowRadius;
        
        [Inject] private DayNightController _dayNightController;
        private bool _died;
        private float _time;

        public static bool Died;
        
        public float CurrentHp => hp;
        public float MaxHp => startHp;
        public event Action OnDied;
        
        private void Start()
        {
            _died = false;
            Died = false;
            
            hp = startHp;
            _animHp = hp;
            
            _glow = GetComponentInChildren(typeof(Light2D)) as Light2D;
            _glowIntensity = _glow.intensity;
            _glowRadius = _glow.pointLightOuterRadius;
            
            _time = Random.Range(0f, 1000f);

            _stGlowRadius = _glowRadius;
            _stGlowIntensity = _glowIntensity;
            _stSpeedPulse = speedPulse;
            _radiusCollider = GetComponent<CircleCollider2D>().radius;
        }

        private void Update()
        {
            _animHp = Mathf.MoveTowards(_animHp, hp, Time.deltaTime * 40);
            
            _glow.intensity = _glowIntensity * (_animHp / startHp);
            _glow.pointLightOuterRadius = _glowRadius * (_animHp / startHp);
            LightPulse.GetPulse(_glow, _glow.intensity, _glow.pointLightOuterRadius, _time, intensityAmp, radiusAmp);
            
            _time += Time.deltaTime * speedPulse;
        }
        
        private float _stGlowRadius;
        private float _stGlowIntensity;
        private float _stSpeedPulse;
        private float _radiusCollider;
        public void UpdateCore(Transform corePos, float newStartHp, float glowModifier, BranchCore[] newBranches, WillowForm newForm)
        {
            transform.position = corePos.position;
            
            var diff = newStartHp - startHp;
            startHp = newStartHp;
            hp += diff;
            
            _glowRadius = _stGlowRadius * glowModifier;
            if (glowModifier / 4 > 1) _glowIntensity = _stGlowIntensity * glowModifier / 4f;
            speedPulse = _stSpeedPulse * glowModifier / 2;
            GetComponent<CircleCollider2D>().radius = _radiusCollider * glowModifier;
            
            branches = newBranches;
            ReadyToReceiveBranches?.Invoke();

            form = newForm;
        }
        
        public void GetHealForm(out float health, out float healDelay)
        {
            health = form.healthPlayer;
            healDelay = form.healthPlayerDelay;
        }

        public bool CanGiveHeal()
        {
            return !_died && _dayNightController.currentPhase == DayNightController.DayPhase.Day && Time.timeScale != 0f;
        }
        
        public event Action ReadyToReceiveBranches;
        public Waypoint GetRandomBranch()
        {
            if (branches.Length == 0) return null;
            var branch = branches[Random.Range(0, branches.Length)];
            return branch.GetWaypoint();
        }
        public bool CanGiveRandomBranch()
        {
            return branches.Length > 0;
        }
        
        public bool CanTakeDamage() => !_died && Time.timeScale != 0f;
    
        public void TakeDamage(float damage)
        {
            if (hp - damage > 0) hp -= damage;
            else hp = 0;

            if (!(hp <= 0) || _died) return;
            _died = true;
            Died = true;
            OnDied?.Invoke();
            print("Willow has died.");
        }
    }
}
