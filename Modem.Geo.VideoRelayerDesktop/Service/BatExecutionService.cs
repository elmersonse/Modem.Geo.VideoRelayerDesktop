using Modem.Geo.VideoRelayerDesktop.Core.Classes;
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

        private BatExecutionService() { }

        public static BatExecutionService GetInstance()
        {
            if (instance == null)
            {
                instance = new BatExecutionService();
            }
            return instance;
        }

        public Response<string> CreateBat(string cameraName)
        {
            try
            {
                
                Response<string> filepath = CombineBatPath(cameraName);
                if(filepath.Status == Core.Enums.Status.Error)
                {
                    return new Response<string>(Core.Enums.Status.Error, filepath.Message);
                }
                FileInfo batFile = new FileInfo(filepath.Data);

                FileStream stream;
                if (!batFile.Exists)
                {
                    stream = batFile.Create();
                }
                else
                {
                    stream = batFile.OpenWrite();
                }
                StreamWriter input = new StreamWriter(stream);
                input.Write($"Hello world! {cameraName} is on-line! \n pause >nul");
                input.Close();
                stream.Close();
                return new Response<string>(Core.Enums.Status.Ok, "");
            }
            catch (Exception ex) 
            {
                return new Response<string>(Core.Enums.Status.Error, ex.Message);
            }
        }

        public Response<Process> ExecuteBat(string cameraName)
        {
            try
            {
                Response<string> filepath = CombineBatPath(cameraName);
                if (filepath.Status == Core.Enums.Status.Error)
                {
                    return new Response<Process>(Core.Enums.Status.Error, filepath.Message);
                }

                Process bat = new Process();
                bat.StartInfo.FileName = filepath.Data;
                bat.StartInfo.UseShellExecute = true;
                bat.Start();
            }
            catch(Exception ex)
            {
                return new Response<Process>(Core.Enums.Status.Error, ex.Message);
            }
            return new Response<Process>(Core.Enums.Status.Ok, "");
        }

        public Response<string> CombineBatPath(string cameraName)
        {
            try
            {
                string batPath = ConfigurationManager.AppSettings.Get("batPath");
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
