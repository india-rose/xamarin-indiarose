using System;
using System.Collections.Generic;
using Windows.Globalization;
using Windows.UI.Xaml.Controls;
using Application.Tablet.Views.Admin;
using Application.Tablet.Views.Admin.Settings;
using Application.Tablet.Views.Dialogs;
using IndiaRose.Business;
using Storm.Mvvm.Inject;
using ColorPickerDialog = Application.Tablet.Views.Dialogs.ColorPickerDialog;
using IndiagramPropertyPage = Application.Tablet.Views.Admin.Settings.IndiagramPropertyPage;
using ViewKey = IndiaRose.Business.Views;

namespace Application.Tablet.CompositionRoot
{
    static class Bootstrap
    {
        public static void Initialize(Frame rootFrame)
        {
            var views = new Dictionary<string, Type>
            {
                {ViewKey.ADMIN_HOME, typeof(MainPage)},
                {ViewKey.ADMIN_CREDITS, typeof(CreditsPage) },
                {ViewKey.ADMIN_SETTINGS_APPLICATIONLOOK, typeof(ApplicationLookPage) },
                {ViewKey.ADMIN_SETTINGS_INDIAGRAMPROPERTIES, typeof(IndiagramPropertyPage) },
                {ViewKey.ADMIN_SETTINGS_HOME, typeof(AppSettingsPage) },
                {ViewKey.ADMIN_INSTALLVOICE_SYNTHESIS, typeof(InstallTextToSpeechPage) },
                {ViewKey.ADMIN_SETTINGS_APPBEHAVIOUR, typeof(AppBehaviourPage) }
            };

			var dialogs = new Dictionary<string, Type>
            {
				/*{Dialogs.ADMIN_COLLECTION_SELECTCATEGORY,typeof(SelectCategoryActionDialog)},
				{Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_CATEGORY,typeof(ExploreCollectionCategoryDialog)},
				{Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_INDIAGRAM,typeof(ExploreCollectionIndiagramDialog)},*/
                {Dialogs.ADMIN_SETTINGS_RESETSETTINGS,typeof(ResetSettingsDialog)},
                {Dialogs.ADMIN_SETTINGS_COLORPICKER, typeof(ColorPickerDialog)}
                /*{{Dialogs.ADMIN_MAILERROR,typeof(MailErrorDialog)},
                {Dialogs.ADMIN_COLLECTION_IMAGECHOICE, typeof(ImageChoiceDialog)},
                {Dialogs.ADMIN_COLLECTION_SOUNDCHOICE, typeof(SoundChoiceDialog)},
                {Dialogs.ADMIN_COLLECTION_RECORDSOUND,typeof(RecordSoundDialog)},
                {Dialogs.ADMIN_COLLECTION_DELETECONFIRMATION_CATEGORY,typeof(DeleteCategoryConfirmation)},
                {Dialogs.ADMIN_COLLECTION_DELETEWARNING_CATEGORY,typeof(DeleteCategoryWarning)},
                {Dialogs.ADMIN_COLLECTION_DELETEWARNING_INDIAGRAM, typeof(DeleteIndiagramWarningDialog)},
                {Dialogs.ADMIN_COLLECTION_CHOOSE,typeof(ChooseCategoryDialog)}*/
            };
            

			WindowsContainer container = WindowsContainer.CreateInstance<Container>(rootFrame, views, dialogs);
            ViewModelsLocator.Initialize(container);
        }
    }
}
