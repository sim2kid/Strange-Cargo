using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Options
{
    public class PlayerSpeed : GenericSliderSetting
    {
        public override void ApplyValue(float value)
        {
            settings.Values.PlayerSpeed = value;
        }
        public override float GetValue()
        {
            return settings.Values.PlayerSpeed;
        }
    }
}