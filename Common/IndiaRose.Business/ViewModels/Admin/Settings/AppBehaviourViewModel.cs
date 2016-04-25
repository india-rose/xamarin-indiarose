using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Delay = (value+10) / 10.0f;
                }
            }
        }
        #endregion

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public AppBehaviourViewModel()
        {
            OkCommand = new DelegateCommand(OkClicked);
            CancelCommand = new DelegateCommand(CancelClicked);
        }

        private void OkClicked()
        {
            //Debug.WriteLine("Drag&drop = " + DragAndDrop + " CategoryReading = " + CategoryReading + " BackButtonActivated = " + BackButtonActivated + " Delay/SliderValue = " + Delay + " / " + SliderValue);
        }

        private void CancelClicked()
        {
            
        }
    }
}

