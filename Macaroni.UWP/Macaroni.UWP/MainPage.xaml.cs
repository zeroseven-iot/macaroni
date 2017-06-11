using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using System.IO.IsolatedStorage;
using Microsoft.Azure.Devices.Client.Exceptions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Macaroni.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static DeviceClient deviceClient;
        static RegistryManager registryManager;
        static string connectionString = "HostName=FerTestHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=+wWaCJYd3dYBd37c6Bkm/2/be7Y/xUM7TWeYw7iS9f4=";
        static string iotHubUri = "FerTestHub.azure-devices.net";
        string deviceId = "Macaroni_01";

        public MainPage()
        {
            this.InitializeComponent();
        }


        private async Task<string> GetDeviceKey()
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            return device.Authentication.SymmetricKey.PrimaryKey;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtOutput.Text += "Getting device key" + Environment.NewLine;
            string deviceKey = await GetDeviceKey();
            txtOutput.Text += "Device key is: " + deviceKey + Environment.NewLine;
            
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey), Microsoft.Azure.Devices.Client.TransportType.Mqtt);

            txtOutput.Text += "Device client successfully created, waiting for messages" + Environment.NewLine;
            ReceiveC2dAsync();
        }

        private async void ReceiveC2dAsync()
        {
            while (true)
            {
                Microsoft.Azure.Devices.Client.Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;

                string message = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                txtOutput.Text += "Message received: " + message  + Environment.NewLine;

                await deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}
