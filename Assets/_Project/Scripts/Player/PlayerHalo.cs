using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Player
{
    public class PlayerHalo : MonoBehaviour
    {
        [SerializeField] private ScriptableHalo stats;
        private Light2D _haloLight;
        private HaloReserve _haloReserve;
        private float _glowIntensity;
        private float _glowRadius;
        
        [SerializeField, Space(5)] private float intensityAmp = 0.3f;
        [SerializeField] private float radiusAmp = 0.4f;
        [SerializeField] private float speedPulse = 1.5f;

        private bool _inAttack;
        
        private float _attackTimer;
        private float _time;

        private float _heatlhPlayer;
        private float _healthDelay;
        private float _healthTimer;

        private void Start()
        {
            _haloLight = GetComponent<Light2D>();
            _haloReserve = FindAnyObjectByType(typeof(HaloReserve)) as HaloReserve;
            _glowIntensity = _haloLight.intensity;
            _glowRadius = _haloLight.pointLightOuterRadius;
            
            _time = Random.Range(0f, 1000f);
        }

        private void Update()
        {
            _attackTimer -= Time.deltaTime;
            _healthTimer -= Time.deltaTime;
            
            if (_inAttack) return;
            _haloLight.intensity = _glowIntensity;
            _haloLight.pointLightOuterRadius = _glowRadius;
            LightPulse.GetPulse(_haloLight, _haloLight.intensity, _haloLight.pointLightOuterRadius, _time, intensityAmp, radiusAmp);
            _time += Time.deltaTime * speedPulse;
        }
    
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Bug"))
            {
                if (!(_attackTimer <= 0)) return;
                DoAttack(other);
            }
            else if (other.CompareTag("Willow"))
            {
                if (!(_healthTimer <= 0)) return;
                DoHealth(other);
            }
        }
        
        private void DoHealth(Collider2D willowCollider)
        {
            var willowCore = willowCollider.GetComponent<Willow.WillowCore>();
            if (!willowCore.CanGiveHeal()) return;
            
            willowCore.GetHealForm(out var haloHeal, out var haloHealDelay);
            _haloReserve.AddReserve(haloHeal);
            _healthTimer = haloHealDelay;
        }

        private async void DoAttack(Collider2D bugCollider)
        {
            var bugCore = bugCollider.GetComponent<Bug.BugCorePart>();
            if (!_haloReserve.TrySpendReserve(stats.haloDamage)) return;
            var dmg = stats.haloDamage;
            bugCore.TakeDamage(dmg);
            _attackTimer = stats.haloDelay;

            _inAttack = true;
            var prev = _haloLight.intensity;
            _haloLight.intensity = prev * 2.7f;
            await UniTask.Delay(80);
            _haloLight.intensity = prev;
            _inAttack = false;
        }
    }
}