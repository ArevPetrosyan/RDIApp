using RDIApp.Helpers;
using RDIApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace RDIApp.ViewModels
{
    public class CurrencySettingsViewModel : ObservableObject
    {
        private ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private ObservableCollection<ItemsListViewModel> _groupedItems = new ObservableCollection<ItemsListViewModel>();
        public ObservableCollection<ItemsListViewModel> GroupedItems
        {
            get { return _groupedItems; }
            set { SetProperty(ref _groupedItems, value); }
        }

        public ICommand ItemDragged { get; }

        public ICommand ItemDraggedOver { get; }

        public ICommand ItemDragLeave { get; }

        public ICommand ItemDropped { get; }

        public ICommand SaveChanges { get; }

        public ICommand SelectedItemCommand { get; private set; }

        public CurrencySettingsViewModel()
        {
            SaveChanges = new Command(SaveItemChanges);
            ItemDragged = new Command<ItemViewModel>(OnItemDragged);
            ItemDraggedOver = new Command<ItemViewModel>(OnItemDraggedOver);
            ItemDragLeave = new Command<ItemViewModel>(OnItemDragLeave);
            ItemDropped = new Command<ItemViewModel>(i => OnItemDropped(i));
            ResetItemsState();
        }

        private void SaveItemChanges()
        {
            DataProviderService.EditSettings(GroupedItems[0]);
            DataProviderService.UpdateSettings();
        }

        private void OnItemDragged(ItemViewModel item)
        {
            Items.ForEach(i => i.IsBeingDragged = item == i);
        }

        private void OnItemDraggedOver(ItemViewModel item)
        {
            var itemBeingDragged = _items.FirstOrDefault(i => i.IsBeingDragged);
            Items.ForEach(i => i.IsBeingDraggedOver = item == i && item != itemBeingDragged);
        }

        private void OnItemDragLeave(ItemViewModel item)
        {
            Items.ForEach(i => i.IsBeingDraggedOver = false);
        }

        private void OnItemDropped(ItemViewModel item)
        {
            var itemToMove = _items.First(i => i.IsBeingDragged);
            var itemToInsertBefore = item;

            if (itemToMove == null || itemToInsertBefore == null || itemToMove == itemToInsertBefore)
                return;

            var categoryToMoveFrom = GroupedItems.First(g => g.Contains(itemToMove));
            categoryToMoveFrom.Remove(itemToMove);

            var categoryToMoveTo = GroupedItems.First(g => g.Contains(itemToInsertBefore));
            var insertAtIndex = categoryToMoveTo.IndexOf(itemToInsertBefore);
            itemToMove.Position = insertAtIndex;
            categoryToMoveTo.Insert(insertAtIndex, itemToMove);
            itemToMove.IsBeingDragged = false;
            itemToInsertBefore.IsBeingDraggedOver = false;

            RecalcPositions(insertAtIndex);
        }

        private void ResetItemsState()
        {
            Items.Clear();
            var items = new List<ItemViewModel>();
            foreach (var item in AppDataState.AppSettings.Values)
            {
                items.Add(new ItemViewModel
                {
                    Description = item.Description,
                    CharCode = item.CharCode,
                    NumCode = item.NumCode,
                    Position = item.Position,
                    Scale = item.Scale,
                    IsActive = item.IsActive
                });
            }
            Items = new ObservableCollection<ItemViewModel>(items.OrderBy(x => x.Position));

            GroupedItems = Items
                .GroupBy(i => i.Id)
                .Select(g => new ItemsListViewModel(g))
                .ToObservableCollection();
        }

        private void RecalcPositions(int index)
        {
            for (int i = index; i < GroupedItems[0].Count; i++)
                GroupedItems[0][i].Position = i;
        }
    }
}
