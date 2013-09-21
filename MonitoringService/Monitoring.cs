using System;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using MonitoringService.Interfaces;

namespace MonitoringService
{
    public partial class MonitoringService : ServiceBase
    {
        private Timer _timer;
        private readonly IRssHandler _handler;

        public MonitoringService()
        {
            InitializeComponent();
        }

        public MonitoringService(IRssHandler rssHandler)
        {
            _handler = rssHandler;
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
            _timer.Interval = 3600000D;
            var articles = await _handler.HandleRssAsync();
            foreach (var article in articles)
            {
                var path =
                    String.Format(
                        @"C:\Users\Dydewki\Documents\Visual Studio 2012\Projects\Service\MonitoringService\Success{0}.txt",
                        article.Newspaper);
                File.AppendAllText(path, String.Format("{0} were collected", article.AddedArticles));
            }
        }
    }
}