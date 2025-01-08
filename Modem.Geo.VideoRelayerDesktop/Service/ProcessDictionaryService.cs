using Modem.Geo.VideoRelayerDesktop.Core.Classes;
using Modem.Geo.VideoRelayerDesktop.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public Response<byte> AddProcess(string key, Process process)
        {
            try
            {
                processDictionary.Add(key, process);
                return new Response<byte>(Core.Enums.Status.Ok, "");
            }
            catch (Exception ex)
            {
                return new Response<byte>(Core.Enums.Status.Error, ex.Message);
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
                Process p = processDictionary[key];
                p.StartInfo.CreateNoWindow = false;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                
                p.Start();
                
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

        public void StopProcess(string key)
        {
            if(IsRunning(key).Data)
            {
                using (StreamWriter streamWriter = processDictionary[key].StandardInput)
                {
                    streamWriter.WriteLine('q');
                }
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
