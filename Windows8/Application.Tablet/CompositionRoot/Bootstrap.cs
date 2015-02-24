using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using IndiaRose.Application.Views;
using IndiaRose.Business;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using Storm.Mvvm.Inject;
using ViewKey = IndiaRose.Business.Views;

namespace IndiaRose.Application.CompositionRoot
{
    class Bootstrap
    {
        public void Initialize(Frame rootFrame)
        {
            Container container = new Container(rootFrame);
            container.Initialize(new Dictionary<string, Type>()
            {
                {ViewKey.ADMIN_HOME, typeof(MainPage)},
                {ViewKey.ADMIN_CREDITS, typeof(CreditsPage)},
            });

            ViewModelsLocator.Initialize(container);

            EmailService emailService = new EmailService();
            container.RegisterInstance<IEmailService>(emailService);
        }
    }
}
