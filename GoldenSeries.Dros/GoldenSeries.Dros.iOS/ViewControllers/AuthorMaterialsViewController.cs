using Foundation;
using System;
using UIKit;
using GoldenSeries.Dros.Models;
using System.Collections.Generic;
using GoldenSeries.Dros.ViewModels;
using System.Collections.Specialized;

namespace GoldenSeries.Dros.iOS
{
    public partial class AuthorMaterialsViewController : UITableViewController
    {
        public AuthorMaterialsViewModel ViewModel { get; set; }
        //UIRefreshControl refreshControl;

        public AuthorMaterialsViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //refreshControl = new UIRefreshControl();
            //refreshControl.ValueChanged += RefreshControl_ValueChanged;
            //TableView.Add(refreshControl);
            ViewModel.LoadMaterialsCommand.Execute(null);
            TableView.RowHeight = UITableView.AutomaticDimension;
            TableView.EstimatedRowHeight = 60;
            TableView.Source = new AuthorMaterialsDataSource(ViewModel);
            Title = ViewModel.Title;
            //ViewModel.PropertyChanged += IsBusy_PropertyChanged;
            //ViewModel.Materials.CollectionChanged += Items_CollectionChanged;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (ViewModel.Materials.Count == 0)
                ViewModel.LoadMaterialsCommand.Execute(null);
        }

        //void RefreshControl_ValueChanged(object sender, EventArgs e)
        //{
        //    if (!ViewModel.IsBusy && refreshControl.Refreshing)
        //        ViewModel.LoadMaterialsCommand.Execute(null);
        //}

        //void IsBusy_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    var propertyName = e.PropertyName;
        //    switch (propertyName)
        //    {
        //        case nameof(ViewModel.IsBusy):
        //            {
        //                InvokeOnMainThread(() =>
        //                {
        //                    if (ViewModel.IsBusy && !refreshControl.Refreshing)
        //                        refreshControl.BeginRefreshing();
        //                    else if (!ViewModel.IsBusy)
        //                        refreshControl.EndRefreshing();
        //                });
        //            }
        //            break;
        //    }
        //}

        void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InvokeOnMainThread(() => TableView.ReloadData());
        }
    }

    class AuthorMaterialsDataSource : UITableViewSource
    {
        static readonly NSString CELL_IDENTIFIER = new NSString("AuthorMaterialCell");
        readonly AuthorMaterialsViewModel viewModel;

        public AuthorMaterialsDataSource(AuthorMaterialsViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override nint RowsInSection(UITableView tableview, nint section) => viewModel.Materials.Count;
        public override nint NumberOfSections(UITableView tableView) => 1;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CELL_IDENTIFIER, indexPath);
            cell.TextLabel.SetContentHuggingPriority(100, UILayoutConstraintAxis.Vertical);
            var material = viewModel.Materials[indexPath.Row];
            cell.TextLabel.Text = material.Title;
            //if(material.Links != null && material.Links.Count > 0)
            //    cell.DetailTextLabel.Text = $"عدد الملفات: {material.Links.Count.ToString()}";
            //else
                //cell.DetailTextLabel.Text = $"عدد الملفات: لا يوجد";
            cell.LayoutMargins = UIEdgeInsets.Zero;

            return cell;
        }
    }
}