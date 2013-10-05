using System;

namespace WebsiteWorkers
{
    public interface IDbWorker
    {
        Unique Identifier { get; }
        String GetIdentifyingQuery();
    }
}
