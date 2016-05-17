using System;
using System.IO;
using System.Linq;
using IndiaRose.Interfaces;
using NUnit.Framework;
using Storm.Mvvm.Inject;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;

namespace UITest.Android
{
    [TestFixture]
    public class Tests
    {
        AndroidApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            // TODO: If the Android app being tested is included in the solution then open
            // the Unit Tests window, right click Test Apps, select Add App Project
            // and select the app projects that should be tested.
            app = ConfigureApp
                .Android
                // TODO: Update this path to point to your Android app and uncomment the
                // code if the app is not included in the solution.

                //.ApkFile ("../../../../Android/Application.Android/bin/Debug/org.indiarose.apk")
                .LaunchableActivity("md5e9eb850b4bfdb148cd4a09650ab85c90.AdminSplashscreenActivity")

                .StartApp();
        }

        [Test]
        public void DummyTest()
        {
            app.Repl();
        }

        //Ne fonctionne pas car impossible de recupperer le service : null exception
        /*[Test]
        public void SettingResetTest()
        {
            ISettingsService settingsService = LazyResolver<ISettingsService>.Service;

            //Set des valeurs par defaut
            settingsService.TopBackgroundColor = "FF4042FF";
            LazyResolver<ISettingsService>.Service.BottomBackgroundColor = "FF5060FF";
            settingsService.SelectionAreaHeight = 60;
            settingsService.IndiagramDisplaySize = 300;
            settingsService.FontName = "DroidSans.ttf";
            settingsService.FontSize = 36;
            settingsService.IsReinforcerEnabled = false;
            settingsService.IsDragAndDropEnabled = true;
            settingsService.IsCategoryNameReadingEnabled = false;
            settingsService.IsBackButtonEnabled = false;
            settingsService.IsMultipleIndiagramSelectionEnabled = true;
            settingsService.TimeOfSilenceBetweenWords = 2.0f;
            settingsService.ReinforcerColor = "FF6589FF";
            settingsService.TextColor = "FF000000";
            settingsService.IsBackButtonEnabled = false;

            //Comportement de l'application
            app.Tap(c=>c.Button().Text("Application settings"));
            app.Tap(c=>c.Button().Text("Reset settings"));
            app.Tap(c=>c.Button().Text("Ok"));

            //verification du reset 
            Assert.Equals(settingsService.TopBackgroundColor, "#FF3838FF");
            Assert.Equals(settingsService.BottomBackgroundColor, "#FF73739E");
            Assert.Equals(settingsService.SelectionAreaHeight, 50);
            Assert.Equals(settingsService.IndiagramDisplaySize, 128);
            Assert.Equals(settingsService.FontName, "Consolas");
            Assert.Equals(settingsService.FontSize, 20);
            Assert.Equals(settingsService.IsReinforcerEnabled, true);
            Assert.Equals(settingsService.IsDragAndDropEnabled, false);
            Assert.Equals(settingsService.IsCategoryNameReadingEnabled, true);
            Assert.Equals(settingsService.IsBackHomeAfterSelectionEnabled, true);
            Assert.Equals(settingsService.IsMultipleIndiagramSelectionEnabled, false);
            Assert.Equals(settingsService.TimeOfSilenceBetweenWords, 1.0f);
            Assert.Equals(settingsService.ReinforcerColor, "#FFFF00FF");
            Assert.Equals(settingsService.TextColor, "#FFFFFFFF");
            Assert.Equals(settingsService.IsBackButtonEnabled, true);
        }*/
    }
}

