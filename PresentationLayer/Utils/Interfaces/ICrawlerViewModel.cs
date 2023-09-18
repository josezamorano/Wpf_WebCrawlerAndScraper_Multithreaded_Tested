using System.Windows.Input;

namespace PresentationLayer.Utils.Interfaces
{
    public interface ICrawlerViewModel
    {
        public string CrawlerViewVisibility { get; set; }

        public ICommand GoBackButtonCommand { get; set; }
    }
}
