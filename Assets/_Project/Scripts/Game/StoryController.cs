using System;
using System.Collections.Generic;
using _Project.Scripts.HUD;
using _Project.Scripts.Willow;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts.Game
{
    public class StoryController : MonoBehaviour
    {
        [SerializeField] private float storyDelay;
        private const float TextTemp = 2;
        
        [SerializeField, Space(10)] private StoryText[] stories;
        [SerializeField, Space(5)] private StoryText restartHello;
        [SerializeField] private StoryText defeat;
        [SerializeField, Space(5)] private StoryText victory;
        private readonly Queue<StoryText> _storiesQueue = new ();
        
        [SerializeField, Space(5)] private TextMeshProUGUI storyText;
        [SerializeField] private Light2D formatterLight;
        
        private Vector3 _startLightScale;
        private float _startLightIntensity;

        private float _targetIntensity;
        private float _storyDelayTimer;

        private void Start()
        {
            if (LangManager.LocalLang == "ru") formatterLight.transform.localScale *= 1.08f;
            
            _startLightScale = formatterLight.transform.localScale;
            _startLightIntensity = 0.2f;
            formatterLight.transform.localScale = Vector3.zero;
            formatterLight.intensity = 0f;
            UpdateText("");
            
            foreach (var story in stories) _storiesQueue.Enqueue(story);
            _storyDelayTimer = storyDelay / 20f;
            _storyDelayTimer += TextTemp;
        }

        private void Update()
        {
            if (!WillowCore.Died) _storyDelayTimer -= Time.deltaTime;
            formatterLight.intensity = Mathf.MoveTowards(formatterLight.intensity, _targetIntensity, Time.deltaTime * 0.24f);
            
            if (_storyDelayTimer <= 0) ShowStory();
        }

        private void ShowStory()
        {
            if (_storiesQueue.Count == 0) return;
            
            UpdateText(_storiesQueue.Dequeue().GetText());
            _storyDelayTimer = storyDelay;
            if (_storiesQueue.Count > 12) _storyDelayTimer /= 1.5f;
            else if (_storiesQueue.Count == 11) _storyDelayTimer /= 2.2f;
            else if (_storiesQueue.Count == 10) _storyDelayTimer /= 5f;
            _storyDelayTimer += TextTemp;
        }

        private async void UpdateText(string text, float delay = 0)
        {
            if (delay != 0) await UniTask.Delay(TimeSpan.FromSeconds(delay));
            if (!formatterLight || !storyText) return;

            if (text == "")
            {
                storyText.text = text;
                return;
            }
            var scale = _startLightScale;
            scale.x *= text.Length;
            formatterLight.transform.localScale = scale;
            _targetIntensity = _startLightIntensity;
            
            await UniTask.Delay(500);
            storyText.DOFade(1, 0f);
            storyText.text = text;
        }

        public async void OnFinishType()
        {
            if (storyText.text == "") return;
            await UniTask.Delay(TimeSpan.FromSeconds(TextTemp));
            storyText.DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() => _targetIntensity = 0);
        }
    }

    [Serializable]
    public class StoryText
    {
        [TextArea(2, 2)] public string textRu;
        [TextArea(2, 2)] public string textEn;

        public string GetText() => LangManager.LocalLang == "en" ? textEn : textRu;
    }

}