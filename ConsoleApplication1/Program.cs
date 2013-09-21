using System.Threading;
using ConsoleApplication1.NinjectModules;
using Ninject;
using RssBusinessLogic.NinjectModules;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main()
        {
            var kernel = new StandardKernel(new ServiceInjectionModule(), new BusinessLogicInjectionModule());
            var handler = kernel.Get<RssHandler>();

            var tasks = handler.HandleRssAsync();
            
            Thread.Sleep(500000);
        }
    }
}
