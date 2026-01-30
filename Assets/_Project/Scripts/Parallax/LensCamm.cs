using _Project.Scripts.Willow;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Scripts.Parallax
{
    public class LensCamm : MonoBehaviour
    {
        public WillowCore core;
        public CinemachineCamera startCam;
        public CinemachineCamera[] vCams;
        public Transform player;

        private int _currentCam;

        private void Start()
        {
            core.OnGrew += ChangeCam;
            ChangeCam();
        }
        
        private async void ChangeCam()
        {
            if (_currentCam != core.GetCamLens() || startCam.gameObject.activeInHierarchy)
            {
                if (startCam.gameObject.activeInHierarchy)
                {
                    await UniTask.Delay(100);
                    if (startCam != null) startCam.gameObject.SetActive(false);
                }
                _currentCam = core.GetCamLens();
                foreach (var cam in vCams)
                    if (cam != null) cam.gameObject.SetActive(false);
                    else return;
                try
                {
                    vCams[core.GetCamLens()].gameObject.SetActive(true);
                }
                catch
                {
                    vCams[^1].gameObject.SetActive(true);
                }
            }
        }
    }
}