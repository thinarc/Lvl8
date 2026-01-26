using _Project.Scripts.Game;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Willow
{
    public class WillowController : MonoBehaviour
    {
        [SerializeField] private GameObject[] stages;
        [SerializeField] private WillowForm[] forms;
        
        [SerializeField, Space(5)] private int startStage;
        private int _currentStage;
        
        [Inject] private DayNightController _dayNightController;
        [Inject] private WillowCore _core;
        public static bool Subscribed;
        
        private void Start()
        {
            foreach (var stage in stages)
            {
                if (stage)
                {
                    stage.SetActive(false);
                    var color = stage.GetComponent<SpriteRenderer>().color;
                    color.a = 0f;
                    stage.GetComponent<SpriteRenderer>().color = color;
                }
                else return;
            }
            _core.GetComponentInChildren<SpriteRenderer>().DOFade(0, 0f);
            
            startStage--;
            _currentStage = startStage;

            if (!Subscribed)
            {
                Subscribed = true;
                return;
            }
            _dayNightController.DayStarted += OnDayStarted;
            _core.OnDied += OnWillowDied;
            Subscribed = false;
        }

        private void OnDayStarted()
        {
            if (_isDead) return;
            GrowUpWillow();
            print("Day has started - Willow grows.");
        }

        private void ThrowGlitter()
        {
            var glitter = Resources.Load<GameObject>("Glitter");
            Instantiate(glitter, _core.transform.position + new Vector3(2.2f * Random.Range(-1f, 1f), 1), Quaternion.identity);
        }
        
        private void GrowUpWillow()
        {
            if (_currentStage >= stages.Length - 1) return;
            if (_currentStage is 2 or 4) ThrowGlitter();
            
            foreach (var stage in stages)
            {
                if (stage)
                {
                    if (stage.activeInHierarchy)
                    {
                        stage.GetComponent<SpriteRenderer>().DOFade(0, 0.8f).SetEase(Ease.OutBack);
                        _core.GetComponentInChildren<SpriteRenderer>().DOFade(0, 0.8f).SetEase(Ease.OutBack);
                    }
                    else
                    {
                        var color = stage.GetComponent<SpriteRenderer>().color;
                        color.a = 0f;
                        stage.GetComponent<SpriteRenderer>().color = color;
                    }
                    stage.SetActive(false);
                }
                else return;
            }
            _currentStage += 1;
            stages[_currentStage].gameObject.SetActive(true);
            stages[_currentStage].GetComponent<SpriteRenderer>().DOFade(1, 2f).SetEase(Ease.OutBack).OnComplete(ThrowGlitter);
            _core.GetComponentInChildren<SpriteRenderer>().DOFade(1, 2f).SetEase(Ease.OutBack).SetDelay(0.8f);
            var corePos = stages[_currentStage].transform.Find("CorePoint");
            var branches = stages[_currentStage].GetComponentsInChildren<BranchCore>();
            _core.UpdateCore(corePos, forms[_currentStage].startHp, forms[_currentStage].glowModifier, branches, forms[_currentStage]);
        }
        
        private bool _isDead;
        private void OnWillowDied() => _isDead = true;
    }
    
    [System.Serializable]
    public class WillowForm
    {
        public float startHp = 100f;
        public float glowModifier = 1.3f;
        public float healthPlayer = 5f;
        public float healthPlayerDelay = 0.2f;
    }
}