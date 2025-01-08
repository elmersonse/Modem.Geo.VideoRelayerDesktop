using Modem.Geo.VideoRelayerDesktop.Core.Classes;
using Modem.Geo.VideoRelayerDesktop.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Service
{
    internal class BatExecutionService
    {
        private static BatExecutionService instance;
        private CameraCollection _collection = CameraCollection.GetInstance();
        private ProcessDictionaryService processDictionaryService = ProcessDictionaryService.GetInstanse();

        private BatExecutionService() { }

        public static BatExecutionService GetInstance()
        {
            if (instance == null)
            {
                instance = new BatExecutionService();
            }
            return instance;
        }

        public Response<byte> CreateBat(Camera camera)
        {
            try
            { 
                Process process = FFmpegHelper.CreateRelayProcessForRTSPInputRecode(camera.Id, camera.InputUrl, camera.OutputUrl);
                processDictionaryService.AddProcess(camera.Name, process);
                return new Response<byte>(Core.Enums.Status.Ok, "");
            }
            catch (Exception ex) 
            {
                return new Response<byte>(Core.Enums.Status.Error, ex.Message);
            }
        }

        public Response<byte> ExecuteBat(string cameraName)
        {
            try
            {
                Camera cam = _collection.GetCameraCollection().FirstOrDefault<Camera>(x => x.Name == cameraName);
                Response<string> filepath = CombineBatPath(cam.Id.ToString());
                if (cam == null)
                {
                    return new Response<byte>(Core.Enums.Status.Error, "Указанной камеры не существует");
                }
                if (filepath.Status == Core.Enums.Status.Error)
                {
                    return new Response<byte>(Core.Enums.Status.Error, filepath.Message);
                }

                Response<string> r = processDictionaryService.StartProcess(cameraName);
                if(r.Status == Core.Enums.Status.Error)
                {
                    return new Response<byte>(Core.Enums.Status.Error, r.Message);
                }
            }
            catch(Exception ex)
            {
                return new Response<byte>(Core.Enums.Status.Error, ex.Message);
            }
            return new Response<byte>(Core.Enums.Status.Ok, "");
        }

        public Response<string> CombineBatPath(string cameraName)
        {
            try
            {
                string batPath = Properties.Settings.Default.batPath;
                
                if (!Directory.Exists(batPath))
                {
                    return new Response<string>(Core.Enums.Status.Error, "Указанной директории не существует");
                }
                string filename = $"{batPath}\\{cameraName}.bat";
                return new Response<string>(Core.Enums.Status.Ok, "", filename);
            }
            catch(Exception ex)
            {
                return new Response<string>(Core.Enums.Status.Error, ex.Message);
            }
        }
    }
}
