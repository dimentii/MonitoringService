using Ninject.Modules;
using RssBusinessLogic;
using RssBusinessLogic.Interfaces;

namespace ConsoleApplication1.NinjectModules
{
    class ServiceInjectionModule: NinjectModule
    {
        public override void Load()
        {
            Bind<IBusinessLogic>().To<BusinessLogic>();
        }
    }
}