using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        
        public AudioSource idleSource;
        public AudioSource daySource;
        public AudioSource nightSource;
        public AudioSource sfx1;
        public AudioSource sfx2;

        public float bgVolume = 1;
        public float sfxVolume = 0.75f;

        public float startBgVolume;
        public float startSfxVolume;

        private void Start()
        {
            startBgVolume = daySource.volume;
            startSfxVolume = sfx1.volume;

            idleSource.volume = startBgVolume * bgVolume;
            daySource.volume = startBgVolume * bgVolume;
            nightSource.volume *= startBgVolume * bgVolume;
            sfx1.volume = startSfxVolume * sfxVolume;
            sfx2.volume = startSfxVolume * sfxVolume;

            instance = this;
        }

        public void UpdateSaves(float bg, float sfx)
        {
            bgVolume = bg;
            sfxVolume = sfx;
            
            idleSource.volume = startBgVolume * bgVolume;
            daySource.volume = startBgVolume * bgVolume;
            nightSource.volume *= startBgVolume * bgVolume;
            sfx1.volume = startSfxVolume * sfxVolume;
            sfx2.volume = startSfxVolume * sfxVolume;
        }

        public void ResetSound()
        {
            idleSource.time = 0f;
            daySource.time = 0f;
            nightSource.time = 0f;
            _dayQueue = 0;
        }

        private int _dayQueue;
        public void PlayDay()
        {
            if (_dayQueue == 0)
            {
                idleSource.DOFade(startBgVolume * bgVolume, 2f).SetEase(Ease.Linear).SetUpdate(true);
                daySource.DOFade(0, 2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => daySource.Pause());
                idleSource.Play();
                _dayQueue = 1;
            }
            else
            {
                daySource.DOFade(startBgVolume * bgVolume, 2f).SetEase(Ease.Linear).SetUpdate(true);
                idleSource.DOFade(0, 2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => idleSource.Pause());
                daySource.Play();
            }
            nightSource.DOFade(0, 2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => nightSource.Pause());
        }

        public void PlayNight()
        {
            nightSource.DOFade(startBgVolume * bgVolume, 2f).SetEase(Ease.Linear).SetUpdate(true);
            idleSource.DOFade(0, 2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => idleSource.Pause());
            daySource.DOFade(0, 2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => daySource.Pause());
            nightSource.Play();
        }

        public void PlaySfx(AudioClip clip)
        {
            print(clip.name);
            sfx1.PlayOneShot(clip);
        }
        
        public void PlaySfx2(AudioClip clip)
        {
            sfx2.PlayOneShot(clip);
        }
    }
}
