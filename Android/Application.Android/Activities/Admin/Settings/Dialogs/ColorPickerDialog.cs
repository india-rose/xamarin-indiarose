#region Usings

using IndiaRose.Business;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Framework.ColorPicker;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "PositiveCommand", TargetPath = "PositiveButtonEvent")]
	[BindingElement(Path = "CurrentColor", TargetPath = "CurrentColor", Mode = BindingMode.TwoWay)]
	[BindingElement(Path = "OldColor", TargetPath = "OldColor")]
	public partial class ColorPickerDialog : AbstractColorPickerDialog
	{
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminSettingsDialogsColorPickerViewModel;
		}
	}
}