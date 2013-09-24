using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace WebsiteWorkers
{
    public interface IDbWorker
    {
        Task<String> GetLink(DbDataReader reader);
        String GetIdentifyingQuery();
    }
}
