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

        public Response<string> CreateBat(string cameraName, string streamKey)
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
                input.Write($"echo \"Hello world! {cameraName} is on-line! Stream started on {streamKey}\" \n pause >nul");
                input.Close();
                stream.Close();
                Response<string> resp = processDictionaryService.AddProcess(cameraName, filepath.Data);
                if (resp.Status == Core.Enums.Status.Ok)
                { 
                    return new Response<string>(Core.Enums.Status.Ok, ""); 
                }
                else
                {
                    return new Response<string>(Core.Enums.Status.Error, resp.Message);
                }
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

                Response<string> r = processDictionaryService.StartProcess(cameraName);
                if(r.Status == Core.Enums.Status.Error)
                {
                    return new Response<Process>(Core.Enums.Status.Error, r.Message);
                }
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
