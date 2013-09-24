using System.ComponentModel;

namespace WebsiteWorkers
{
    public enum Identifier
    {
        [Description("[Link] = Item.value('link[1]', 'nvarchar(200)')")]
        Link = 1,
        [Description("[Guid_link] = Item.value('guid[1]', 'nvarchar(200)')")]
        Guid = 2
    }
}
