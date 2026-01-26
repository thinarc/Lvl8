using _Project.Scripts.Common.Installers;
using _Project.Scripts.Willow;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Game
{
    public class BugSpawner : MonoBehaviour
    {
        [Inject] private BugFactory _factory;
        [SerializeField] private WillowCore core;
        [Inject] private DayNightController _dayNightController;
        
        [SerializeField, Space(5)] private  SpawnStats[] spawnStats;
        [SerializeField] private int currentState;
        
        private float _spawnTimer;
        private int _currentBugs;

        private void Start()
        {
            currentState--;
            
            _spawnTimer = 2f;
            _currentBugs = 0;
            
            _dayNightController.NightStarted += OnNightStarted;
        }

        private void OnNightStarted()
        {
            currentState++;
            _currentBugs = 0;
        }

        private void Update()
        {
            _spawnTimer -= Time.deltaTime;

            if (currentState == 6)
            {
                currentState = 5;
                print("CURRENT STATE == 6, YOU CHECK THE WIN CONDITION ON DAY?");
            }

            if (!(_spawnTimer <= 0f) || _dayNightController.currentPhase == DayNightController.DayPhase.Day) return;
            if (_currentBugs >= spawnStats[currentState].maxBugs) return;
            Spawn();
            _spawnTimer = spawnStats[currentState].spawnInterval;
            _currentBugs++;
        }

        private void Spawn() => _factory.Create(core);
    }

    [System.Serializable]
    public class SpawnStats
    {
        public float spawnInterval;
        public int maxBugs;
    }
}