using System;

using Foundation;
using UIKit;

namespace GoldenSeries.Dros.iOS
{
    public partial class AuthorCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("AuthorCell");
        public static readonly UINib Nib;

        static AuthorCell()
        {
            Nib = UINib.FromName("AuthorCell", NSBundle.MainBundle);
        }

        protected AuthorCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
    }
}
