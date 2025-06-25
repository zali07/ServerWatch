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

            try
            {
                var parsed = JToken.Parse(output);

                JArray arrayOutput;

                if (parsed.Type == JTokenType.Array)
                {
                    arrayOutput = (JArray)parsed;
                }
                else if (parsed.Type == JTokenType.Object)
                {
                    arrayOutput = new JArray(parsed);
                }
                else
                {
                    throw new Exception("Unexpected JSON structure: expected object or array.");
                }

                return JsonConvert.SerializeObject(arrayOutput, Formatting.Indented);
            }
            catch
            {
                throw new Exception($"Unexpected JSON format from PowerShell script.\nOutput: {output}\n");
            }
        }
    }
}
