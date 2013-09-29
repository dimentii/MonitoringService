using System;

namespace WebsiteWorkers
{
    public interface IDbWorker
    {
        Identifier Identifier { get; set; }
        String GetIdentifyingQuery();
    }
}
