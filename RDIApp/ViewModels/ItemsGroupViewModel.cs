using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.ObjectModel;

namespace RDIApp.ViewModels
{

    public class ItemsListViewModel : ObservableCollection<ItemViewModel>
    {

        public ItemsListViewModel(IEnumerable<ItemViewModel> items)
            : base(items)
        {
        }
    }

    public class ItemViewModel : ObservableObject
    {
        public bool IsActive { get; set; }
        public int Position { get; set; }
        public int Id { get; set; }//440
        public int NumCode { get; set; }//510
        public int Scale { get; set; }//1000
        public string CharCode { get; set; }//USD
        public string Description { get; set; }//Армянских драмов

        private bool _isBeingDragged;
        public bool IsBeingDragged
        {
            get { return _isBeingDragged; }
            set { SetProperty(ref _isBeingDragged, value); }
        }

        private bool _isBeingDraggedOver;
        public bool IsBeingDraggedOver
        {
            get { return _isBeingDraggedOver; }
            set { SetProperty(ref _isBeingDraggedOver, value); }
        }
    }
}
