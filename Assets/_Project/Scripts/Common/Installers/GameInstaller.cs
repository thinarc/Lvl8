using _Project.Scripts.Common.EntryPoints;
using Zenject;

namespace _Project.Scripts.Common.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEntryPoint>().AsSingle().NonLazy();
        }
    }
}