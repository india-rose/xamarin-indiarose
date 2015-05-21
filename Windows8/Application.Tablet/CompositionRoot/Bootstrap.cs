using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using IndiaRose.Application.Views;
using IndiaRose.Application.Views.Collection;
using IndiaRose.Application.Views.Dialogs;
using IndiaRose.Business;
using IndiaRose.Business.ViewModels.Admin.Collection;
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
            };

			var dialogs = new Dictionary<string, Type>
            {
                {Dialogs.ADMIN_SETTINGS_RESETSETTINGS,typeof(ResetSettingsDialog)},
                {Dialogs.ADMIN_SETTINGS_DRAGANDDROP, typeof(DragAndDropDialog)},
                {Dialogs.ADMIN_SETTINGS_CATEGORYREADING, typeof(CategoryReadingDialog)},
                {Dialogs.ADMIN_SETTINGS_READINGDELAY, typeof(ReadingDelayDialog)},
                {Dialogs.ADMIN_SETTINGS_COLORPICKER, typeof(ColorPickerDialog)}
            };


			WindowsContainer container = WindowsContainer.CreateInstance<Container>(rootFrame, views, dialogs);
            ViewModelsLocator.Initialize(container);
        }
    }
}
