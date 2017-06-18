using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Macaroni.API.Models
{
    public class IoTMessage
    {
        public string DeviceID { get; set; }
        public string Text { get; set; }
    }
}