using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RssDataAccessLayer
{
    public interface IRssDAL
    {
        Task<List<String>> FillRss(String tableName, String xml);
    }
}
