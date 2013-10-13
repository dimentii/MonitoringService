using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using RssBusinessLogic;
using RssBusinessLogic.Interfaces;
using WebsiteWorkers.Workers;

namespace MonitoringService
{
    public partial class MonitoringService : ServiceBase
    {
        private Timer _timer;
        private readonly IBusinessLogic _businessLogic;
        private readonly List<WebsiteInput> _websiteInputs = new List<WebsiteInput>
            {
                new WebsiteInput("Kommersant", "http://kommersant.ru/rss/daily.xml", new KommersantRu()),
                new WebsiteInput("Izvestia", "http://izvestia.ru/xml/rss/all.xml", new IzvestiaRu()),
                new WebsiteInput("Korrespondent", "http://k.img.com.ua/rss/ru/news.xml", new KorrespondentNet()),
                new WebsiteInput("Interfax", "http://www.interfax.ru/rss.asp", new InterfaxRu()),
                new WebsiteInput("Regnum", "http://www.regnum.ru/rss/index.xml", new RegnumRu()),
                new WebsiteInput("AnnewsRu", "http://annews.ru/rss.php", new AnnewsRu())
            };

        public MonitoringService()
        {
            InitializeComponent();
        }

        public MonitoringService(IBusinessLogic businessLogic)
        {
            _businessLogic = businessLogic;
            InitializeComponent();
        }
        
        protected override void OnStart(String[] args)
        {
            _timer = new Timer(60000D) {AutoReset = true};
            _timer.Elapsed += CollectArticles;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer = null;
        }

        private async void CollectArticles(Object sender, ElapsedEventArgs e)
        {
            _timer.Interval = 7200000D;
            await _businessLogic.BeginWork(_websiteInputs);
        }
    }
}