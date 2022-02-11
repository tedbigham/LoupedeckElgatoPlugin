using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Tmds.MDns;

namespace Loupedeck.LoupedeckElgatoPlugin
{
    public class ElgatoDeviceManager
    {
        private ServiceBrowser _serviceBrowser;
        public Dictionary<string, ElgatoDevice> _devices = new Dictionary<string, ElgatoDevice>();
        private List<IAdjustment> _adjustments = new List<IAdjustment>();

        public static readonly ElgatoDeviceManager Instance = new ElgatoDeviceManager();

        private ElgatoDeviceManager()
        {
            Init();
        }
        
        private void Init()
        {
            _serviceBrowser = new ServiceBrowser();
            _serviceBrowser.ServiceAdded += OnServiceAdded;
            _serviceBrowser.StartBrowse("_elg._tcp");
        }

        public void Stop()
        {
            _serviceBrowser.StopBrowse();
            _serviceBrowser = null;
        }

        public void RegisterAdjustment<T>(T adjustment) where T : IAdjustment
        {
            if (_serviceBrowser == null)
                Init();
            
            _adjustments.RemoveAll(a => a.GetType() == typeof(T));
            _adjustments.Add(adjustment);
        }

        private void OnServiceAdded(Object sender, ServiceAnnouncementEventArgs e)
        {
            var a = e.Announcement;
            if (_devices.ContainsKey(a.Instance))
                return;
            
            AddDevice(a);
        }

        private void AddDevice(ServiceAnnouncement a)
        {
            var uri = MakeUri(a);
            var state = GetDeviceState(uri);
            var device = new ElgatoDevice
            {
                Name = a.Instance,
                State = state,
                Uri = uri,
            };
            _devices[a.Instance] = device;
            _adjustments.ForEach(p => p.OnDeviceAdded(device));
        }

        private Uri MakeUri(ServiceAnnouncement a)
        {
            return new Uri($"http://{a.Addresses[0]}:{a.Port}/elgato/lights");
        }

        public void UpdateRemote(ElgatoDevice device)
        {
            Send(device.Uri, "PUT", JsonConvert.SerializeObject(device.State));
        }

        private DeviceState GetDeviceState(Uri uri)
        {
            var result = Send(uri, "GET");
            if (result == null)
                return null;
            
            return JsonConvert.DeserializeObject<DeviceState>(result);
        } 

        private string Send(Uri uri, string method, string body = "")
        {
            Console.WriteLine(uri);
            using (var client = new WebClient())
            {
                return body != "" ? client.UploadString(uri, method, body) : client.DownloadString(uri);
            }
        }
    }
}
