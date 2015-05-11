using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using IndiaRose.Application.Views;
using IndiaRose.Business;
using Storm.Mvvm.Inject;
using ViewKey = IndiaRose.Business.Views;

namespace IndiaRose.Application.CompositionRoot
{
    static class Bootstrap
    {
        public static void Initialize(Frame rootFrame)
        {
            var views = new Dictionary<string, Type>
            {
                {ViewKey.ADMIN_HOME, typeof(MainPage)},
                {ViewKey.ADMIN_CREDITS, typeof(CreditsPage)},
                {ViewKey.ADMIN_SETTINGS_HOME, typeof(AppSettingsPage)},
            };

			var dialogs = new Dictionary<string, Type>
            {

            };


			WindowsContainer container = WindowsContainer.CreateInstance<Container>(rootFrame, views, dialogs);
            ViewModelsLocator.Initialize(container);
        }
    }
}
