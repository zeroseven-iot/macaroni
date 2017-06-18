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
using Macaroni.Common;

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
        static string connectionString = Constants.HubConnectionString;
        static string iotHubUri = Constants.HubUrI;
        string deviceId = "Macaroni_01";

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary> Load finished page event </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args</param>
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtOutput.Text += "Getting device key" + Environment.NewLine;
            string deviceKey = await GetDeviceKey();
            txtOutput.Text += "Device key is: " + deviceKey + Environment.NewLine;
            
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey), Microsoft.Azure.Devices.Client.TransportType.Mqtt);

            txtOutput.Text += "Device client successfully created, waiting for messages" + Environment.NewLine;
            ReceiveC2dAsync();
        }

        /// <summary> Gets the device key </summary>
        /// <returns></returns>
        private async Task<string> GetDeviceKey()
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            Device device;
            try
            {
                // TODO: the device ID should be generated automatically (maybe a GUID) and stored in the local storage of the device
                // then check if there is something already stored and use that before generate another GUID
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (Exception)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }

            return device.Authentication.SymmetricKey.PrimaryKey;
        }

        /// <summary> Open connection with the hub and wait for messages </summary>
        private async void ReceiveC2dAsync()
        {
            // loop never finishes
            while (true)
            {
                Microsoft.Azure.Devices.Client.Message receivedMessage = null;
                try
                {
                    // await for a message
                    receivedMessage = await deviceClient.ReceiveAsync();

                    // when message received check if not null
                    if (receivedMessage == null) continue;

                    // Decode message and add to the output box
                    string message = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    txtOutput.Text += "Message received: " + message + Environment.NewLine;
                }
                finally
                {
                    // send a message back saying we are done with the message
                    if(receivedMessage != null)
                    {
                        await deviceClient.CompleteAsync(receivedMessage);
                    }
                }
            }
        }
    }
}
