using _Project.Scripts.Common.EntryPoints;
using _Project.Scripts.Input;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Common.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private InputManager inputManagerPrefab;
        [SerializeField] private UnityEngine.EventSystems.EventSystem eventSystemPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().FromComponentInNewPrefab(inputManagerPrefab).AsSingle().NonLazy();
            Container.InstantiatePrefab(eventSystemPrefab);

            Container.BindInterfacesAndSelfTo<BootstrapEntryPoint>().AsSingle().NonLazy();
        }
    }
}