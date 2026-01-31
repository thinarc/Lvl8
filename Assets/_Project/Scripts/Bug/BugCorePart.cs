using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Bug
{
    public class BugCorePart : MonoBehaviour
    {
        public float startHp = 10f;
        [SerializeField, Header("Model")] private float hp;
        private float _animHp;
        private float _expandedHp;
        
        public float StartHp => startHp;
        
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


        public bool died;
        public float TakeDamage(float damage)
        {
            if (hp - damage > 0)
            {
                hp -= damage;
                var color1 = _glow.color;
                var color2 = GetComponentInChildren<SpriteRenderer>().color;
                var newColor1 = Color.red / 1.6f;
                var newColor2 = Color.red / 3f;
                newColor1.a = color1.a;
                newColor2.a = color2.a;
                _glow.color = newColor1;
                GetComponentInChildren<SpriteRenderer>().color = newColor2;
                RetColor(color1, color2);
                var scale = transform.localScale;
                transform.DOScale(scale * 1.24f, 0.2f).SetEase(Ease.Linear).OnComplete(() => transform.DOScale(scale, 0.1f));
                return 0;
            }

            died = true;
            var retHp = (hp - damage) * -1;
            hp = 0;
            transform.DOScale(0, 0.4f).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
            return retHp;
        }

        private async void RetColor(Color color1, Color color2)
        {
            await UniTask.Delay(100);
            _glow.color = color1;
            GetComponentInChildren<SpriteRenderer>().color = color2;
        }
        
        public void TakeHp(float hpToAdd)
        {
            startHp += hpToAdd / 2.4f;
            _expandedHp += hpToAdd;
            hp += hpToAdd;
        }
    }
}
