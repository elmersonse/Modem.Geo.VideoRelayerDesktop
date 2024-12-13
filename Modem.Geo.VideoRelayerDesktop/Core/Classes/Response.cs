using Modem.Geo.VideoRelayerDesktop.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Core.Classes
{
    internal class Response<T>
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public Response(Status status, string message)
        {
            Status = status;
            Message = message;
            Data = default(T);
        }
    }
}
