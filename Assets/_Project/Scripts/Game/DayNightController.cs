using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Game
{
    public class DayNightController : MonoBehaviour
    {
        [SerializeField] private float dayDuration = 25f;
        [SerializeField] private float nightDuration = 60f;
        private float _timer;
        
        [SerializeField, Space(5)] private float dayIntensity = 0.7f;
        [SerializeField] private float nightIntensity = 0.2f;

        [Header("Debug"), SerializeField] public DayPhase currentPhase;
        public enum DayPhase { Day, Night }
        
        public event Action DayStarted;
        public event Action NightStarted;

        private Light2D _global;
        
        private async void Start()
        {
            _global = GetComponent<Light2D>();
            
            await UniTask.Delay(1000);
            StartDay();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) SwitchPhase();

            var duration = currentPhase == DayPhase.Day ? nightDuration : dayDuration;
            if (_timer / duration > 0.8f) return;
            var targetIntensity = currentPhase == DayPhase.Day ? nightIntensity : dayIntensity;
            var step = Mathf.Abs(targetIntensity - _global.intensity) / duration;
            step *= 2.8f;
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
            
            _global.intensity = dayIntensity;
        }

        private void StartNight()
        {
            currentPhase = DayPhase.Night;
            _timer = nightDuration;
            NightStarted?.Invoke();
            
            _global.intensity = nightIntensity;
        }
    }
}