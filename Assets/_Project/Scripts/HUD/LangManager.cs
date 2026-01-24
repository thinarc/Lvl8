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
        private bool _pressed = false;
        
        public static string LocalLang = "en";
        
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
                panel1.DOFade(1, 0.3f).OnComplete(() => panel2.DOFade(1, 0.2f)).SetUpdate(true);
                panel1.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
            }
            
            await UniTask.WaitUntil(() => _pressed);
        }

        public void SelectButton(string lang)
        {
            LocalLang = lang;
            print("Selected lang: " + lang);

            Time.timeScale = 1;
            panel1.DOFade(0, 0.3f).OnComplete(() => panel2.DOFade(0, 0.2f));
            panel1.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
        }
    }
}