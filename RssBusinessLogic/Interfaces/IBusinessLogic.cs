using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssBusinessLogic.Interfaces
{
    public interface IBusinessLogic
    {
        Task BeginWork(List<WebsiteInput> inputs);
    }
}