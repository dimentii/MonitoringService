using System;
using System.Threading.Tasks;

namespace ConsoleApplication1.Interfaces
{
    public interface IRssReader
    {
        Task<String> GetXmlStringsFromRssAsync(String feedUriString);
    }
}
