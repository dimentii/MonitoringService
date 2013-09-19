using System;
using System.Threading.Tasks;

namespace RssBusinessLogic.Interfaces
{
    public interface IRssWriter
    {
        Task HandleRss(String table, String xml);
    }
}
