using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.HUD
{
    public class Hint : MonoBehaviour
    {
        private static readonly int ShowKey = Animator.StringToHash("Show");
        private Animator _anim;
        
        [SerializeField] private Image hintFill;
        [SerializeField] private bool show;
        [SerializeField] private float value;
        
        [SerializeField] private bool finished;
        [SerializeField] private bool cancel;

        private void Start()
        {
            _anim = GetComponent<Animator>();
        }
        
        public async UniTask ShowHint(float duration)
        {
            show = true;
            value = 0f;
            finished = false;
            var elapsed = 0f;
            while (elapsed < duration && !finished && show)
            {
                elapsed += Time.deltaTime;
                value = elapsed / duration;
                
                await UniTask.Yield();

                if (!cancel) continue;
                var elapsedCancel = elapsed;
                while (elapsedCancel > 0)
                {
                    elapsedCancel -= Time.deltaTime * 4f;
                    value = elapsedCancel / duration;
                
                    await UniTask.Yield();
                }
                show = false;
                cancel = false;
                return;
            }
            show = false;
        }

        public void CancelHint() => cancel = true;

        private void Update()
        {
            _anim.SetBool(ShowKey, show);
            if (value > 1f)
            {
                value = 1f;
                finished = true;
                Destroy(gameObject);
            }
            if (value < 0f) value = 0f;
            hintFill.fillAmount = value;
        }
    }
}
