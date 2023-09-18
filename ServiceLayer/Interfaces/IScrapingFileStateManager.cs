using ServiceLayer.Models;
using System.Collections.Generic;

namespace ServiceLayer.Interfaces
{
    public interface IScrapingFileStateManager
    {
        public List<SourceScrapingFile> UpdateScrapingFilesState(List<SourceScrapingFile> collection, bool isChecked);
    }
}
