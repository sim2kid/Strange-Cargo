using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Options
{
    public class MouseSensitivity : GenericSliderSetting
    {
        public override void ApplyValue(float value)
        {
            settings.Values.MouseSensitivity = value;
        }
        public override float GetValue()
        {
            return settings.Values.MouseSensitivity;
        }
    }

}
