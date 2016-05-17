using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using IndiaRose.Framework.Views;
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

        public Tests()
        {
            // J'ai déplacé cette partie dans le constructeur pour éviter de relancer l'app à chaque test (c'est ce qui se passe quand ce code est dans BeforeEachTest())
            app = ConfigureApp
                .Android
                //.ApkFile ("../../../../Android/Application.Android/bin/Debug/org.indiarose.apk")
                .LaunchableActivity("md5e9eb850b4bfdb148cd4a09650ab85c90.AdminSplashscreenActivity")
                .StartApp();
        }

        [SetUp]
        public void BeforeEachTest()
        {
            
        }

        /// <summary>
        /// Permet de vérifier si l'indiagram est bien créé
        /// Le problème est que pour le moment, on ne peut pas pas accéder aux services ici
        /// On vérifie donc seulement si on a un IndiagramView de plus dans l'IndiagramBrowserView
        /// Ce qui n'est pas fiable, cas dans le cas où tous les indiagrams ne peuvent pas être affichés,
        /// le nouvel indiagram ne sera pas dans la vue alors qu'il pourrait très bien avoir été ajouté quand même
        /// </summary>
        [Test]
        public void AddIndiagramTest()
        {
            //app.Repl();

            app.Tap("Collection management");
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));

            int nbIndiagrams = app.Query(c => c.Class("IndiagramView")).Length;
            app.Tap("Add");
            app.EnterText(c => c.Class("EditText"), "TestIndiagram");
            app.DismissKeyboard();
            app.Tap("Ok");

            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            int nbIndiagramsAfterAdd = app.Query(c => c.Class("IndiagramView")).Length;
            //Debug.WriteLine("Add : " + nbIndiagrams + " -> " + nbIndiagramsAfterAdd);
            Assert.AreEqual(nbIndiagrams + 1, nbIndiagramsAfterAdd);
        }

        /// <summary>
        /// Permet de vérifier si l'indiagram est bien supprimé
        /// Problème de fiabilité, cf summary de AddIndiagramTest
        /// </summary>
        [Test]
        public void DeleteIndiagramTest()
        {
            int nbIndiagrams = app.Query(c => c.Class("IndiagramView")).Length;
            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp[temp.Length - 2]; // Le dernier element est la fleche "next", donc on prend celui d'avant
            app.Tap(ind.Id);
            app.Tap("Delete");
            app.Tap("Delete");

            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            int nbIndiagramsAfterDelete = app.Query(c => c.Class("IndiagramView")).Length;
            //Debug.WriteLine("Delete : " + nbIndiagrams + " -> " + nbIndiagramsAfterDelete);
            Assert.AreEqual(nbIndiagrams - 1, nbIndiagramsAfterDelete);
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

