using ConsoleApplication1.Interfaces;
using Ninject.Modules;
using RssBusinessLogic;
using RssBusinessLogic.Interfaces;

namespace ConsoleApplication1.NinjectModules
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