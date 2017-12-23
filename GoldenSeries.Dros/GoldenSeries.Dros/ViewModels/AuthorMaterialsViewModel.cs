using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using GoldenSeries.Dros.Models;
using GoldenSeries.Dros.Helpers;
namespace GoldenSeries.Dros.ViewModels
{
    public class AuthorMaterialsViewModel : BaseViewModel
    {
        public ObservableCollection<Material> Materials { get; set; }
        public Command LoadMaterialsCommand { get; set; }
        public Guid AuthorId { get; set; }

        public AuthorMaterialsViewModel()
        {
            Title = "Material";
            Materials = new ObservableCollection<Material>();
            LoadMaterialsCommand = new Command(async () => await ExecuteLoadMaterialsCommand());
        }

        async Task ExecuteLoadMaterialsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Materials.Clear();
                var materials = await dataStore.GetAuthorMaterials(AuthorId);
                foreach (var material in materials)
                {
                    Materials.Add(material);
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