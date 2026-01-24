using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts
{
    public static class LightPulse
    {
        public static void GetPulse(Light2D component, float startIntensity, float startRadius, float time, float intensityAmp = 0.3f, float radiusAmp = 0.4f)
        {
            component.intensity = startIntensity + Mathf.Sin(time) * intensityAmp;
            component.pointLightOuterRadius = startRadius + Mathf.Sin(time) * radiusAmp;
        }
    }
}