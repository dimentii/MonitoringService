using Ninject.Modules;
using RssBusinessLogic.Interfaces;
using RssDataAccessLayer;
using RssDataAccessLayer.Interfaces;

namespace RssBusinessLogic.NinjectModules
{
    public class BusinessLogicInjectionModule: NinjectModule
    {
        public override void Load()
        {
            Bind<IDatabaseWorker>().To<DatabaseWorker>();
            Bind<IWebsiteLoader>().To<WebsiteLoader>();
            Bind<IWebsiteParser>().To<WebsiteParser>();
            Bind<IDataAccessLayer>().To<DataAccessLayer>();
        }
    }
}
