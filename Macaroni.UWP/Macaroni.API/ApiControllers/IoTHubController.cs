using Macaroni.API.Models;
using Macaroni.Common;
using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Macaroni.API.ApiControllers
{
    public class IoTHubController : ApiController
    {
        static ServiceClient serviceClient;

        public void SendMessage(IoTMessage message)
        {
            serviceClient = ServiceClient.CreateFromConnectionString(Constants.HubConnectionString);
            Task.Run(() => { serviceClient.SendAsync(message.DeviceID, new Message(Encoding.ASCII.GetBytes(message.Text))); });
        }
    }

}