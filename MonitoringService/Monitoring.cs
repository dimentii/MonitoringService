using System;
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
            _timer.Interval = 7200000D;
            await _handler.HandleRssAsync();
        }
    }
}