using Modem.Geo.VideoRelayerDesktop.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Modem.Geo.VideoRelayerDesktop.Core.Classes
{
    internal class CameraCollection
    {
        private static CameraCollection instance;
        private List<Camera> _cameras;

        public static CameraCollection GetInstance()
        {
            if (instance == null)
            {
                instance = new CameraCollection();
            }
            return instance;
        }

        public CameraCollection() 
        {
            _cameras = new List<Camera>();
        }

        public List<Camera> GetCameraCollection()
        {
            List<Camera> list = new List<Camera>(_cameras);
            return list;
        }

        public Response<string> AddCamera(Camera camera)
        {
            Response<string> response;
            try
            {
                if(_cameras.FindIndex(x => x.Name.Equals(camera.Name)) != -1)
                {
                    response = new Response<string>(Enums.Status.Error, $"Камера c именем {camera.Name} уже существует");
                    return response;
                };
                _cameras.Add(camera);
                response = new Response<string>(Enums.Status.Ok, "");
            }
            catch(Exception ex)
            { 
                response = new Response<string>(Enums.Status.Error, ex.Message);
            }
            return response;
        }

        public async Task<Response<string>> AddCameraFromApi()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string loginUrl = $"https://{Settings.Default.api}/{Settings.Default.loginApi}";
                    await client.LoginAsync(
                        loginUrl,
                        new
                        {
                            Login = Settings.Default.login,
                            Password = Settings.Default.password,
                        });


                    string uri = $"https://{Settings.Default.api}/{Settings.Default.cameraListApi}{Settings.Default.wellboreId}";
                    var res = await client.GetAsync(uri);
                    string content = await res.Content.ReadAsStringAsync();
                    List<Camera> result = JsonSerializer.Deserialize<List<Camera>>(content);

                    foreach (Camera camera in result)
                    {
                        AddCamera(camera);
                    }
                }
            }
            catch (Exception ex) 
            {
                return new Response<string>(Enums.Status.Error, ex.Message);
            }
            return new Response<string>(Enums.Status.Ok, "");
        }
    }
}
