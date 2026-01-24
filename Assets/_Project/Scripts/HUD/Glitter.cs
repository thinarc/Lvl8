using System;
using _Project.Scripts.Player;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.HUD
{
    public class Glitter : MonoBehaviour
    {
        private bool _triggered;

        private void PickupGlitter()
        {
            GetComponent<SpriteRenderer>().DOFade(0f, 0.1f).SetEase(Ease.InExpo);
            transform.DOMove(FindAnyObjectByType<PlayerController>().transform.position + new Vector3(0, 0.08f), 0.1f).SetEase(Ease.Flash);
            transform.DOScale(0f, 0.2f).SetEase(Ease.InExpo).OnComplete(() => Destroy(gameObject));
        }
        
        private async void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _triggered = true;
                var hint = GetComponentInChildren<Hint>();
                if (!hint) return;
                await hint.ShowHint(1);
                if (_triggered) PickupGlitter();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                GetComponentInChildren<Hint>()?.CancelHint();
                _triggered = false;
            }
        }
    }
}
