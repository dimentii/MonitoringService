using System.Collections.Generic;
using System.Threading.Tasks;
using RssBusinessLogic;

namespace MonitoringService.Interfaces
{
    public interface IRssHandler
    {
        Task<List<ReportData>> HandleRssAsync();
    }
}