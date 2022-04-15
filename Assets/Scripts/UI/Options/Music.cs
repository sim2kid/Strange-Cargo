using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Options
{
    public class Music : GenericSliderSetting
    {
        public override void ApplyValue(float value)
        {
            settings.Values.MusicVolume = value;
        }
        public override float GetValue()
        {
            return settings.Values.MusicVolume;
        }
    }

}