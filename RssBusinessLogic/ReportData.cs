﻿using System;

namespace RssBusinessLogic
{
    public class ReportData
    {
        public ReportData(Int32 addedArticles, String newspaper)
        {
            AddedArticles = addedArticles;
            Newspaper = newspaper;
        }
        public Int32 AddedArticles { get; set; }
        public String Newspaper { get; set; }

        public override String ToString()
        {
            return String.Format("Newspaper {0} produced {1} news", Newspaper, AddedArticles);
        }
    }
}
