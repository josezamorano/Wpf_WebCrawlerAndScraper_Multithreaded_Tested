using System.Collections.Generic;

namespace DomainLayer.Utils.Interfaces
{
    public interface IHtmlParser
    {
        List<string> GetLinks(string htmlContent);
    }
}
