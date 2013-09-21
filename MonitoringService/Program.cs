using System.ServiceProcess;
using MonitoringService.NinjectModules;
using Ninject;
using RssBusinessLogic.NinjectModules;

namespace MonitoringService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var kernel = new StandardKernel(new ServiceInjectionModule(), new BusinessLogicInjectionModule());
            var handler = kernel.Get<RssHandler>();
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new MonitoringService(handler) 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
