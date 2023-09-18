namespace ServiceLayer.Models
{
    public class ScrapeInputValidationReport
    {
        public string ScrapingFolderPathReport { get; set; }

        public string AlltxtFilesReport { get; set; }

        public string XPathExpressionReport { get; set; }

        public bool AllScrapeInputsAreValid { get; set; }
    }
}
