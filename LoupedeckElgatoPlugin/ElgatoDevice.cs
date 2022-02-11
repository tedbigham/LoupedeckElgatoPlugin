using System;
using System.Collections.Generic;

namespace Loupedeck.LoupedeckElgatoPlugin
{
    public class ElgatoDevice
    {
        public String Name;
        public Uri Uri;
        public DeviceState State;
    }

    public class DeviceState
    {
        public int numberOfLights;
        public List<ElgatoLight> lights;
    }
    
    public class ElgatoLight
    {
        public int on; // 0=off 1=on
        public int brightness;  // 0 to 100
        public int temperature; // (color) 143=2900K -> 344=7000K 
    }
}