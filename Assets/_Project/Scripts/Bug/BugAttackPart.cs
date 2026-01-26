using _Project.Scripts.Game;
using _Project.Scripts.Willow;
using UnityEngine;
using WaypointSystem;
using Zenject;

namespace _Project.Scripts.Bug
{
    public class BugAttackPart : MonoBehaviour
    {
        [SerializeField] private float delayBeforeAttack = 1f;
        [SerializeField] private float delayAttack = 0.2f;
        [SerializeField] private float damage = 5f;
        [SerializeField] private float increaseHpMultiplier = 0.4f;
        
        [Inject] private WillowCore _willowCore;
        [Inject] private DayNightController _dayNightController;
        private WaypointAgent _agent;
        private BugCorePart _core;

        private bool _startMoving;
        private bool _attackWillow;

        private float _attackTimer;

        private bool _init;
        
        [Inject]
        private void Construct()
        {
            _agent = GetComponent(typeof(WaypointAgent)) as WaypointAgent;
            _core = GetComponent(typeof(BugCorePart)) as BugCorePart;

            if (_agent) _agent.pathToFollow = null;
            
            var randNum = Random.Range(1.1f, 2.1f);
            transform.localScale *= randNum / 1.4f;
            if (_core) _core.TakeHp((_core.StartHp * randNum - _core.StartHp) / 2);
            if (_agent) _agent.speed *= Random.Range(0.8f, 2f);
        }

        private void OnWillowReady()
        {
            if (_agent) _agent.pathToFollow = _willowCore.GetRandomBranch();
            if (_agent) _agent.enabled = true;
            _prevPosition = transform.position;
        }

        private Vector3 _prevPosition;
        private void Update()
        {
            var velocity = (transform.position - _prevPosition) / Time.deltaTime;
            _prevPosition = transform.position;
            
            if (velocity.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                var rot = Quaternion.AngleAxis(angle + 30, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 5f);
            }

            
            if (_willowCore.CanGiveRandomBranch() && !_init)
            {
                OnWillowReady();
                _init = true;
            }

            if (_init && _dayNightController.currentPhase == DayNightController.DayPhase.Day)
            {
                var core = GetComponent<BugCorePart>();
                core.TakeDamage(core.startHp);
            }
            
            if (!_startMoving && _agent.currentTValue != 0) _startMoving = true;
            if (!_attackWillow && _agent.pathToFollow && _startMoving && _agent.currentTValue == 0)
            {
                _attackWillow = true;
                _attackTimer = delayBeforeAttack;
            }

            AttackWillow();
        }

        private void AttackWillow()
        {
            if (!_attackWillow || !_willowCore.CanTakeDamage()) return;
            _attackTimer -= Time.deltaTime;

            if (!(_attackTimer <= 0)) return;
            _willowCore.TakeDamage(damage);
            _attackTimer = delayAttack;
            _core.TakeHp(damage * increaseHpMultiplier);
        }
    }
}