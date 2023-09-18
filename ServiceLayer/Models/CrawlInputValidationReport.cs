namespace ServiceLayer.Models
{
    public class CrawlInputValidationReport
    {
        public string UrlLabelReport { get; set; }

        public string OptionsLabelReport { get; set; }

        public string PagesCountLabelReport { get; set; }

        public string SelectedFolderReport { get; set; }

        public bool AllCrawlInputsAreValid { get; set; }
    }
}
