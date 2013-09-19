using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main()
        {
            var rssHandler = new RssHandler();
            var tasks = rssHandler.HandleRssAsync();
            
            Thread.Sleep(500000);
        }
    }
}
