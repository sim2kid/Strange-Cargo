using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Environment
{
    [ExecuteAlways]
    public class SkyboxController : MonoBehaviour
    {
        [SerializeField]
        Gradient CloudColor;
        [SerializeField]
        Gradient SkyColor;
        [SerializeField]
        Gradient HorizonColor;
        [SerializeField]
        Gradient UnderskyColor;
        [SerializeField]
        AnimationCurve StarIntensity;
        [SerializeField]
        float StarMultiplier = 1;

        [SerializeField]
        Gradient FogColor;
        [SerializeField]
        AnimationCurve FogDensity;
        [SerializeField]
        float FogIntensity = 0.03f;

        TimeController time;
        UnityEngine.Material skybox;
        void Start()
        {
            time = GetComponent<TimeController>();
            if(time == null && Application.IsPlaying(gameObject))
                time = GetComponent<TimeController>();
            skybox = RenderSettings.skybox;
        }

        void Update()
        {
            if (time == null)
                return;

            if(RenderSettings.fogMode != FogMode.ExponentialSquared)
                RenderSettings.fogMode = FogMode.ExponentialSquared;

            float dayPercent = time.DayProgress;
            skybox.SetFloat("_Key", dayPercent);

            RenderSettings.fogDensity = Mathf.Clamp01(FogDensity.Evaluate(dayPercent)) * FogIntensity;

            dayPercent = -Mathf.Cos(dayPercent * 2 * Mathf.PI) + 0.5f;

            skybox.SetColor("_CloudColor", CloudColor.Evaluate(dayPercent));
            skybox.SetColor("_SkyColor", SkyColor.Evaluate(dayPercent));
            skybox.SetColor("_HorizonColor", HorizonColor.Evaluate(dayPercent));
            skybox.SetColor("_UnderskyColor", UnderskyColor.Evaluate(dayPercent));

            skybox.SetFloat("_StarIntensity", Mathf.Clamp01(StarIntensity.Evaluate(dayPercent)) * StarMultiplier);
            RenderSettings.fogColor = FogColor.Evaluate(dayPercent);
            
        }
    }
}