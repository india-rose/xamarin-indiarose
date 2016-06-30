using System;
using System.Diagnostics;
using System.Windows.Input;
using Storm.Mvvm.Commands;

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
    public class AppBehaviourViewModel : AbstractSettingsViewModel
    {

        #region Properties
        private bool _dragAndDrop;
        private bool _categoryReading;
        private bool _backButtonActivated;
        private float _delay;
        private int _sliderValue;

        public bool DragAndDrop
        {
            get { return _dragAndDrop; }
            set { SetProperty(ref _dragAndDrop, value); }
        }

        public bool CategoryReading
        {
            get { return _categoryReading; }
            set { SetProperty(ref _categoryReading, value); }
        }

        public bool BackButtonActivated
        {
            get { return _backButtonActivated; }
            set { SetProperty(ref _backButtonActivated, value); }
        }

        public float Delay
        {
            get { return _delay; }
            set { SetProperty(ref _delay, value); }
        }

        public int SliderValue
        {
            get { return _sliderValue; }
            set
            {
                if (SetProperty(ref _sliderValue, value))
                {
                    Delay = (value) / 10.0f;
                }
            }
        }
        #endregion

        public AppBehaviourViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            DragAndDrop = SettingsService.IsDragAndDropEnabled;
            CategoryReading = SettingsService.IsCategoryNameReadingEnabled;
            BackButtonActivated = SettingsService.IsBackButtonEnabled;
            Delay = SettingsService.TimeOfSilenceBetweenWords;
            
            SliderValue = Convert.ToInt32(Delay*10.0f);
            // Ne pas remplacer par SliderValue = (int)(Delay*10.0f); qui renvoie parfois des résultats erronés (tester avec 2.6 par ex). Pourquoi ?

            // float temp = Delay*10.0f;
            // SliderValue = (int) temp;
            // Fonctionne, mais demande une variable intermédiaire.
        }

        protected override void SaveAction()
        {
            SettingsService.IsDragAndDropEnabled = DragAndDrop;
            SettingsService.IsCategoryNameReadingEnabled = CategoryReading;
            SettingsService.IsBackButtonEnabled = BackButtonActivated;
            SettingsService.TimeOfSilenceBetweenWords = Delay;

            base.SaveAction();
            BackAction();
        }
    }
}

