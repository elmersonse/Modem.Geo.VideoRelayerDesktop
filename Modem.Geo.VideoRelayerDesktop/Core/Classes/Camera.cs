using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Core.Classes
{
    internal class Camera
    {
        string StreamKey;
        string Name;

        public Camera() { }

        public Camera(string streamKey, string name)
        {
            StreamKey = streamKey;
            Name = name;
        }

        public string GetStreamKey()
        {
            return StreamKey;
        }

        public string GetName()
        {
            return Name;
        }
    }
}
