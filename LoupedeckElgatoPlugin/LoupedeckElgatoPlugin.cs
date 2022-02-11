namespace Loupedeck.LoupedeckElgatoPlugin
{
    using System;

    public class LoupedeckElgatoPlugin : Plugin
    {
        public override bool UsesApplicationApiOnly => true;
        public override bool HasNoApplication => true;
        
        public override void RunCommand(String commandName, String parameter)
        {
        }
        
        public override void ApplyAdjustment(String adjustmentName, String parameter, Int32 diff)
        {
        }
    }
}
