using System;
using System.Collections.Generic;
using Android.App;
using Android.Runtime;
using IndiaRose.Application.Activities.Admin;
using IndiaRose.Application.Activities.Admin.Collection;
using IndiaRose.Application.Activities.Admin.Collection.Dialogs;
using IndiaRose.Application.Activities.Admin.Settings;
using IndiaRose.Application.Activities.Admin.Settings.Dialogs;
using IndiaRose.Business;
using Storm.Mvvm;
using Storm.Mvvm.Inject;

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

            Dictionary<string, Type> views = new Dictionary<string, Type>
			{
				{Views.ADMIN_HOME, typeof(HomeActivity)},
                {Views.ADMIN_CREDITS, typeof(CreditsActivity)},
                {Views.ADMIN_INSTALLVOICE_SYNTHESIS, typeof(InstallTextToSpeechActivity)},
                {Views.ADMIN_SERVERSYNCHRONIZATION, typeof(ServerSynchronizationActivity)},
                {Views.ADMIN_SETTINGS_HOME, typeof(AppSettingsActivity)},
                {Views.ADMIN_SETTINGS_APPLICATIONLOOK, typeof(ApplicationLookActivity)},
         		{Views.ADMIN_SETTINGS_INDIAGRAMPROPERTIES, typeof(IndiagramPropertyActivity)},
                {Views.ADMIN_COLLECTION_MANAGEMENT, typeof(CollectionManagementActivity)},
                {Views.ADMIN_COLLECTION_ADD, typeof(AddIndiagramActivity)},
                {Views.ADMIN_COLLECTION_WATCH,typeof(WatchIndiagramActivity)},
				{Views.ADMIN_COLLECTION_SELECTCATEGORY,typeof(SelectCategoryActivity)}
			};
            Dictionary<string, Type> dialogs = new Dictionary<string, Type>
			{
				{Dialogs.ADMIN_SETTINGS_COLORPICKER, typeof(ColorPickerDialog)},
				{Dialogs.ADMIN_SETTINGS_DRAGANDDROP, typeof(DragAndDropDialog)},
                {Dialogs.ADMIN_SETTINGS_READINGDELAY , typeof(ReadingDelayDialog)},
				{Dialogs.ADMIN_SETTINGS_CATEGORYREADING, typeof(CategoryReadingDialog)},
                {Dialogs.ADMIN_SETTINGS_RESETSETTINGS, typeof(ResetSettingsDialog)},
                {Dialogs.ADMIN_MAILERROR,typeof(MailErrorDialog)},
                {Dialogs.ADMIN_COLLECTION_EXPLORECOLLECTION,typeof(ExploreCollectionDialog)},
                {Dialogs.ADMIN_COLLECTION_IMAGECHOICE,typeof(ImageChoiceDialog)},
                {Dialogs.ADMIN_COLLECTION_SOUNDCHOICE,typeof(SoundChoiceDialog)},
                {Dialogs.ADMIN_COLLECTION_RECORDSOUND,typeof(RecordSoundDialog)},
				{Dialogs.ADMIN_COLLECTION_SELECT,typeof(SelectManagementDialog)},
				{Dialogs.ADMIN_COLLECTION_DELETEWARNING,typeof(DeleteWarningDialog)},
				{Dialogs.ADMIN_COLLECTION_DELETEINDIAGRAM,typeof(DeleteIndiagramDialog)},
				{Dialogs.ADMIN_COLLECTION_CONFIRMATION,typeof(ConfirmationDeleteDialog)},
                {Dialogs.ADMIN_COLLECTION_DELCATEGORYWARNING,typeof(DelCategoryWarningDialog)}

			};

			AndroidContainer.CreateInstance<Container>(this, views, dialogs);
		}
	}
}
