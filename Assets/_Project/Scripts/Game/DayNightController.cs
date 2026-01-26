using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Game
{
    public class DayNightController : MonoBehaviour
    {
        [SerializeField] private float dayDuration = 25f;
        [SerializeField] private float nightDuration = 60f;
        private float _timer;
        public float CurrentTime => _timer;
        
        [SerializeField, Space(5)] private float dayIntensity = 0.6f;
        [SerializeField] private float nightIntensity = 0.1f;

        [Header("Debug"), SerializeField] public DayPhase currentPhase;
        public enum DayPhase { Day, Night }
        
        public event Action DayStarted;
        public event Action NightStarted;
        public event Action PhaseChanged;

        private Light2D _global;
        public SpriteRenderer fog;
        
        private async void Start()
        {
            _global = GetComponent<Light2D>();
            fog.DOFade(0f, 0f);
            _timer = 1f;
            
            await UniTask.NextFrame();
            StartDay();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) SwitchPhase();

            var duration = currentPhase == DayPhase.Day ? dayDuration : nightDuration;
            if (_timer / duration > 0.4f) return;
            var targetIntensity = currentPhase == DayPhase.Day ? nightIntensity : dayIntensity;
            var step = Mathf.Abs(targetIntensity - _global.intensity) / (duration * 0.4f);
            step *= 3f;
            _global.intensity = Mathf.MoveTowards(_global.intensity, targetIntensity, step * Time.deltaTime);
        }

        private void SwitchPhase()
        {
            if (currentPhase == DayPhase.Day) StartNight();
            else StartDay();
        }

        private void StartDay()
        {
            currentPhase = DayPhase.Day;
            _timer = dayDuration;
            DayStarted?.Invoke();
            PhaseChanged?.Invoke();
            
            _global.intensity = dayIntensity;
        }

        private void StartNight()
        {
            currentPhase = DayPhase.Night;
            _timer = nightDuration;
            NightStarted?.Invoke();
            PhaseChanged?.Invoke();
            
            _global.intensity = nightIntensity;
            fog.DOFade(0.8f, 0.5f).SetEase(Ease.Flash).OnComplete(() =>
            {
                fog.DOFade(0, 2.4f).SetEase(Ease.OutFlash);
            });
        }
    }
}