using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Project.Scripts
{
    public static class LightPulse
    {
        public static void GetPulse(Light2D component, float startIntensity, float startRadius, float time, float intensityAmp = 0.3f, float radiusAmp = 0.4f)
        {
            var intensity = startIntensity + Mathf.Sin(time) * intensityAmp;
            var pointLightOuterRadius = startRadius + Mathf.Sin(time) * radiusAmp;
            component.intensity = Mathf.MoveTowards(component.intensity, intensity, Time.deltaTime * 1.5f);
            component.pointLightOuterRadius = Mathf.MoveTowards(component.pointLightOuterRadius, pointLightOuterRadius, Time.deltaTime * 1.5f);
        }
    }
}