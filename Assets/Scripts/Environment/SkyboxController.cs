using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Environment
{
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

        TimeController time;
        UnityEngine.Material skybox;
        void Start()
        {
            time = Toolbox.Instance.TimeController;
            skybox = RenderSettings.skybox;
        }

        void Update()
        {
            float dayPercent = time.DayProgress;
            skybox.SetFloat("_Key", dayPercent);

            dayPercent = -Mathf.Cos(dayPercent * 2 * Mathf.PI) + 0.5f;

            skybox.SetColor("_CloudColor", CloudColor.Evaluate(dayPercent));
            skybox.SetColor("_SkyColor", SkyColor.Evaluate(dayPercent));
            skybox.SetColor("_HorizonColor", HorizonColor.Evaluate(dayPercent));
            skybox.SetColor("_UnderskyColor", UnderskyColor.Evaluate(dayPercent));

            skybox.SetFloat("_StarIntensity", Mathf.Clamp01(StarIntensity.Evaluate(dayPercent)) * StarMultiplier);
        }
    }
}