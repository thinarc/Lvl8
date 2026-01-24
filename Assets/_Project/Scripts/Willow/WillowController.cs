using _Project.Scripts.Game;
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
            GrowUpWillow();
            print("Day has started - Willow grows.");
        }
        
        private void GrowUpWillow()
        {
            if (_currentStage >= stages.Length - 1) return;
            
            foreach (var stage in stages)
                if (stage) stage.SetActive(false);
                else return;
            _currentStage += 1;
            stages[_currentStage].gameObject.SetActive(true);
            var corePos = stages[_currentStage].transform.Find("CorePoint");
            var branches = stages[_currentStage].GetComponentsInChildren<BranchCore>();
            _core.UpdateCore(corePos, forms[_currentStage].startHp, forms[_currentStage].glowModifier, branches, forms[_currentStage]);
        }
        
        private void OnWillowDied()
        {
            
        }
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