using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Bug
{
    public class BugCorePart : MonoBehaviour
    {
        [SerializeField] private float startHp = 10f;
        [SerializeField, Header("Model")] private float hp;
        private float _animHp;
        private float _expandedHp;
        
        [SerializeField, Space(5)] private float intensityAmp = 0.3f;
        [SerializeField] private float radiusAmp = 0.4f;
        [SerializeField] private float speedPulse = 1.5f;

        private Light2D _glow;
        private float _glowIntensity;
        private float _glowRadius;
        
        private float _time;
        
        private void Start()
        {
            hp = startHp;
            _animHp = hp;
            
            _glow = GetComponentInChildren(typeof(Light2D)) as Light2D;
            _glowIntensity = _glow.intensity;
            _glowRadius = _glow.pointLightOuterRadius;
            
            _time = Random.Range(0f, 1000f);
        }

        private void Update()
        {
            _animHp = Mathf.MoveTowards(_animHp, hp, Time.deltaTime * 40);
            
            _glow.intensity = _glowIntensity * (_animHp / startHp);
            _glow.pointLightOuterRadius = _glowRadius * (_animHp / (startHp - _expandedHp));
            LightPulse.GetPulse(_glow, _glow.intensity, _glow.pointLightOuterRadius, _time, intensityAmp, radiusAmp);
            
            _time += Time.deltaTime * speedPulse;
        }
    
        public async void TakeDamage(float damage)
        {
            if (hp - damage > 0)
            {
                hp -= damage;
                var color = _glow.color;
                _glow.color = Color.yellow / 1.4f;
                await UniTask.Delay(100);
                _glow.color = color;
            }
            else hp = 0;

            if (!(hp <= 0)) return;
            Destroy(gameObject);
        }
        
        public void TakeHp(float hpToAdd)
        {
            startHp += hpToAdd;
            _expandedHp += hpToAdd;
            hp += hpToAdd;
        }
    }
}
