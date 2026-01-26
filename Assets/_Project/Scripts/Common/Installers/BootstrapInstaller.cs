using _Project.Scripts.Common.EntryPoints;
using _Project.Scripts.HUD;
using _Project.Scripts.Input;
using _Project.Scripts.Willow;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Common.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private InputManager inputManagerPrefab;
        [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystemPrefab;
        [SerializeField] private LangManager langManagerPrefab;

        public override void InstallBindings()
        {
            LangManager.LocalLang = "en";
            
            WillowController.Subscribed = false;
            Stats.Sub = false;
            Container.BindInterfacesAndSelfTo<InputManager>().FromComponentInNewPrefab(inputManagerPrefab).AsSingle().NonLazy();
            Container.InstantiatePrefab(eventSystemPrefab);
            Container.BindInterfacesAndSelfTo<LangManager>().FromComponentInNewPrefab(langManagerPrefab).AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<BootstrapEntryPoint>().AsSingle().NonLazy();
        }
    }
}