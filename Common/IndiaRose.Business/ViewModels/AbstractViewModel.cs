﻿#region Usings

using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Business.ViewModels
{
    /// <summary>
    /// Classe abstraite dont tous les VueModèles doivent descendre
    /// </summary>
	public abstract class AbstractViewModel : ViewModelBase
	{
        protected IMessageDialogService MessageDialogService
        {
            get { return LazyResolver<IMessageDialogService>.Service; }
        }

		protected ILocalizationService LocalizationService
		{
			get { return LazyResolver<ILocalizationService>.Service; }
		}

        public ICommand CloseDialogCommand { get; private set; }
		public ICommand BackCommand { get; private set; }

		protected AbstractViewModel()
		{
			BackCommand = new DelegateCommand(BackAction);
            CloseDialogCommand = new DelegateCommand(CloseDialogAction);
		}

	    protected virtual void CloseDialogAction()
	    {
	        MessageDialogService.DismissCurrentDialog();
	    }
		protected virtual void BackAction()
		{
			NavigationService.GoBack();
		}
	}
}