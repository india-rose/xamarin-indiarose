#region Usings

using System.Windows.Input;
using IndiaRose.Data.UIModel;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

#endregion

namespace IndiaRose.Business.ViewModels.Admin.Settings.Dialogs
{
	public class ColorPickerViewModel : ViewModelBase
	{
		public const string COLOR_CONTAINER_PARAMETER = "Color";

		#region Properties

        private ColorContainer _currentColor;
        private ColorContainer _oldColor;
		private ColorContainer _color;

        public ColorContainer CurrentColor
		{
			get { return _currentColor; }
			set { SetProperty(ref _currentColor, value); }
		}

        public ColorContainer OldColor
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
                    OldColor = new ColorContainer()
                    {
                        Color = _color.Color,
                    };
                    CurrentColor = new ColorContainer()
                    {
                        Color = _color.Color,
                    };
				}
			}
		}

		#endregion
		public ICommand SaveCommand { get; set; }

		public ColorPickerViewModel(IContainer container) : base(container)
		{
			SaveCommand = new DelegateCommand(SaveAction);
		}

		private void SaveAction()
		{
			if (Color != null)
			{
				Color.Color = CurrentColor.Color;
			}
		}
	}
}