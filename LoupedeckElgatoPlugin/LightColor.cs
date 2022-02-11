using System;

namespace Loupedeck.LoupedeckElgatoPlugin
{
    // color range 2900K -> 7000K maps to 143 -> 344 
    public class LightColor : LightAdjustmentBase
    {
        public LightColor() : base("Light Color")
        {
        }

        protected override void AdjustValue(ElgatoLight light, Int32 ticks)
        {
            light.temperature += ticks;
            light.temperature = Math.Min(344, Math.Max(143, light.temperature));
        }

        protected override string GetValue(ElgatoLight light)
        {
            var x = light.temperature;
            
            // the *magic* formula would return 6950 so we fudge it, SimCity style.
            if (x == 143) return "7000K";
            
            var y = -0.00035 * Math.Pow(x,3) + 0.33901 * Math.Pow(x,2) - 119.40491 * x + 18129.47428;
            var t = Math.Floor(y / 50) * 50;
            
            return $"{t}K";
        }
    }
}