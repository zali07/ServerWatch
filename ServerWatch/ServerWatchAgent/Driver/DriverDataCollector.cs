using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;

namespace ServerWatchAgent.Driver
{
    public class DriverDataCollector
    {
        public static string CheckDriversOnServer()
        {
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Driver", "getDriveData.ps1");

            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException("PowerShell script not found at: " + scriptPath);
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(error))
                {
                    throw new Exception("PowerShell error: " + error);
                }

                var token = JToken.Parse(output);

                if (token is JArray array)
                {
                    foreach (JObject drive in array)
                    {
                        drive["ServerName"] = Environment.MachineName;
                    }

                    return JsonConvert.SerializeObject(array, Formatting.Indented);
                }
                else if (token is JObject obj)
                {
                    obj["ServerName"] = Environment.MachineName;
                    return JsonConvert.SerializeObject(obj, Formatting.Indented);
                }
                else
                {
                    throw new Exception("Unexpected JSON format from PowerShell script.");
                }
            }
        }
    }
}
