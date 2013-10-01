using System;

namespace WebsiteWorkers
{
    public class CompleteArticleData
    {
        public String Article { get; set; }
        public String Author { get; set; }
        public String Description { get; set; }
        public String Link { get; set; }
        public String Title { get; set; }

        public override String ToString()
        {
            return String.Format("(N'{0}', N'{1}', N'{2}', N'{3}', N'{4}'),", Link, Author, Title, Description, Article);
        }
    }
}