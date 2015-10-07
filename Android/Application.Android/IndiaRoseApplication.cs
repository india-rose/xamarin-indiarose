using System;
using System.Collections.Generic;
using Android.App;
using Android.Runtime;
using IndiaRose.Application.Activities;
using IndiaRose.Application.Activities.Admin;
using IndiaRose.Application.Activities.Admin.Collection;
using IndiaRose.Application.Activities.Admin.Collection.Dialogs;
using IndiaRose.Application.Activities.Admin.Settings;
using IndiaRose.Application.Activities.Admin.Settings.Dialogs;
using IndiaRose.Application.Activities.User;
using IndiaRose.Business;
using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Xamarin;

namespace IndiaRose.Application
{
	[Application]
	public class IndiaRoseApplication : ApplicationBase
	{
		public IndiaRoseApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
		{

		}

		public override void OnCreate()
		{
			base.OnCreate();

			Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
			{
				if (isStartupCrash)
				{
					Insights.PurgePendingCrashReports().Wait();
				}
			};
			Insights.Initialize("794b025da8b9b50d4e4b6f8abf64cc051ec52f94", Context);
			Insights.Track("IndiaRoseLaunched", "Email", "dev@indiarose.org");

            Dictionary<string, Type> views = new Dictionary<string, Type>
			{
				// Admin
				{Views.ADMIN_HOME, typeof(HomeActivity)},
                {Views.ADMIN_CREDITS, typeof(CreditsActivity)},
                {Views.ADMIN_INSTALLVOICE_SYNTHESIS, typeof(InstallTextToSpeechActivity)},
                {Views.ADMIN_SERVERSYNCHRONIZATION, typeof(ServerSynchronizationActivity)},
				// Admin/Settings
                {Views.ADMIN_SETTINGS_HOME, typeof(AppSettingsActivity)},
                {Views.ADMIN_SETTINGS_APPLICATIONLOOK, typeof(ApplicationLookActivity)},
         		{Views.ADMIN_SETTINGS_INDIAGRAMPROPERTIES, typeof(IndiagramPropertyActivity)},
				// Admin/Collection
                {Views.ADMIN_COLLECTION_HOME, typeof(CollectionManagementActivity)},
                {Views.ADMIN_COLLECTION_ADDINDIAGRAM, typeof(AddIndiagramActivity)},
                {Views.ADMIN_COLLECTION_WATCHINDIAGRAM,typeof(WatchIndiagramActivity)},

				// User
				{Views.USER_HOME, typeof(UserHomeActivity)}
			};
            Dictionary<string, Type> dialogs = new Dictionary<string, Type>
			{
				// /
				{Dialogs.IMPORTING_COLLECTION, typeof(ImportingCollectionDialog)},
				// Admin
                {Dialogs.ADMIN_MAILERROR,typeof(MailErrorDialog)},
				// Admin/Settings/Dialogs
				{Dialogs.ADMIN_SETTINGS_COLORPICKER, typeof(ColorPickerDialog)},
				{Dialogs.ADMIN_SETTINGS_DRAGANDDROP, typeof(DragAndDropDialog)},
                {Dialogs.ADMIN_SETTINGS_READINGDELAY , typeof(ReadingDelayDialog)},
				{Dialogs.ADMIN_SETTINGS_CATEGORYREADING, typeof(CategoryReadingDialog)},
                {Dialogs.ADMIN_SETTINGS_RESETSETTINGS, typeof(ResetSettingsDialog)},
				// Admin/Collection/Dialogs
				// called from add indiagram
                {Dialogs.ADMIN_COLLECTION_IMAGECHOICE,typeof(ImageChoiceDialog)},
                {Dialogs.ADMIN_COLLECTION_SOUNDCHOICE,typeof(SoundChoiceDialog)},
                {Dialogs.ADMIN_COLLECTION_RECORDSOUND,typeof(RecordSoundDialog)},
				// called from collection browsing
				{Dialogs.ADMIN_COLLECTION_CHOOSE,typeof(ChooseCategoryDialog)},
				{Dialogs.ADMIN_COLLECTION_SELECTCATEGORY,typeof(SelectCategoryActionDialog)},
				{Dialogs.ADMIN_COLLECTION_SELECTCATEGORY_WITHOUTCHILDREN,typeof(SelectCategoryWithoutChildrenActionDialog)},
				{Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_CATEGORY,typeof(ExploreCollectionCategoryDialog)},
				{Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_CATEGORY_WITHOUTCHILDREN,typeof(ExploreCollectionCategoryWithoutChildrenDialog)},
                {Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION_INDIAGRAM,typeof(ExploreCollectionIndiagramDialog)},
				// deletion alert & confirmation
				{Dialogs.ADMIN_COLLECTION_DELETEWARNING_INDIAGRAM,typeof(DeleteIndiagramWarningDialog)},
                {Dialogs.ADMIN_COLLECTION_DELETEWARNING_CATEGORY,typeof(DeleteCategoryWarningDialog)},
				{Dialogs.ADMIN_COLLECTION_DELETECONFIRMATION_CATEGORY,typeof(DeleteCategoryConfirmationDialog)},

			};

			AndroidContainer.CreateInstance<Container>(this, views, dialogs);
		}
	}
}
