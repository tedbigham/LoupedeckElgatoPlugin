using System;

namespace Loupedeck.LoupedeckElgatoPlugin
{
    public class LightBrightness : LightAdjustmentBase
    {
        public LightBrightness() : base("Light Brightness")
        {
        }

        protected override void AdjustValue(ElgatoLight light, Int32 ticks)
        {
            light.brightness += ticks;
            light.brightness = Math.Min(100, Math.Max(0, light.brightness));
        }

        protected override string GetValue(ElgatoLight light)
        {
            return $"{light.brightness}%";
        }
    }
}
