using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Core.Classes
{
    internal class CameraCollection
    {
        private List<Camera> _cameras;

        public CameraCollection() 
        {
            _cameras = new List<Camera>();
        }

        public List<Camera> GetCameraCollection()
        {
            List<Camera> list = new List<Camera>(_cameras);
            return list;
        }

        public Response AddCamera(Camera camera)
        {
            Response response;
            try
            {
                _cameras.Add(camera);
                response = new Response(Enums.Status.Ok, "");
            }
            catch(Exception ex)
            { 
                response = new Response(Enums.Status.Error, ex.Message);
            }
            return response;
        }


    }
}
