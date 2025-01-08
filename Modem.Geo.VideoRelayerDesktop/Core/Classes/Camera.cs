using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Core.Classes
{
    public class Camera
    {
        public long Id { get; set; }
        public long WellboreId { get; set; }
        public string Name { get; set; }
        public string InputUrl { get; set; }
        public string OutputUrl { get; set; }

    }
}
