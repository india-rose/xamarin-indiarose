using System;
using System.IO;
using System.Text;
using Android.App;
using Android.Content;
using IndiaRose.Data.Model;
using Newtonsoft.Json;
using Storm.Mvvm.Events;
using Storm.Mvvm.Interfaces;

namespace IndiaRose.Services.Android
{
    public class SettingsService : AbstractSettingsService
    {
	    private const string SETTINGS_FILE_NAME = "settings.json";

	    protected IActivityService ActivityService;

	    public SettingsService(IActivityService activityService)
	    {
		    ActivityService = activityService;

			ActivityService.ActivityChanged += OnActivityChanged;
	    }

	    private void OnActivityChanged(object sender, ValueChangedEventArgs<Activity> valueChangedEventArgs)
	    {
		    if (valueChangedEventArgs.NewValue != null)
		    {
			    ActivityService.ActivityChanged -= OnActivityChanged;
			    Load();
		    }
	    }

	    protected override bool ExistsOnDisk()
	    {
		    try
		    {
			    using (Stream result = ActivityService.CurrentActivity.OpenFileInput(SETTINGS_FILE_NAME))
			    {
					return result != null;
			    }
		    }
		    catch (Exception)
		    {
				return false;
		    }
	    }

	    protected override SettingsModel LoadFromDisk()
	    {
		    try
		    {
			    using (Stream inputStream = ActivityService.CurrentActivity.OpenFileInput(SETTINGS_FILE_NAME))
			    {
				    string content;
				    using (StreamReader reader = new StreamReader(inputStream, Encoding.UTF8))
				    {
					    content = reader.ReadToEnd();
				    }
				    SettingsModel result = JsonConvert.DeserializeObject<SettingsModel>(content);
				    return result;
			    }
		    }
		    catch (Exception)
		    {
				//TODO : add log
			    return null;
		    }
		    
	    }

	    protected override void SaveOnDisk(SettingsModel model)
	    {
		    try
		    {
			    Stream outputStream = ActivityService.CurrentActivity.OpenFileOutput(SETTINGS_FILE_NAME, FileCreationMode.Private);
			    string content = JsonConvert.SerializeObject(model, Formatting.None);
			    using (StreamWriter writer = new StreamWriter(outputStream, Encoding.UTF8))
			    {
				    writer.Write(content);
					writer.Flush();
			    }
		    }
		    catch (Exception)
		    {
			    //TODO : add log
		    }
	    }
    }
}
