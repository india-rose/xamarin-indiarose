#region Usings

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings.Dialogs;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
	public class BackgroundColorViewModel : AbstractBackViewModel
	{


		public ICommand OkCommand { get; private set; }
		public ICommand TopAreaColorCommand { get; private set; }
		public ICommand BottomAreaColorCommand { get; private set; }
	    
        private int _currentSize;

	    public int CurrentSize
	    {
	        get { return _currentSize; }
            set { SetProperty(ref _currentSize, value); }
	    }
		public ObservableCollection<int> ZoneHeightCollection { get; private set; }

		public ISettingsService SettingsService { get; private set; }

		public BackgroundColorViewModel(IContainer container)
			: base(container)
		{
			OkCommand = new DelegateCommand(OkAction);
			TopAreaColorCommand = new DelegateCommand(TopAreaColorAction);
			BottomAreaColorCommand = new DelegateCommand(BottomAreaColorAction);
			SettingsService = Container.Resolve<ISettingsService>();
			InitZoneHeightsArray();
		    CurrentSize = SettingsService.SelectionAreaHeight;
		}

		private void BottomAreaColorAction()
		{
			IMessageDialogService messageDialogService = Container.Resolve<IMessageDialogService>();
			messageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>()
			{
				{ColorPickerViewModel.COLOR_KEY_PARAMETER, ColorPickerViewModel.BOTTOM_AREA_BACKGROUND_COLOR}
			});
		}

		private void TopAreaColorAction()
		{
			IMessageDialogService messageDialogService = Container.Resolve<IMessageDialogService>();
			messageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>()
			{
				{ColorPickerViewModel.COLOR_KEY_PARAMETER, ColorPickerViewModel.TOP_AREA_BACKGROUND_COLOR}
			});
		}

		private void InitZoneHeightsArray()
		{
			// Si le tableau n'est pas initialisé
			if (ZoneHeightCollection == null)
			{
                ZoneHeightCollection = new ObservableCollection<int>();
			}
			// Vider le tableau pour le regenerer
            ZoneHeightCollection.Clear();
			// Service pour recuppérer la taille de l'ecran
			IScreenService screenService = Container.Resolve<IScreenService>();
			// Acces aux parametres de l'appli
			// Taille mini de la zone d'indiagrams
			// i.e: la taille d'un indiagram + 20% en pourcentage de la hauteur de l'écran
			int minHeight = (int) ((SettingsService.IndiagramDisplaySize*1.2)/screenService.Height*100);
			// Taille maxi de la zone d'indiagrams
			// i.e: 95% de la hauteur de l'écran - la hauteur minimum de la zone
			int maxHeight = 95 - minHeight;
			// Iteration sur toutes les tailles possibles
			for (int i = minHeight; i <= maxHeight; ++i)
			{
                ZoneHeightCollection.Add(i);
			}
		}

		private void OkAction()
		{
		    SettingsService.SelectionAreaHeight = CurrentSize;

            SettingsService.Save();
			BackAction();
		}
	}
}