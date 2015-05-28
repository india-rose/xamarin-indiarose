using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using IndiaRose.Application.Views;
using IndiaRose.Application.Views.Admin.Collection;
using IndiaRose.Application.Views.Collection;
using IndiaRose.Application.Views.Dialogs;
using IndiaRose.Business;
using IndiaRose.Business.ViewModels.Admin.Collection.Dialogs;
using Storm.Mvvm.Inject;
using IndiagramPropertyPage = IndiaRose.Application.Views.IndiagramPropertyPage;
using ViewKey = IndiaRose.Business.Views;

namespace IndiaRose.Application.CompositionRoot
{
    static class Bootstrap
    {
        public static void Initialize(Frame rootFrame)
        {
            var views = new Dictionary<string, Type>
            {
				{ViewKey.ADMIN_COLLECTION_HOME,typeof(CollectionManagementPage)},
                {ViewKey.ADMIN_HOME, typeof(MainPage)},
                {ViewKey.ADMIN_CREDITS, typeof(CreditsPage)},
                {ViewKey.ADMIN_SETTINGS_HOME, typeof(AppSettingsPage)},
                {ViewKey.ADMIN_SERVERSYNCHRONIZATION, typeof(ServerSynchronizationPage)},
                {ViewKey.ADMIN_SETTINGS_INDIAGRAMPROPERTIES, typeof(IndiagramPropertyPage)},
                {ViewKey.ADMIN_SETTINGS_APPLICATIONLOOK,typeof(ApplicationLookPage)},
                {ViewKey.ADMIN_COLLECTION_ADDINDIAGRAM,typeof(AddIndiagramPage)},
                {ViewKey.ADMIN_COLLECTION_WATCHINDIAGRAM, typeof(WatchIndiagramPage)}
            };

			var dialogs = new Dictionary<string, Type>
            {
				{Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_CATEGORY,typeof(ExploreCollectionCategoryDialog)},
				{Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_INDIAGRAM,typeof(ExploreCollectionIndiagramDialog)},
                {Dialogs.ADMIN_SETTINGS_RESETSETTINGS,typeof(ResetSettingsDialog)},
                {Dialogs.ADMIN_SETTINGS_DRAGANDDROP, typeof(DragAndDropDialog)},
                {Dialogs.ADMIN_SETTINGS_CATEGORYREADING, typeof(CategoryReadingDialog)},
                {Dialogs.ADMIN_SETTINGS_READINGDELAY, typeof(ReadingDelayDialog)},
                {Dialogs.ADMIN_SETTINGS_COLORPICKER, typeof(ColorPickerDialog)},
                {Dialogs.ADMIN_MAILERROR,typeof(MailErrorDialog)},
                {Dialogs.IMPORTING_COLLECTION, typeof(ImportingCollectionDialog)},
                {Dialogs.ADMIN_COLLECTION_IMAGECHOICE, typeof(ImageChoiceDialog)},
                {Dialogs.ADMIN_COLLECTION_SOUNDCHOICE, typeof(SoundChoiceDialog)},
                {Dialogs.ADMIN_COLLECTION_RECORDSOUND,typeof(RecordSoundDialog)},
                {Dialogs.ADMIN_COLLECTION_DELETECONFIRMATION_CATEGORY,typeof(DeleteCategoryConfirmation)},
                {Dialogs.ADMIN_COLLECTION_DELETEWARNING_CATEGORY,typeof(DeleteCategoryWarning)},
                {Dialogs.ADMIN_COLLECTION_DELETEWARNING_INDIAGRAM, typeof(DeleteIndiagramWarningDialog)}
            };


			WindowsContainer container = WindowsContainer.CreateInstance<Container>(rootFrame, views, dialogs);
            ViewModelsLocator.Initialize(container);
        }
    }
}
