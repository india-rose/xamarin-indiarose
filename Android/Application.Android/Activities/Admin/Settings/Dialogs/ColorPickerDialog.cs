#region Usings

using IndiaRose.Framework.Converters;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Framework.ColorPicker;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

#endregion

namespace IndiaRose.Application.Activities.Admin.Settings.Dialogs
{
	[BindingElement(Path = "SaveCommand", TargetPath = "PositiveButtonEvent")]
	[BindingElement(Path = "CurrentColor", TargetPath = "CurrentColor", Mode = BindingMode.TwoWay, Converter = typeof(ColorStringToIntConverter))]
    [BindingElement(Path = "OldColor", TargetPath = "OldColor", Converter = typeof(ColorStringToIntConverter))]
	public partial class ColorPickerDialog : AbstractColorPickerDialog
	{
		protected override ViewModelBase CreateViewModel()
		{
		    return Container.Locator.AdminSettingsDialogsColorPickerViewModel;
		}

	    public ColorPickerDialog()
        {
            var trad = DependencyService.Container.Resolve<ILocalizationService>();
            Buttons[DialogsButton.Positive] = trad.GetString("Button_Ok", "Text");
            Buttons[DialogsButton.Negative] = trad.GetString("Button_Back", "Text");
            //TODO le string pour le titre du colorpicker
            //Title = trad.GetString()
	    }
	}
}