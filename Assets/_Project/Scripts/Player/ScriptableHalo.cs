using UnityEngine;

namespace _Project.Scripts.Player
{
    [CreateAssetMenu]
    public class ScriptableHalo : ScriptableObject
    {
        public float haloDamage = 5f;
        public float haloDelay = 0.4f;
        public float splashMultiplier = 2f;
    }
}