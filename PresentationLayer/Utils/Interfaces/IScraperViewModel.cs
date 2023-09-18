using System.Windows.Input;

namespace PresentationLayer.Utils.Interfaces
{
    public interface IScraperViewModel
    {
        public string ScraperViewVisibility { get; set; }
        public ICommand GoBackButtonCommand { get; set; }
    }
}
