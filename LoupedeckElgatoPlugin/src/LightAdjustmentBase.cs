using System;
using System.Timers;

namespace Loupedeck.LoupedeckElgatoPlugin
{
    public abstract class LightAdjustmentBase : PluginDynamicAdjustment, IAdjustment
    {
        // dial was turned
        protected abstract void AdjustValue(ElgatoLight light, Int32 ticks);
        
        // return the current value
        protected abstract String GetValue(ElgatoLight light);

        // we wait until dials have stopped turning before updating
        private Timer RemoteUpdateTimer;

        private const int UPDATE_DELAY_MSEC = 400;

        protected LightAdjustmentBase(string title) : base(title, "", "", true)
        {
            ElgatoDeviceManager.Instance.RegisterAdjustment(this);
            this.MakeProfileAction("list;Device");
        }

        public void OnDeviceAdded(ElgatoDevice device)
        {
            this.AddParameter(device.Name, device.Name, "Device");
        }

        protected ElgatoDevice GetDevice(string actionParameter)
        {
            return ElgatoDeviceManager.Instance._devices.ContainsKey(actionParameter)
                ? ElgatoDeviceManager.Instance._devices[actionParameter]
                : null;
        }

        protected override void ApplyAdjustment(string actionParameter, int diff)
        {
            var device = GetDevice(actionParameter);
            foreach (var light in device.State.lights)
            {
                AdjustValue(light, diff);
            }

            StartUpdateTimer(device);
            this.AdjustmentValueChanged();
            this.ActionImageChanged();
        }

        // dial was pressed, this toggles the light on/off
        protected override void RunCommand(String actionParameter)
        {
            var device = GetDevice(actionParameter);
            if (device.State.lights.Count == 0)
                return;
            var newState = 1 - device.State.lights[0].on;
            device.State.lights.ForEach(l => l.on = newState);
            ElgatoDeviceManager.Instance.UpdateRemote(device);
            this.AdjustmentValueChanged();
            this.ActionImageChanged();
        }

        // display value on device. if there is more than one light, show the first one
        protected override String GetAdjustmentValue(String actionParameter)
        {
            return GetValue(GetDevice(actionParameter).State.lights[0]);
        }

        private void StartUpdateTimer(ElgatoDevice device)
        {
            RemoteUpdateTimer?.Stop();
            RemoteUpdateTimer?.Dispose();
            RemoteUpdateTimer = new Timer(UPDATE_DELAY_MSEC);
            RemoteUpdateTimer.AutoReset = false;
            RemoteUpdateTimer.Elapsed += (sender, args) => ElgatoDeviceManager.Instance.UpdateRemote(device);
            RemoteUpdateTimer.Start();
        }
    }
}
