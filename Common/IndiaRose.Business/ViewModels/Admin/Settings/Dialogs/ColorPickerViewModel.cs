﻿#region Usings

using System.Windows.Input;
using IndiaRose.Data.UIModel;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
    /// <summary>
    /// VueModèle pour le dialogue de choix des couleurs
    /// </summary>
	public class ColorPickerViewModel : ViewModelBase
	{
		public const string COLOR_CONTAINER_PARAMETER = "Color";

		#region Properties

        private string _currentColor;
        private string _oldColor;
		private ColorContainer _color;

        public string CurrentColor
		{
			get { return _currentColor; }
            set { SetProperty(ref _currentColor, value); }
		}

        public string OldColor
		{
			get { return _oldColor; }
			set { SetProperty(ref _oldColor, value); }
		}

		[NavigationParameter]
		public ColorContainer Color
		{
			get { return _color; }
			set
			{
				_color = value;
				if (_color != null)
				{
				    OldColor = _color.Color;
				    CurrentColor = _color.Color;
				}
				else
				{
				    OldColor = CurrentColor = "#FF000000";
				}
			}
		}

		#endregion
		public ICommand SaveCommand { get; set; }
        public ICommand CloseDialogCommand { get; set; }

		public ColorPickerViewModel()
		{
			SaveCommand = new DelegateCommand(SaveAction);
            CloseDialogCommand = new DelegateCommand(CloseDialogAction);
		}

	    private void CloseDialogAction()
	    {
	        LazyResolver<IMessageDialogService>.Service.DismissCurrentDialog();
	    }

		private void SaveAction()
		{
			if (Color != null)
			{
				Color.Color = CurrentColor;
			}
            CloseDialogAction();
		}

	}
}