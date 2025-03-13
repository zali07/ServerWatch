using System.Diagnostics;
using System.Runtime.Versioning;

namespace EventLogSourceCreator
{
    class Program
    {
        [SupportedOSPlatform("windows")]
        static void Main(string[] args)
        {
            const string sourceName = "CosysInternalException";
            const string logName = "Application";

            try
            {
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, logName);
                    Console.WriteLine($"Event source '{sourceName}' created successfully in log '{logName}'.");
                }
                else
                {
                    Console.WriteLine($"Event source '{sourceName}' already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating event source: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
