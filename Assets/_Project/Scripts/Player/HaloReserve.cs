using _Project.Scripts.Upgrades;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Player
{
    public class HaloReserve : MonoBehaviour, IUpgradeTarget
    {
        [SerializeField] private float maxReserve = 100f;
        [SerializeField] private float idleRechargeRate;
        
        private Slider _reserveSlider;
        [SerializeField, Space(5)] private Image reserveFill;
        [SerializeField] private Color oldColor;
        [SerializeField] private Color blinkColor;
        [SerializeField] private Color dmgColor;
        
        private float _currentReserve;
        private float _currentAnimReserve;

        private float _willowMulti = 1f;

        private void Start()
        {
            _reserveSlider = GetComponent<Slider>();
            _reserveSlider.maxValue = maxReserve;
            
            _currentReserve = 0f;
            _currentAnimReserve = 0f;
            _reserveSlider.value = 0f;
        }
        
        public void ApplyUpgrade(UpgradeData upgrade)
        {
            if (upgrade.type == UpgradeType.ReserveMax)
            {
                maxReserve *= upgrade.value;
                _reserveSlider.maxValue = maxReserve;
            }
            
            if (upgrade.type == UpgradeType.IdleRechargeRate)
            {
                idleRechargeRate *= upgrade.value;
            }
            
            if (upgrade.type == UpgradeType.RechargeFromWillowMulti)
            {
                _willowMulti *= upgrade.value;
            }
        }

        private void Update()
        {
            if (Time.timeScale != 0f) AddReserve(idleRechargeRate * Time.deltaTime);
            
            _currentAnimReserve = Mathf.MoveTowards(_currentAnimReserve, _currentReserve, Time.deltaTime * 8);
            _reserveSlider.value = _currentAnimReserve;
        }

        private Tween _fiil;
        private Tween _fill2;
        public bool TrySpendReserve(float amount)
        {
            if (_currentReserve - amount < 0)
            {
                if (_fiil != null) _fiil.Kill();
                _fiil = reserveFill.DOColor(blinkColor, 0f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    if (_fill2 != null) _fill2.Kill();
                    _fill2 = reserveFill.DOColor(oldColor, 0.6f).SetEase(Ease.OutBack).OnComplete(() => _fiil.Kill());
                });
                return false;
            }
            _currentReserve -= amount;
            if (_fiil != null) _fiil.Kill();
            _fiil = reserveFill.DOColor(dmgColor, 0f).SetEase(Ease.InBack).OnComplete(() =>
            {
                if (_fill2 != null) _fill2.Kill();
                _fill2 = reserveFill.DOColor(oldColor, 0.6f).SetEase(Ease.OutBack).OnComplete(() => _fiil.Kill());
            });
            if (_currentReserve < 0) _currentReserve = 0;
            return true;
        }
        
        public void AddReserve(float amount, bool fromWillow = false)
        {
            if (fromWillow) amount *= _willowMulti;
            _currentReserve += amount;
            if (_currentReserve > maxReserve) _currentReserve = maxReserve;
        }
    }
}
