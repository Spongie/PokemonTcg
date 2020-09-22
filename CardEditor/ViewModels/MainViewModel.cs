using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardEditor.ViewModels
{
    public class MainViewModel : DataModel
    {
		private SetsViewModel setViewModel;

		public MainViewModel()
		{
			SetViewModel = new SetsViewModel();
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
