using System;

using GoldenSeries.Dros.Models;

namespace GoldenSeries.Dros.ViewModels
{
    public class ItemDetailViewModel : OldBaseViewModel
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            if (item != null)
            {
                Title = item.Text;
                Item = item;
            }
        }
    }
}
