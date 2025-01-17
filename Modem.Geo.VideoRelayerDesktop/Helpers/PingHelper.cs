using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Helpers
{
    internal static class PingHelper
    {
        public static bool PortalStatus = false;
        public static bool CurrentCameraStatus = false;
        

        public static bool AdressPing(string adress)
        {
            try
            {
                using (Ping pinger = new Ping())
                {
                    PingReply reply = pinger.Send(adress);
                    if (reply != null && reply.Status == IPStatus.DestinationHostUnreachable) 
                    {
                        throw new PingException($"{adress} is ureachable");
                    }
                    CurrentCameraStatus = true;
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                CurrentCameraStatus = false;
                return false;
            }
        }

        public static bool PortalPing()
        {
            try
            {
                using (Ping pinger = new Ping())
                {
                    PingReply reply = pinger.Send("show.innerica.ru");
                    PortalStatus = true;
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (PingException)
            {
                PortalStatus = false;
                return false;
            }
        }
    }
}
