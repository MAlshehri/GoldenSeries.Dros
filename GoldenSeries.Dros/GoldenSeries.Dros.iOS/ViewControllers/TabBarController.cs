using System;
using UIKit;

namespace GoldenSeries.Dros.iOS
{
    public partial class TabBarController : UITabBarController
    {
        public TabBarController(IntPtr handle) : base(handle)
        {
            TabBar.Items[0].Title = "الاسماء";
            //TabBar.Items[1].Title = "About";
        }
    }
}
