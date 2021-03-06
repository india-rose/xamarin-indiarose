﻿#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using IndiaRose.Business.ViewModels.Admin.Settings.Dialogs;
using IndiaRose.Data.UIModel;
using IndiaRose.Interfaces;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
    /// <summary>
    /// VueModèle de la page "Couleur de Fond"
    /// </summary>
    public class ApplicationLookViewModel : AbstractSettingsViewModel
    {
        #region Properties

        private int _currentSize;
        private ColorContainer _topColor;
        private ColorContainer _bottomColor;

        public ColorContainer BottomColor
        {
            get { return _bottomColor; }
            set { SetProperty(ref _bottomColor, value); }
        }
        public ColorContainer TopColor
        {
            get { return _topColor; }
            set { SetProperty(ref _topColor, value); }
        }
        public int CurrentSize
        {
            get { return _currentSize; }
            set { SetProperty(ref _currentSize, value); }
        }

        public ObservableCollection<int> ZoneHeightCollection { get; private set; }

        #endregion

        #region Services

        public IScreenService ScreenService
        {
            get { return LazyResolver<IScreenService>.Service; }
        }

        #endregion

        #region Commands

        public ICommand TopAreaColorCommand { get; private set; }
        public ICommand BottomAreaColorCommand { get; private set; }

        #endregion

        public ApplicationLookViewModel()
        {
            TopAreaColorCommand = new DelegateCommand(TopAreaColorAction);
            BottomAreaColorCommand = new DelegateCommand(BottomAreaColorAction);

            ZoneHeightCollection = new ObservableCollection<int>();
            InitZoneHeightsArray();
            //Initialisation des paramètres déjà connu
            CurrentSize = SettingsService.SelectionAreaHeight;
            BottomColor = new ColorContainer
            {
                Color = SettingsService.BottomBackgroundColor
            };
            TopColor = new ColorContainer
            {
                Color = SettingsService.TopBackgroundColor
            };
        }

        /// <summary>
        /// Calcul les tailles pour les zones possibles
        /// </summary>
        private void InitZoneHeightsArray()
        {
            // clear the collection before filling it
            ZoneHeightCollection.Clear();

            // calculate the minimal size of the top area
            // : indiagram size + 20% 
            int minHeight = (int)((SettingsService.IndiagramDisplaySize * 1.2) / ScreenService.Height * 100);


            // calculate the maximal size of the top area
            // : 95% of the screen minus the minimal size of the bottom area.
            int maxHeight = 95 - minHeight;

            // Limit automatically to 50%
            minHeight = Math.Max(minHeight, 50);

            for (int i = minHeight; i <= maxHeight; ++i)
            {
                ZoneHeightCollection.Add(i);
            }
        }
        
        #region Action
        /// <summary>
        /// Ouvre le dialogue de choix des couleurs pour changer la couleur de fond de la partie basse (partie lecture)
        /// </summary>
        private void BottomAreaColorAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{

				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, BottomColor}
			});
        }

        /// <summary>
        /// Ouvre le dialogue de choix des couleurs pour changer la couleur de fond de la partie haute (partie collection)
        /// </summary>
        private void TopAreaColorAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{
				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, TopColor}
			});
        }

        protected override void SaveAction()
        {
            SettingsService.SelectionAreaHeight = CurrentSize;
            SettingsService.TopBackgroundColor = TopColor.Color;
            SettingsService.BottomBackgroundColor = BottomColor.Color;
            base.SaveAction();
            BackAction();
        }
        #endregion
    }
}