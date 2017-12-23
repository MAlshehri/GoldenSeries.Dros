using System.Windows.Input;

using GoldenSeries.Dros.Helpers;

namespace GoldenSeries.Dros.ViewModels
{
    public class AboutViewModel : OldBaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Plugin.Share.CrossShare.Current.OpenBrowser("https://xamarin.com/platform"));
        }

        public ICommand OpenWebCommand { get; }
    }
}