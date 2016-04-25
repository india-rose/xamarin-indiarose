using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndiaRose.Business.ViewModels.Admin.Settings
{
    public class AppBehaviorViewModel : AbstractSettingsViewModel
    {

        #region Properties
        private bool _dragAndDrop;
        private bool _categoryReading;
        private bool _backButtonActivated;
        private float _delay;

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
        #endregion
    }
}
