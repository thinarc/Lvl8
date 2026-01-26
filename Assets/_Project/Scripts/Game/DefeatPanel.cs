using _Project.Scripts.HUD;
using _Project.Scripts.Willow;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Game
{
    public class DefeatPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup panel1;
        [SerializeField] private CanvasGroup panel2;
        [SerializeField] private Image button;
        
        private bool _notSelected = true;
        private bool _pressed;

        public static bool _init;
        
        private void Start()
        {
            FindAnyObjectByType<WillowCore>().OnDied += OnDefeat;
            if (!_init)
            {
                _init = true;
                panel1.DOFade(0, 0f);
                panel2.DOFade(0, 0f);
                button.DOFade(0, 0f);
                panel1.blocksRaycasts = false;
                panel2.blocksRaycasts = false;
                panel1.interactable = false;
                panel2.interactable = false;
                return;
            }
            if (_init)
            {
                panel1.DOFade(0, 0f);
                panel2.DOFade(0, 0f);
                button.DOFade(0, 0f);
                panel1.blocksRaycasts = false;
                panel2.blocksRaycasts = false;
                panel1.interactable = false;
                panel2.interactable = false;
            }
        }

        private async void OnDefeat()
        {
            SoundManager.instance.PlaySfx(Resources.Load<AudioClip>("Loose"));
            
            await UniTask.Delay(1000);
            
            panel1.transform.DOScale(0, 0f).SetEase(Ease.InBack);
            button.transform.DOScale(0, 0f).SetEase(Ease.InBack);
            panel1.blocksRaycasts = true;
            panel2.blocksRaycasts = true;
            panel1.interactable = true;
            panel2.interactable = true;
            
            if (!_notSelected) return;
            _notSelected = false;
            await UniTask.Delay(100);
            Time.timeScale = 0;
            panel2.DOFade(1, 0.3f).SetUpdate(true);
            panel1.DOFade(1, 0.2f).SetUpdate(true);
            button.DOFade(1, 0.2f).SetUpdate(true).SetDelay(2.6f);
            button.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(true).SetDelay(2.6f);
            panel1.transform.DOMove(panel1.transform.position + new Vector3(0, 0.2f, 0), 0.3f).SetEase(Ease.OutBack).SetUpdate(true).SetDelay(2.6f);
            panel1.transform.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
            
            await UniTask.WaitUntil(() => _pressed);
            await UniTask.Delay(400);
            panel1.blocksRaycasts = false;
            panel2.blocksRaycasts = false;
            panel1.interactable = false;
            panel2.interactable = false;
        }
        
        public async void RestartSceneDef()
        {
            _pressed = true;
            
            Time.timeScale = 1;
            panel2.DOFade(0, 0.4f);
            panel1.DOFade(0, 0.3f);
            button.DOFade(0, 0.1f).SetUpdate(true);
            button.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
            panel1.transform.DOScale(0, 0.3f).SetEase(Ease.InBack);
            
            await UniTask.Delay(200);
            print("Scene restart");
        }
    }
}
