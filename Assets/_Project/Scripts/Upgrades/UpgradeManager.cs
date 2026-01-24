using System.Collections.Generic;
using _Project.Scripts.HUD;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        [SerializeField] private List<UpgradeData> allUpgrades;
        [SerializeField] private RectTransform[] views;

        private async void Start()
        {
            foreach (var view in views)
            {
                view.transform.DOScale(0, 0);
            }
            
            await UniTask.Delay(500);
            UpdateViews();
        }

        private void UpdateViews()
        {
            var updates = GetRandomUpgrades(views.Length);
            var queue = new Queue<UpgradeData>();
            foreach (var u in updates) queue.Enqueue(u);

            for (var i = 0; i < views.Length; i++)
            {
                var update = queue.Dequeue();
                var view = views[i];
                var label = "";
                var desc = "";
                var lang = LangManager.LocalLang;
                if (lang == "ru")
                {
                    label = update.labelRu;
                    desc = update.descRu;
                }
                else
                {
                    label = update.labelEn;
                    desc = update.descEn;
                }
                
                if (view == null) return;
                var n = view.Find("name").GetComponent<TextMeshProUGUI>();
                var d = view.Find("desc").GetComponent<TextMeshProUGUI>();
                n.text = label;
                d.text = desc;

                view.transform.DOScale(0, 0);
                view.transform.DOScale(1, 0.5f).SetDelay(i * 0.2f).SetEase(Ease.OutBack);
            }
        }

        public List<UpgradeData> GetRandomUpgrades(int count)
        {
            var pool = new List<UpgradeData>(allUpgrades);
            List<UpgradeData> result = new();

            for (var i = 0; i < count && pool.Count > 0; i++)
            {
                var index = Random.Range(0, pool.Count);
                result.Add(pool[index]);
                pool.RemoveAt(index);
            }

            return result;
        }

        public void ApplyUpgrade(UpgradeData upgrade, IUpgradeTarget target)
        {
            target.ApplyUpgrade(upgrade);
        }
    }
}