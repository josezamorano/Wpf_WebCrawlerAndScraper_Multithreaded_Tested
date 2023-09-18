using System.Collections.Generic;

namespace ServiceLayer.Models
{
    public class InputsForValidation
    {
        //Crawl Inputs
        public string UrlText { get; set; }
        public string TotalPagesForSearchText { get; set; }

        public bool RadioBtnSearchAllPagesChecked { get; set; }

        public bool RadioBtnSearchCountChecked { get; set; }

        public string ReportFolder { get; set; }

        //Scrape Inputs

        public string ScrapingFolderPath { get; set; }

        public Queue<string> AllCheckedTxtFiles { get; set; }

        public string XPathExpression { get; set; }
    }
}
