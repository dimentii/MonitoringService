using System.Collections.Generic;
using System.Threading;
using ConsoleApplication1.NinjectModules;
using Ninject;
using RssBusinessLogic;
using RssBusinessLogic.NinjectModules;
using WebsiteWorkers;
using WebsiteWorkers.Workers;

namespace ConsoleApplication1
{
    class Program
    {
        private static readonly List<WebsiteInput> WebsiteInputs = new List<WebsiteInput>
            {
                new WebsiteInput("Kommersant", "http://kommersant.ru/rss/daily.xml", new KommersantRu(Identifier.Guid)),
                new WebsiteInput("Izvestia", "http://izvestia.ru/xml/rss/all.xml", new IzvestiaRu(Identifier.Link)),
                new WebsiteInput("Korrespondent", "http://k.img.com.ua/rss/ru/news.xml", new KorrespondentNet(Identifier.Link))
            };

        static void Main()
        {
            var kernel = new StandardKernel(new ServiceInjectionModule(), new BusinessLogicInjectionModule());
            var handler = kernel.Get<BusinessLogic>();

            var tasks = handler.BeginWork(WebsiteInputs);
            
            Thread.Sleep(500000);
        }
    }
}
