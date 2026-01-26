using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;
        
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

            daySource.volume = startBgVolume * bgVolume;
            nightSource.volume *= startBgVolume * bgVolume;
            sfx1.volume = startSfxVolume * sfxVolume;
            sfx2.volume = startSfxVolume * sfxVolume;

            instance = this;
        }

        public void PlayDay()
        {
            daySource.DOFade(startBgVolume * bgVolume, 2f).SetEase(Ease.Linear).SetUpdate(true);
            nightSource.DOFade(0, 2f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => nightSource.Pause());
            daySource.Play();
        }

        public void PlayNight()
        {
            nightSource.DOFade(startBgVolume * bgVolume, 2f).SetEase(Ease.Linear).SetUpdate(true);
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
