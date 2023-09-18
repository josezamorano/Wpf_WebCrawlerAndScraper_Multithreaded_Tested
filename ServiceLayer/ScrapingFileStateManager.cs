using ServiceLayer.Interfaces;
using ServiceLayer.Models;
using System.Collections.Generic;

namespace ServiceLayer
{
    public class ScrapingFileStateManager : IScrapingFileStateManager
    {

        public List<SourceScrapingFile> UpdateScrapingFilesState(List<SourceScrapingFile> collection, bool isChecked)
        {
            var newList = new List<SourceScrapingFile>();
            if (collection != null)
            {                
                foreach (SourceScrapingFile file in collection)
                {
                    file.IsSelected = isChecked;
                    newList.Add(file);
                }                
            }
            return newList;
        }
    }
}
