using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Macaroni.SendMessageToDevice
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=FerTestHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=+wWaCJYd3dYBd37c6Bkm/2/be7Y/xUM7TWeYw7iS9f4=";

        static void Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            Console.WriteLine("Type a message to send and press enter.");
            var message = Console.ReadLine();
            SendCloudToDeviceMessageAsync(message).Wait();
            Console.ReadLine();
        }

        private async static Task SendCloudToDeviceMessageAsync(string message)
        {
            var commandMessage = new Message(Encoding.ASCII.GetBytes(message));
            await serviceClient.SendAsync("Macaroni_01", commandMessage);
        }
    }
}
