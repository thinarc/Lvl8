using System;
using _Project.Scripts.HUD;
using _Project.Scripts.Willow;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Zenject;

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

        public event Action BeforeDayStarted;

        public event Action Pass;

        private Light2D _global;
        public SpriteRenderer fog;

        [Inject] private SoundManager _sound;
        
        private async void Start()
        {
            _global = GetComponent<Light2D>();
            fog.DOFade(0f, 0f);
            _timer = 1f;
            
            await UniTask.NextFrame();
            await UniTask.NextFrame();
            StartDay();
        }

        private bool _callBefore;

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f) SwitchPhase();
            
            if (_timer < 2f && !_callBefore && currentPhase == DayPhase.Night)
            {
                _callBefore = true;
                BeforeDayStarted?.Invoke();
            }

            var duration = currentPhase == DayPhase.Day ? dayDuration : nightDuration;
            if (_timer / duration > 0.55f) return;
            var targetIntensity = currentPhase == DayPhase.Day ? nightIntensity : dayIntensity;
            var step = Mathf.Abs(targetIntensity - _global.intensity) / (duration * 0.4f);
            step *= 2.45f;
            _global.intensity = Mathf.MoveTowards(_global.intensity, targetIntensity, step * Time.deltaTime);
        }

        private void SwitchPhase()
        {
            if (currentPhase == DayPhase.Day) StartNight();
            else StartDay();
        }

        private void StartDay()
        {
            _callBefore = false;
            currentPhase = DayPhase.Day;
            _timer = dayDuration;
            DayStarted?.Invoke();
            PhaseChanged?.Invoke();
            
            _global.intensity = dayIntensity;
            _sound.PlayDay();
        }

        private int _nights;
        private void StartNight()
        {
            currentPhase = DayPhase.Night;
            _nights++;
            if (_nights == 6) Pass?.Invoke();
            _timer = nightDuration;
            NightStarted?.Invoke();
            PhaseChanged?.Invoke();
            
            _global.intensity = nightIntensity;
            fog.DOFade(0.8f, 0.5f).SetEase(Ease.Flash).OnComplete(() =>
            {
                fog.DOFade(0, 2.4f).SetEase(Ease.OutFlash);
            });
            
            _sound.PlayNight();
        }

        public void CallRestart()
        {
            _sound.ResetSound();
            DefeatPanel._init = false;
            WillowController.Subscribed = false;
            Stats.Sub = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            print("restart");
        }
    }
}