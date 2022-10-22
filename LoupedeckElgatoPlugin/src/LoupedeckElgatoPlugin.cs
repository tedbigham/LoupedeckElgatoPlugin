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

        public override void Load()
        {
            this.Info.Icon16x16 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("icon-16.png"));
            this.Info.Icon32x32 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("icon-32.png"));
            this.Info.Icon48x48 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("icon-48.png"));
            this.Info.Icon256x256 = EmbeddedResources.ReadImage(EmbeddedResources.FindFile("icon-256.png"));
        }
    }
}
