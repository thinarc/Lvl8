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

            if (Subscribed)
            {
                Subscribed = true;
                return;
            }
            _dayNightController.DayStarted += OnDayStarted;
            _dayNightController.BeforeDayStarted += OnBeforeDay;
            _core.OnDied += OnWillowDied;
            Subscribed = false;
        }

        private void OnDayStarted()
        {
            if (_isDead) return;
            GrowUpWillow();
        }

        private void ThrowGlitter()
        {
            var glitter = Resources.Load<GameObject>("Glitter");
            Instantiate(glitter, _core.transform.position + new Vector3(2.6f * Random.Range(-1f, 1f), 1), Quaternion.identity);
        }

        private void OnBeforeDay()
        {
            _core.CallGrew();
        }
        
        private void GrowUpWillow()
        {
            if (_currentStage >= stages.Length - 1) return;
            
            foreach (var stage in stages)
            {
                if (stage)
                {
                    if (stage.activeInHierarchy)
                    {
                        stage.GetComponent<SpriteRenderer>().DOFade(0, 0.65f).SetEase(Ease.OutBack);
                        _core.GetComponentInChildren<SpriteRenderer>().DOFade(0, 0f);
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
            if (_currentStage is 1 or 3 or 5) ThrowGlitter();
            stages[_currentStage].gameObject.SetActive(true);
            var stageScale = stages[_currentStage].GetComponent<SpriteRenderer>().transform.localScale.x;
            var stagePos = stages[_currentStage].GetComponent<SpriteRenderer>().transform.position;
            if (_currentStage != 0)
            {
                stages[_currentStage].GetComponent<SpriteRenderer>().transform.DOScale(stages[_currentStage - 1].GetComponent<SpriteRenderer>().transform.localScale.x, 0f);
                stages[_currentStage].GetComponent<SpriteRenderer>().transform.DOMove(stages[_currentStage - 1].GetComponent<SpriteRenderer>().transform.position, 0f);
            }
            stages[_currentStage].GetComponent<SpriteRenderer>().DOFade(1, 2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                ThrowGlitter();
                
            });
            stages[_currentStage].GetComponent<SpriteRenderer>().transform.DOScale(stageScale, 1.4f).SetEase(Ease.OutBack);
            stages[_currentStage].GetComponent<SpriteRenderer>().transform.DOMove(stagePos, 1.4f).SetEase(Ease.OutBack);
            _core.GetComponentInChildren<SpriteRenderer>().DOFade(1, 2f).SetEase(Ease.OutBack).SetDelay(0.4f);
            var corePos = stages[_currentStage].transform.Find("CorePoint");
            var branches = stages[_currentStage].GetComponentsInChildren<BranchCore>();
            _core.UpdateCore(corePos, forms[_currentStage].startHp, forms[_currentStage].glowModifier, branches, forms[_currentStage]);
        }
        
        private bool _isDead;

        private void OnWillowDied()
        {
            _core.GetComponentInChildren<SpriteRenderer>().DOFade(0, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
            foreach (var stage in stages)
            {
                stage.GetComponent<SpriteRenderer>().DOFade(0, 0.65f).SetEase(Ease.OutBack).SetUpdate(true);
                stage.transform.DOScale(0.5f, 2f).SetEase(Ease.OutBack).SetUpdate(true);
            }
            _isDead = true;
        }
    }
    
    [System.Serializable]
    public class WillowForm
    {
        public float startHp = 100f;
        public float glowModifier = 1.3f;
        public float healthPlayer = 5f;
        public float healthPlayerDelay = 0.2f;
        public float bugMultiplier = 1.3f;
        public int camLens;
    }
}