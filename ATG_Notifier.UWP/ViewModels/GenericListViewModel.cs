using ATG_Notifier.ViewModels.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ATG_Notifier.UWP.ViewModels
{
    public class GenericListViewModel<TModel> : ObservableObject
        where TModel : ObservableObject
    {
        private IList<TModel> items;
        private TModel selectedItem;

        public GenericListViewModel()
        {
            this.DeleteCommand = new RelayCommand<TModel>(OnRemove);
            this.ClearCommand = new RelayCommand(OnClear);
        }

        public IList<TModel> Items
        {
            get => items;
            set => Set(ref items, value);
        }

        public TModel SelectedItem
        {
            get => selectedItem;
            set => Set(ref selectedItem, value);
        }

        public bool IsEmpty => this.Items.Count == 0;

        public ICommand DeleteCommand { get; }

        public ICommand ClearCommand { get; }

        public void Add(TModel model)
        {
            OnAdd(model);
        }

        public void Remove(TModel model)
        {
            OnRemove(model);
        }

        protected virtual void OnAdd(TModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Items.Add(model);

            if (this.Items.Count == 1)
            {
                NotifyPropertyChanged(nameof(this.IsEmpty));
            }
        }

        protected virtual void OnRemove(TModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            bool status = this.Items.Remove(model);

            if (status && this.Items.Count == 0)
            {
                NotifyPropertyChanged(nameof(this.IsEmpty));
            }
        }

        protected virtual void OnClear()
        {
            var notCleared = this.Items.Count > 0;

            this.Items.Clear();

            if (notCleared)
            {
                NotifyPropertyChanged(nameof(this.IsEmpty));
            }
        }
    }
}
