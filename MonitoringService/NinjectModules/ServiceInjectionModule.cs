using MonitoringService.Interfaces;
using Ninject.Modules;
using RssBusinessLogic;
using RssBusinessLogic.Interfaces;

namespace MonitoringService.NinjectModules
{
    class ServiceInjectionModule: NinjectModule
    {
        public override void Load()
        {
            Bind<IRssHandler>().To<RssHandler>();
            Bind<IBusinessLogic>().To<BusinessLogic>();
        }
    }
}