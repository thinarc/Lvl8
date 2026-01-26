using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.HUD
{
    public class LangManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panel1;
        [SerializeField] private CanvasGroup panel2;
        
        [SerializeField] private bool skip;
        
        private bool _notSelected = true;
        private bool _pressed;
        
        public static string LocalLang;
        
        public async UniTask SelectLang()
        {
            panel1.DOFade(0, 0f);
            panel2.DOFade(0, 0f);
            panel1.transform.DOScale(0, 0f).SetEase(Ease.InBack);
            
            if (!_notSelected) return;
            _notSelected = false;
            if (skip) _pressed = true;
            else
            {
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

        public void SelectButton(string lang)
        {
            LocalLang = lang;
            _pressed = true;

            Time.timeScale = 1;
            panel2.DOFade(0, 0.3f);
            panel1.DOFade(0, 0.2f);
            panel1.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }
    }
}