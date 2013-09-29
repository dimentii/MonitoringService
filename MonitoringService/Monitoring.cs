using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using RssBusinessLogic;
using RssBusinessLogic.Interfaces;
using WebsiteWorkers;
using WebsiteWorkers.Workers;

namespace MonitoringService
{
    public partial class MonitoringService : ServiceBase
    {
        private Timer _timer;
        private readonly IBusinessLogic _businessLogic;
        private readonly List<WebsiteInput> _websiteInputs = new List<WebsiteInput>
            {
                new WebsiteInput("Kommersant", "http://kommersant.ru/rss/daily.xml", new KommersantRu(Identifier.Guid)),
                new WebsiteInput("Izvestia", "http://izvestia.ru/xml/rss/all.xml", new IzvestiaRu(Identifier.Link)),
                new WebsiteInput("Korrespondent", "http://k.img.com.ua/rss/ru/news.xml", new KorrespondentNet(Identifier.Link))
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
            _timer = new Timer(30000D) {AutoReset = true};
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