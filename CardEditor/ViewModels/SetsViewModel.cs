using Entities.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CardEditor.ViewModels
{
	public class SetsViewModel : DataModel
    {
		private ObservableCollection<Set> sets = new ObservableCollection<Set>();

		public SetsViewModel()
		{
			AddNewCommand = new RelayCommand(CanAddNewSet, AddNewSet);
		}

		private bool CanAddNewSet(object obj)
		{
			return true;
		}

		private void AddNewSet(object obj)
		{
			Sets.Add(new Set { SetCode = "xxx", Name = "New set" });
		}

		public ObservableCollection<Set> Sets
		{
			get { return sets; }
			set
			{
				sets = value;
				FirePropertyChanged();
			}
		}

		private Set selectedSet;

		public Set SelectedSet
		{
			get { return selectedSet; }
			set
			{
				selectedSet = value;
				FirePropertyChanged();
			}
		}

		public ICommand AddNewCommand { get; set; }
	}
}
