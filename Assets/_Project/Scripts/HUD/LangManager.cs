using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.HUD
{
    public class LangManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panel1;
        [SerializeField] private CanvasGroup panel2;
        [SerializeField] private CanvasGroup panel3;
        
        [SerializeField] private bool skip;
        
        private bool _notSelected = true;
        private bool _pressed;
        private bool _pressed2;
        
        public static string LocalLang;
        
        [SerializeField] TextMeshProUGUI bgSlText;
        [SerializeField] TextMeshProUGUI sfxSlText;
        [SerializeField] TextMeshProUGUI buttonDoneText;

        [SerializeField] private Slider bgSlider;
        [SerializeField] private Slider sfxSlider;
        
        [Inject] private SoundManager _soundManager;
        
        public async UniTask SelectLang()
        {
            panel1.blocksRaycasts = false;
            panel2.blocksRaycasts = false;
            panel1.interactable = false;
            panel2.interactable = false;
            panel3.blocksRaycasts = false;
            panel3.interactable = false;
            panel1.DOFade(0, 0f);
            panel2.DOFade(0, 0f);
            panel3.DOFade(0, 0f);
            panel1.transform.DOScale(0, 0f).SetEase(Ease.InBack);
            panel3.transform.DOScale(0, 0f).SetEase(Ease.InBack);
            
            if (!_notSelected) return;
            _notSelected = false;
            if (skip) _pressed = true;
            else
            {
                panel1.blocksRaycasts = true;
                panel2.blocksRaycasts = true;
                panel1.interactable = true;
                panel2.interactable = true;
                await UniTask.Delay(100);
                Time.timeScale = 0;
                panel2.DOFade(1, 0.3f).SetUpdate(true);
                panel1.DOFade(1, 0.2f).SetUpdate(true);
                panel1.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
            }
            
            await UniTask.WaitUntil(() => _pressed);
            await UniTask.Delay(400);
            panel1.blocksRaycasts = false;
            panel2.blocksRaycasts = false;
            panel1.interactable = false;
            panel2.interactable = false;
        }

        public async void SelectButton(string lang)
        {
            LocalLang = lang;
            _pressed = true;
            
            panel1.DOFade(0, 0.3f).SetUpdate(true);
            panel1.transform.DOScale(0, 0.3f).SetEase(Ease.InBack).SetUpdate(true);
            
            panel3.blocksRaycasts = true;
            panel3.interactable = true;
            await UniTask.Delay(100, DelayType.UnscaledDeltaTime);
            panel3.DOFade(1, 0.2f).SetUpdate(true);
            panel3.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
            if (LocalLang == "ru")
            {
                buttonDoneText.text = "Ок";
            }
            
            await UniTask.WaitUntil(() => _pressed2);
            await UniTask.Delay(400);
            panel3.blocksRaycasts = false;
            panel3.interactable = false;
        }

        private void Update()
        {
            bgSlText.text = "Bg: " + (bgSlider.value * 100).ToString("F0") + "%";
            sfxSlText.text = "Sfx: " + (sfxSlider.value * 100).ToString("F0") + "%";
            if (LocalLang == "ru")
            {
                bgSlText.text = "Фон: " + (bgSlider.value * 100).ToString("F0") + "%";
                sfxSlText.text = "Звук: " + (sfxSlider.value * 100).ToString("F0") + "%";
            }
            _soundManager.UpdateSaves(bgSlider.value, sfxSlider.value);
        }

        public void SelectButton2()
        {
            _pressed2 = true;
            _soundManager.UpdateSaves(bgSlider.value, sfxSlider.value);
            Time.timeScale = 1;
            panel2.DOFade(0, 0.4f);
            panel3.DOFade(0, 0.3f);
            panel3.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }
    }
}