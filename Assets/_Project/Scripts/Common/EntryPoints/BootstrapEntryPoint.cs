using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Scripts.Common.EntryPoints
{
    public class BootstrapEntryPoint : IInitializable
    {
        public void Initialize()
        {
            SceneManager.LoadScene("Game");
        }
    }
}