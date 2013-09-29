using Ninject.Modules;
using RssBusinessLogic;
using RssBusinessLogic.Interfaces;

namespace MonitoringService.NinjectModules
{
    class ServiceInjectionModule: NinjectModule
    {
        public override void Load()
        {
            Bind<IBusinessLogic>().To<BusinessLogic>();
        }
    }
}