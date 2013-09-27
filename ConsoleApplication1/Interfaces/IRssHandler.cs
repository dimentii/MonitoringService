using System.Collections.Generic;
using System.Threading.Tasks;
using RssBusinessLogic;

namespace ConsoleApplication1.Interfaces
{
    public interface IRssHandler
    {
        Task HandleRssAsync();
    }
}