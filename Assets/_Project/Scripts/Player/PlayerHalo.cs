using _Project.Scripts.Upgrades;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Player
{
    public class PlayerHalo : MonoBehaviour, IUpgradeTarget
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

        private bool _splashed;
        private float _startRad;
        
        private float _dmgMultiplier = 1f;
        private float _splashMulti = 1f;
        private float _attDelay;

        private void Start()
        {
            _haloLight = GetComponent<Light2D>();
            _haloReserve = FindAnyObjectByType(typeof(HaloReserve)) as HaloReserve;
            _glowIntensity = _haloLight.intensity;
            _glowRadius = _haloLight.pointLightOuterRadius;
            
            _startRad = GetComponent<CircleCollider2D>().radius;

            _splashMulti = stats.splashMultiplier;
            _attDelay = stats.haloDelay;
            
            _time = Random.Range(0f, 1000f);
            
            _healthTimer = 0.6f;
        }


        public void ApplyUpgrade(UpgradeData upgrade)
        {
            if (upgrade.type == UpgradeType.HaloRadius)
            {
                _glowRadius *= upgrade.value;
                _startRad *= upgrade.value;
                GetComponent<CircleCollider2D>().radius *= upgrade.value;
                _glowIntensity *= upgrade.value / 1.6f;
                speedPulse += 0.1f;
            }

            if (upgrade.type == UpgradeType.HaloPower)
            {
                _dmgMultiplier *= upgrade.value;
                _glowIntensity *= 1.064f;
                speedPulse += 0.1f;
            }

            if (upgrade.type == UpgradeType.SplashMultiplier)
            {
                _splashMulti *= upgrade.value;
            }

            if (upgrade.type == UpgradeType.HaloCooldown)
            {
                _attDelay /= upgrade.value;
            }
        }

        private void Update()
        {
            _attackTimer -= Time.deltaTime;
            _healthTimer -= Time.deltaTime;
            
            if (_inAttack) return;
            var intensity = _glowIntensity;
            var rad = _glowRadius;
            if (_splashed)
            {
                intensity *= (stats.splashMultiplier / 2);
                rad /= (stats.splashMultiplier / 2);
            }
            _haloLight.intensity = Mathf.MoveTowards(_haloLight.intensity, intensity, Time.deltaTime * speedPulse);
            _haloLight.pointLightOuterRadius = Mathf.MoveTowards(_haloLight.pointLightOuterRadius, rad, Time.deltaTime * speedPulse);
            LightPulse.GetPulse(_haloLight, intensity, rad, _time, intensityAmp, radiusAmp);
            _time += Time.deltaTime * speedPulse;

            if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1) && !_splashed)
            {
                GetComponent<CircleCollider2D>().radius = _startRad / (stats.splashMultiplier / 2);
                _splashed = true;
            }
            else if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse1) && _splashed)
            {
                GetComponent<CircleCollider2D>().radius = _startRad;
                _splashed = false;
            }

            if (_haloLight.intensity > 2.3f)
            {
                Debug.Log("strange intensity " + _haloLight.intensity);
                // if (_glowIntensity > 2.3f) _glowIntensity = 2.3f;
                // _haloLight.intensity = 2.3f;
            }
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
            _haloReserve.AddReserve(haloHeal, true);
            _healthTimer = haloHealDelay;
        }

        private async void DoAttack(Collider2D bugCollider)
        {
            var bugCore = bugCollider.GetComponent<Bug.BugCorePart>();
            var dmgWithoutUpgrade = stats.haloDamage;
            if (_splashed) dmgWithoutUpgrade *= stats.splashMultiplier;
            var dmg = dmgWithoutUpgrade * _dmgMultiplier;
            dmgWithoutUpgrade *= _dmgMultiplier / 1.46f;
            if (!_haloReserve.TrySpendReserve(dmgWithoutUpgrade)) return;
            var retHealth = bugCore.TakeDamage(dmg);
            _haloReserve.AddReserve(retHealth);
            _attackTimer = _attDelay;

            _inAttack = true;
            var prev = _haloLight.intensity;
            _haloLight.intensity = prev * 3.1f;
            await UniTask.Delay(80);
            _haloLight.intensity = prev;
            _inAttack = false;
        }
    }
}