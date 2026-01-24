using _Project.Scripts.Bug;
using _Project.Scripts.Game;
using _Project.Scripts.HUD;
using _Project.Scripts.Willow;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Common.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private WillowCore willowCore;
        [SerializeField] private DayNightController dayNightController;
        
        public async override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DayNightController>().FromInstance(dayNightController).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WillowCore>().FromInstance(willowCore).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BugAttackPart>().FromInstance(FindAnyObjectByType(typeof(BugAttackPart))).AsSingle().NonLazy();
            
            await Container.Resolve<LangManager>().SelectLang();
        }
    }
}