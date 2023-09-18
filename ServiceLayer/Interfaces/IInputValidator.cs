using ServiceLayer.Models;

namespace ServiceLayer.Interfaces
{
    public interface IInputValidator
    {
        CrawlInputValidationReport ValidateCrawlInputs(InputsForValidation inputsForValidation);

        ScrapeInputValidationReport ValidateScrapeInputs(InputsForValidation inputsForValidation);
    }
}
