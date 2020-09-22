using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace CardEditor.ViewModels
{
    public class MainViewModel : DataModel
    {
		private SetsViewModel setViewModel;

		public MainViewModel()
		{
			SetViewModel = new SetsViewModel();
		}

		public async Task Save()
		{
			Directory.CreateDirectory("Data");
			await SetViewModel.Save();
		}

		public async Task Load()
		{
			if (!Directory.Exists("Data"))
				return;

			await SetViewModel.LoadSets();
		}

		public SetsViewModel SetViewModel
		{
			get { return setViewModel; }
			set
			{
				setViewModel = value;
				FirePropertyChanged();
			}
		}

	}
}
