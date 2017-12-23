using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using GoldenSeries.Dros.Models;
using GoldenSeries.Dros.Helpers;
namespace GoldenSeries.Dros.ViewModels
{
    public class AuthorsViewModel : BaseViewModel
    {
        public ObservableCollection<Author> Authors { get; set; }
        public Command LoadAuthorsCommand { get; set; }
        //public Command AddItemCommand { get; set; }

        public AuthorsViewModel()
        {
            Title = "قائمة الاسماء";
            Authors = new ObservableCollection<Author>();
            LoadAuthorsCommand = new Command(async () => await ExecuteLoadAuthorsCommand());
            //AddItemCommand = new Command<Item>(async (Item item) => await AddItem(item));
        }

        async Task ExecuteLoadAuthorsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Authors.Clear();
                var authors = await dataStore.GetAuthors();
                foreach (var author in authors)
                {
                    Authors.Add(author);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}