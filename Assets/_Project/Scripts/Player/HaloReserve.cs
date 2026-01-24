using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Player
{
    public class HaloReserve : MonoBehaviour
    {
        [SerializeField] private float maxReserve = 100f;
        [SerializeField] private float idleRechargeRate;
        
        private Slider _reserveSlider;
        
        private float _currentReserve;
        private float _currentAnimReserve;

        private void Start()
        {
            _reserveSlider = GetComponent<Slider>();
            _reserveSlider.maxValue = maxReserve;
            
            _currentReserve = 0f;
        }

        private void Update()
        {
            AddReserve(idleRechargeRate / 100);
            
            _currentAnimReserve = Mathf.MoveTowards(_currentAnimReserve, _currentReserve, Time.deltaTime * 50);
            _reserveSlider.value = _currentAnimReserve;
        }
        
        public bool TrySpendReserve(float amount)
        {
            if (_currentReserve - amount < 0) return false;
            _currentReserve -= amount;
            if (_currentReserve < 0) _currentReserve = 0;
            return true;
        }
        
        public void AddReserve(float amount)
        {
            _currentReserve += amount;
            if (_currentReserve > maxReserve) _currentReserve = maxReserve;
        }
    }
}
