using System.Collections.Generic;
using System.Threading;
using ConsoleApplication1.NinjectModules;
using Ninject;
using RssBusinessLogic;
using RssBusinessLogic.NinjectModules;
using WebsiteWorkers.Workers;

namespace ConsoleApplication1
{
    class Program
    {
        private static readonly List<WebsiteInput> WebsiteInputs = new List<WebsiteInput>
            {
                new WebsiteInput("Kommersant", "http://kommersant.ru/rss/daily.xml", new KommersantRu()),
                new WebsiteInput("Izvestia", "http://izvestia.ru/xml/rss/all.xml", new IzvestiaRu()),
                new WebsiteInput("Korrespondent", "http://k.img.com.ua/rss/ru/news.xml", new KorrespondentNet()),
                new WebsiteInput("Interfax", "http://www.interfax.ru/rss.asp", new InterfaxRu())
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
