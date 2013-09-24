using System;
using System.Threading.Tasks;
using WebsiteWorkers;

namespace RssBusinessLogic.Interfaces
{
    public interface IBusinessLogic
    {
        Task<ReportData> ProcessData<TWorker>(String table, String xml, TWorker worker)
            where TWorker : IDbWorker, IWebWorker;
    }
}