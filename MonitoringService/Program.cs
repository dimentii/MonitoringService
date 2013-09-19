using System.ServiceProcess;

namespace MonitoringService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new MonitoringService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
