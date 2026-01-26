using System;
using _Project.Scripts.Game;
using _Project.Scripts.Willow;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.HUD
{
    public class Stats : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI willowText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private ParticleSystem willow;
        [SerializeField] private ParticleSystem timer;
        
        [SerializeField] private WillowCore willowCore;
        [SerializeField] private DayNightController dayNightController;
        
        public Color colorNight = Color.white;
        public Color colorDay = Color.black;
        
        private bool _initialized;
        public static bool Sub;
        private int c = 0;

        private void Start()
        {
            if (!Sub)
            {
                Sub = true;
                // return;
            }
            dayNightController.PhaseChanged += () =>
            {
                if (dayNightController.currentPhase == DayNightController.DayPhase.Night)
                    timerText.transform.parent.GetComponent<Image>().color = colorNight;
                else timerText.transform.parent.GetComponent<Image>().color = colorDay;
                print("binded Timer");
                if (c != 0) SoundManager.instance.PlaySfx(Resources.Load<AudioClip>("Timer"));
                c++;
                timer.Play();
                Sub = true;
            };
            dayNightController.NightStarted += () =>
            {
                SoundManager.instance.PlaySfx(Resources.Load<AudioClip>("Night"));
            };
        }

        private void Update()
        {
            if (willowCore.CurrentHp != 0) _initialized = true;
            if (!_initialized) return;
            
            if (willowText.text != $"{willowCore.CurrentHp} / {willowCore.MaxHp}") willow.Play();
            willowText.text = $"{willowCore.CurrentHp} / {willowCore.MaxHp}";
            var time = dayNightController.CurrentTime;
            if (time < 1f) time = 1f;
            timerText.text = time.ToString("F0") + "s";
            if (dayNightController.CurrentTime < 0.8f) timerText.text += "..";
        }
    }
}
