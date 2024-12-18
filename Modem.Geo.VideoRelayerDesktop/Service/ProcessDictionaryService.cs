using Modem.Geo.VideoRelayerDesktop.Core.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modem.Geo.VideoRelayerDesktop.Service
{
    internal class ProcessDictionaryService
    {
        private static ProcessDictionaryService instanse;
        private Dictionary<string, Process> processDictionary;

        private ProcessDictionaryService()
        {
            processDictionary = new Dictionary<string, Process>();
        }

        public static ProcessDictionaryService GetInstanse()
        {
            if (instanse == null)
            {
                instanse = new ProcessDictionaryService();
            }
            return instanse;
        }

        public Response<string> AddProcess(string cameraName, string filepath)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = filepath;
                p.StartInfo.UseShellExecute = true;
                if (!processDictionary.ContainsKey(cameraName))
                {
                    processDictionary.Add(cameraName, p);
                }
                return new Response<string>(Core.Enums.Status.Ok, "");
            }
            catch (Exception ex)
            {
                return new Response<string>(Core.Enums.Status.Error, ex.Message);
            }
        }

        public Response<string> StartProcess(string key)
        {
            try
            {
                if (!processDictionary.ContainsKey(key))
                {
                    return new Response<string>(Core.Enums.Status.Error, "Процесса не существует");
                }
                if (IsRunning(key).Data)
                {
                    return new Response<string>(Core.Enums.Status.Error, "Процесс уже запущен");
                }
                processDictionary[key].Start();
                return new Response<string>(Core.Enums.Status.Ok, "");
            }
            catch (Exception ex)
            {
                return new Response<string>(Core.Enums.Status.Error, ex.Message);
            }
        }

        public Response<bool> IsRunning(string key)
        {
            try
            {
                return new Response<bool>(Core.Enums.Status.Ok, "", !processDictionary[key].HasExited);
            }
            catch (Exception)
            {
                return new Response<bool>(Core.Enums.Status.Error, "", false);
            }
        }

        public void CoseAll()
        {
            foreach (string key in processDictionary.Keys)
            {
                if (processDictionary[key] != null && IsRunning(key).Data)
                {
                    processDictionary[key].Close();
                }
            }

        }
    }
}
