using Entities.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
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

		internal async Task LoadSets()
		{
			var json = await File.ReadAllTextAsync("Data/sets.json");

			var loadedSets = JsonConvert.DeserializeObject<List<Set>>(json);
			Sets.Clear();
			
			foreach (var set in loadedSets)
			{
				Sets.Add(set);
			}
		}

		internal async Task Save()
		{
			var json = JsonConvert.SerializeObject(Sets.ToList());

			await File.WriteAllTextAsync("Data/sets.json", json);
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
