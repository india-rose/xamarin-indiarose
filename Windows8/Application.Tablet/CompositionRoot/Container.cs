using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using IndiaRose.Interfaces;
using IndiaRose.Services;
using Storm.Mvvm.Inject;

namespace IndiaRose.Application.CompositionRoot
{
	class Container : WindowsContainer
	{
		protected override void Initialize(Frame rootFrame, Dictionary<string, Type> views, Dictionary<string, Type> dialogs)
		{
			base.Initialize(rootFrame, views, dialogs);
		
			RegisterInstance<IEmailService>(new EmailService());
		}
	}
}
