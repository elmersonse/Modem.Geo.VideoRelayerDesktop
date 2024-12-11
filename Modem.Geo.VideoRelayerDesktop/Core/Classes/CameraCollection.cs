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
                if(_cameras.FindIndex(x => x.GetName().Equals(camera.GetName())) != -1)
                {
                    response = new Response(Enums.Status.Error, $"Камера именем {camera.GetName()} уже существует");
                    return response;
                };
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
