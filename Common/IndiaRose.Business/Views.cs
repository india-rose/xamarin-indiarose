namespace IndiaRose.Business
{
    /// <summary>
    /// Classe pour les déclarations des pages
    /// Sert pour la navigation
    /// </summary>
	public class Views
	{
        // SplashScreen
        public const string SPLASH_SCREEN = "SplashScreen";

        // Pages in /Admin
        public const string ADMIN_CREDITS = "Admin_CreditsView";
		public const string ADMIN_HOME = "Admin_HomeView";
		public const string ADMIN_INSTALLVOICE_SYNTHESIS = "Admin_InstallVoiceSynthesisView";
		public const string ADMIN_SERVERSYNCHRONIZATION = "Admin_ServerSynchronizationView";

		// Pages in /Admin/Settings
		public const string ADMIN_SETTINGS_HOME = "Admin_Settings_HomeView";
		public const string ADMIN_SETTINGS_APPLICATIONLOOK = "Admin_Settings_ApplicationLookView";
		public const string ADMIN_SETTINGS_INDIAGRAMPROPERTIES = "Admin_Settings_IndiagramPropertiesView";
        public const string ADMIN_SETTINGS_APPBEHAVIOUR = "Admin_Settings_AppBehaviourView";

        //Pages in /Admin/Collection
	    public const string ADMIN_COLLECTION_HOME = "Admin_Collection_HomeView";
	    public const string ADMIN_COLLECTION_ADDINDIAGRAM = "Admin_AddIndiagramView";
	    public const string ADMIN_COLLECTION_WATCHINDIAGRAM = "Admin_WatchIndiagramView";

        //Pages in /Users
	    public const string USER_HOME = "User_HomeView";

	}
}