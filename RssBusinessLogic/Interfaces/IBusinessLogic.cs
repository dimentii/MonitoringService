using System;
using System.Threading.Tasks;
using WebsiteWorkers;

namespace RssBusinessLogic.Interfaces
{
    public interface IBusinessLogic
    {
        Task<ReportData> ProcessData(String table, String xml, IWorker worker);
    }
}