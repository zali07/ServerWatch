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

            string sharedTempFolder = @"C:\Windows\Temp";

            if (!Directory.Exists(sharedTempFolder))
            {
                Directory.CreateDirectory(sharedTempFolder);
            }

            string sharedTempFile = Path.Combine(sharedTempFolder, "driveDataOutput.json");

            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\" -OutputFilePath \"{sharedTempFile}\"",
                UseShellExecute = true,
                Verb = "runas",
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }

            string output = File.ReadAllText(sharedTempFile);

            //File.Delete(sharedTempFile);

            JToken driversData;

            try
            {
                driversData = JToken.Parse(output);
            }
            catch
            {
                throw new Exception($"Unexpected JSON format from PowerShell script.\nOutput: {output}\n");
            }

            return JsonConvert.SerializeObject(driversData, Formatting.Indented);
        }
    }
}
