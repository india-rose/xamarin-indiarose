#region Usings

using IndiaRose.Framework.Converters;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Framework.ColorPicker;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "SaveCommand", TargetPath = "PositiveButtonEvent")]
	[BindingElement(Path = "CurrentColor", TargetPath = "CurrentColor", Mode = BindingMode.TwoWay, Converter = typeof(ColorConverter))]
    [BindingElement(Path = "OldColor", TargetPath = "OldColor", Converter = typeof(ColorConverter))]
	public partial class ColorPickerDialog : AbstractColorPickerDialog
	{
		protected override ViewModelBase CreateViewModel()
		{
			return Container.Locator.AdminSettingsDialogsColorPickerViewModel;
		}
	}
}