using Foundation;
using System;
using System.Collections.Specialized;
using UIKit;

using GoldenSeries.Dros.ViewModels;
using GoldenSeries.Dros.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace GoldenSeries.Dros.iOS
{
    public partial class AuthorsViewController : UITableViewController
    {
        //UIRefreshControl refreshControl;

        public AuthorsViewModel ViewModel { get; set; }
        Dictionary<string, List<Author>> indexedTableAuthors = new Dictionary<string, List<Author>>();
        string[] keys;

        ResultsTableController resultsTableController;
        UISearchController searchController;
        bool searchControllerWasActive;
        bool searchControllerSearchFieldWasFirstResponder;

        public AuthorsViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = new AuthorsViewModel();
            ViewModel.LoadAuthorsCommand.Execute(null);
            // Setup UITableView.
            //refreshControl = new UIRefreshControl();
            //refreshControl.ValueChanged += RefreshControl_ValueChanged;
            //TableView.Add(refreshControl);
            var regex = new Regex("^[a-z]");
            foreach (var author in ViewModel.Authors)
            {
                var firstLetter = author.Name[0].ToString();
                if (regex.IsMatch(firstLetter.ToLower())) continue;
                if (indexedTableAuthors.ContainsKey(firstLetter))
                {
                    indexedTableAuthors[firstLetter].Add(author);
                }
                else
                {
                    if(firstLetter == "أ" || firstLetter == "إ")
                    {
                        if(indexedTableAuthors.ContainsKey("ا"))
                            indexedTableAuthors["ا"].Add(author);
                        else
                            indexedTableAuthors.Add("ا", new List<Author>() { author });
                    }
                    else
                    {
                        indexedTableAuthors.Add(firstLetter, new List<Author>() { author });
                    }
                }
            }
            keys = indexedTableAuthors.Keys.ToArray();

            TableView.Source = new AuthorsDataSource(indexedTableAuthors, keys);


            //Search bar
            resultsTableController = new ResultsTableController
            {
                Authors = new List<Author>()
            };

            searchController = new UISearchController(resultsTableController)
            {
                WeakDelegate = this,
                DimsBackgroundDuringPresentation = false,
                WeakSearchResultsUpdater = this
            };

            searchController.SearchBar.SizeToFit();
            searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
            searchController.SearchBar.Placeholder = "ابحث بالاسم";
            searchController.SearchBar.ShowsCancelButton = false;
            TableView.TableHeaderView = searchController.SearchBar;

            resultsTableController.TableView.WeakDelegate = this;
            searchController.SearchBar.WeakDelegate = this;

            DefinesPresentationContext = true;

            if (searchControllerWasActive)
            {
                searchController.Active = searchControllerWasActive;
                searchControllerWasActive = false;

                if (searchControllerSearchFieldWasFirstResponder)
                {
                    searchController.SearchBar.BecomeFirstResponder();
                    searchControllerSearchFieldWasFirstResponder = false;
                }
            }



            // Title
            Title = ViewModel.Title;

            //ViewModel.PropertyChanged += IsBusy_PropertyChanged;
            //ViewModel.Authors.CollectionChanged += Items_CollectionChanged;
        }


        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (ViewModel.Authors.Count == 0)
                ViewModel.LoadAuthorsCommand.Execute(null);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if (segue.Identifier == "NavigateToAuthorsMaterial")
            {
                var controller = segue.DestinationViewController as AuthorMaterialsViewController;
                var indexPath = TableView.IndexPathForCell(sender as UITableViewCell);
                var author = indexedTableAuthors[keys[indexPath.Section]][indexPath.Row];
                controller.ViewModel = new AuthorMaterialsViewModel { AuthorId = author.Id };
            }
        }

        //void RefreshControl_ValueChanged(object sender, EventArgs e)
        //{
        //    if (!ViewModel.IsBusy && refreshControl.Refreshing)
        //        ViewModel.LoadAuthorsCommand.Execute(null);
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

        [Export("searchBarSearchButtonClicked:")]
        public virtual void SearchButtonClicked(UISearchBar searchBar)
        {
            searchBar.ResignFirstResponder();
        }

        [Export("updateSearchResultsForSearchController:")]
        public virtual void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            var tableController = (ResultsTableController)searchController.SearchResultsController;
            tableController.Authors = PerformSearch(searchController.SearchBar.Text);
            tableController.TableView.ReloadData();
        }

        List<Author> PerformSearch(string searchString)
        {
            searchString = searchString.Trim();

            var filteredAuthors = new List<Author>();
            foreach (var key in indexedTableAuthors.Keys)
            {
                foreach (var author in indexedTableAuthors[key])
                {
                    if (author.Name.Contains(searchString))
                    {
                        filteredAuthors.Add(author);
                    }
                }
            }
            return filteredAuthors.ToList();
        }
    }

    class AuthorsDataSource : UITableViewSource
    {
        public static readonly NSString CELL_IDENTIFIER = new NSString("AuthorCell");
        string[] keys;

        Dictionary<string, List<Author>> indexedTableAuthors;

        public AuthorsDataSource(Dictionary<string, List<Author>> indexedTableAuthors, string[] keys)
        {
            this.indexedTableAuthors = indexedTableAuthors;
            this.keys = keys;
        }
        public override nint NumberOfSections(UITableView tableView)
        {
            return keys.Length;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return keys[section];
        }
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return indexedTableAuthors[keys[section]].Count;
        }

        public override String[] SectionIndexTitles(UITableView tableView)
        {
            return indexedTableAuthors.Keys.OrderBy(x => x).ToArray();
        }
        //public override nint RowsInSection(UITableView tableview, nint section) => viewModel.Authors.Count;
        //public override nint NumberOfSections(UITableView tableView) => 1;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CELL_IDENTIFIER, indexPath);

            var author = indexedTableAuthors[keys[indexPath.Section]][indexPath.Row];
            cell.TextLabel.Text = author.Name;
            //cell.DetailTextLabel.Text = item.Id.ToString();
            cell.LayoutMargins = UIEdgeInsets.Zero;

            return cell;
        }
    }

    class ResultsTableController : UITableViewController
    {
        public List<Author> Authors { get; set; }
        static readonly NSString CELL_IDENTIFIER = new NSString("AuthorCell2");
        public ResultsTableController()
        {

        }

        public override void ViewDidLoad()
        {
            TableView.RegisterNibForCellReuse(AuthorCell.Nib, "AuthorCell2");
            base.ViewDidLoad();
        }

        public ResultsTableController(IntPtr handle) : base(handle)
        {

        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return Authors.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            Author author = Authors[indexPath.Row];
            UITableViewCell cell = tableView.DequeueReusableCell(CELL_IDENTIFIER);
            cell.TextLabel.Text = author.Name;
            cell.LayoutMargins = UIEdgeInsets.Zero;
            return cell;
        }
    }
}
