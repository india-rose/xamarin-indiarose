﻿#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// VueModèle de la page "Propriétés des Indiagrams"
    /// </summary>
    public class IndiagramPropertyViewModel : AbstractSettingsViewModel
    {
        #region Services

        protected IFontService FontService
        {
            get { return LazyResolver<IFontService>.Service; }
        }

        #endregion

        #region Properties

        private string _fontName;
        private int _indiagramSize;
        private int _fontSize;
        private ColorContainer _reinforcerColor;
        private ColorContainer _textColor;
        private bool _backAfterSelection;
        private bool _reinforcerEnabled;
        private bool _multipleIndiagramSelection;

        public bool BackAfterSelection
        {
            get { return _backAfterSelection; }
            set { SetProperty(ref _backAfterSelection, value); }
        }
        public bool ReinforcerEnabled
        {
            get { return _reinforcerEnabled; }
            set { SetProperty(ref _reinforcerEnabled, value); }
        }

        public bool MultipleIndiagramSelection
        {
            get { return _multipleIndiagramSelection; }
            set { SetProperty(ref _multipleIndiagramSelection, value); }
        }

        public ColorContainer ReinforcerColor
        {
            get { return _reinforcerColor; }
            set { SetProperty(ref _reinforcerColor, value); }
        }

        public ColorContainer TextColor
        {
            get { return _textColor; }
            set { SetProperty(ref _textColor, value); }
        }

        public int IndiagramSize
        {
            get { return _indiagramSize; }
            set { SetProperty(ref _indiagramSize, value); }
        }

        public string FontName
        {
            get { return _fontName; }
            set { SetProperty(ref _fontName, value); }
        }

        public int FontSize
        {
            get { return _fontSize; }
            set { SetProperty(ref _fontSize, value); }
        }

        public ObservableCollection<int> IndiagramSizes { get; private set; }
        public ObservableCollection<string> FontNames { get; private set; }
        public ObservableCollection<int> FontSizes { get; private set; }

        #endregion

        public ICommand ReinforcerColorCommand { get; private set; }
        public ICommand TextColorCommand { get; private set; }

        public IndiagramPropertyViewModel()
        {
            ReinforcerColorCommand = new DelegateCommand(ReInforcerColorAction);
            TextColorCommand = new DelegateCommand(TextColorAction);

            //Initialisation des paramètres
            ReinforcerColor = new ColorContainer
            {
                Color = SettingsService.ReinforcerColor
            };
            TextColor = new ColorContainer
            {
                Color = SettingsService.TextColor
            };
            ReinforcerEnabled = SettingsService.IsReinforcerEnabled;
            BackAfterSelection = SettingsService.IsBackHomeAfterSelectionEnabled;
            MultipleIndiagramSelection = SettingsService.IsMultipleIndiagramSelectionEnabled;

            //Création de la liste de taille possible pour les Indiagrams
            IndiagramSizes = new ObservableCollection<int>();
            int maxIndiagramSize = (int)(LazyResolver<IScreenService>.Service.Height * 0.4);

            foreach (int size in new[] { 32, 48, 64, 80, 128, 160, 200, 256, 280, 300, 350, 400, 450, 500, 550, 600 }.Where(x => x <= maxIndiagramSize))
            {
                IndiagramSizes.Add(size);
            }
            IndiagramSize = SettingsService.IndiagramDisplaySize;

            //Création de la liste de taille de police possible
            FontSizes = new ObservableCollection<int>();

            foreach (int size in Enumerable.Range(8, 50).Where(x => x % 2 == 0))
            {
                FontSizes.Add(size);
            }
            FontSize = SettingsService.FontSize;

            //Création de la liste des polices disponibles
            FontNames = new ObservableCollection<string>();
            foreach (string font in FontService.FontList.Keys)
            {
                FontNames.Add(font);
            }
            if (FontService.FontList.ContainsValue(SettingsService.FontName))
            {
                FontName = FontService.FontList.First(
                    x => string.Equals(x.Value, SettingsService.FontName, StringComparison.OrdinalIgnoreCase)).Key;
            }
            else
            {
                FontName = FontService.FontList.First().Key;
            }
        }

        #region Action
        /// <summary>
        /// Ouvre le dialogue de choix des couleurs pour modifier la couleur du renforçateur de lecture des Indiagrams
        /// </summary>
        private void ReInforcerColorAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{
				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, ReinforcerColor}
			});
        }

        /// <summary>
        /// Ouvre le dialogue de choix des couleurs pour modifier la couleur du texte des Indiagrams
        /// </summary>
        private void TextColorAction()
        {
            MessageDialogService.Show(Business.Dialogs.ADMIN_SETTINGS_COLORPICKER, new Dictionary<string, object>
			{
				{ColorPickerViewModel.COLOR_CONTAINER_PARAMETER, TextColor}
			});
        }

        protected override void SaveAction()
        {
            SettingsService.IndiagramDisplaySize = IndiagramSize;
            SettingsService.FontSize = FontSize;
            SettingsService.FontName = FontService.FontList[FontName];
            SettingsService.ReinforcerColor = ReinforcerColor.Color;
            SettingsService.TextColor = TextColor.Color;
            SettingsService.IsBackHomeAfterSelectionEnabled = BackAfterSelection;
            SettingsService.IsReinforcerEnabled = ReinforcerEnabled;
            SettingsService.IsMultipleIndiagramSelectionEnabled = MultipleIndiagramSelection;

            base.SaveAction();
            BackAction();
        }
        #endregion
    }
}