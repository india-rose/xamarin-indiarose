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
    public class TestsAdmin
    {
        AndroidApp app;

        public TestsAdmin()
        {
            // J'ai déplacé cette partie dans le constructeur pour éviter de relancer l'app à chaque test (c'est ce qui se passe quand ce code est dans BeforeEachTest())
            app = ConfigureApp
                .Android
                //.ApkFile ("../../../../Android/Application.Android/bin/Debug/org.indiarose.apk")
                .LaunchableActivity("md5e9eb850b4bfdb148cd4a09650ab85c90.AdminSplashscreenActivity")
                .StartApp();
        }

        /*[SetUp]
        public void BeforeEachTest()
        {
            
        }*/

        /*[Test]
        public void OpenREPL()
        {
            app.Repl();
        }*/

       

        /// <summary>
        /// Permet de vérifier si l'indiagram est bien créé
        /// Le problème est que pour le moment, on ne peut pas pas accéder aux services ici
        /// On vérifie donc seulement si on a un IndiagramView de plus dans l'IndiagramBrowserView
        /// Ce qui n'est pas fiable, cas dans le cas où tous les indiagrams ne peuvent pas être affichés,
        /// le nouvel indiagram ne sera pas dans la vue alors qu'il pourrait très bien avoir été ajouté quand même
        /// </summary>
        [Test]
        public void A_AddIndiagramTest()
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
        /// Effectue automatiquement l'edit du dernier indiagram ajouter
        /// </summary>
        [Test]
        public void B_EditIndiagramTest()
        {
            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp[temp.Length - 2]; 
            app.Tap(ind.Id);
            app.Tap("See");
            app.Tap("Edit");
            app.ClearText(c => c.Class("EditText"));
            app.EnterText(c => c.Class("EditText"), "TestEditIndiagram");
            app.DismissKeyboard();
            app.Tap("Ok");
            app.Back();
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
        }

        /// <summary>
        /// Permet de vérifier si l'indiagram est bien supprimé
        /// Problème de fiabilité, cf summary de AddIndiagramTest
        /// </summary>
        [Test]
        public void b_DeleteIndiagramTest()
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
            app.Back();
            Assert.AreEqual(nbIndiagrams - 1, nbIndiagramsAfterDelete);
        }

        [Test]
        public void c_AddIndiagramInCategoryTest()
        {
            app.Tap("Collection management");
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            int nbIndiagrams = app.Query(c => c.Class("IndiagramView")).Length;
            app.Tap("Add");
            app.EnterText(c => c.Class("EditText"), "TestIndiagramInCategory");
            app.DismissKeyboard();

            app.Tap("Choose category");
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            var temp = app.Query(c => c.Class("IndiagramView"));
            app.Tap(temp[3].Id);
            app.Tap("button1");
            app.Tap("Ok");
            // Ajout terminé

            // On va maintenant vérifier le contenu de la catégorie
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            temp = app.Query(c => c.Class("IndiagramView"));
            app.Tap(temp[3].Id);
            app.Tap("button3");

            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            temp = app.Query(c => c.Class("IndiagramView"));
            Assert.IsTrue(temp.Length > 10); // Les 9 chiffres (il manque le 0) + l'indiagramview du button next

            /*app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            int nbIndiagramsAfterAdd = app.Query(c => c.Class("IndiagramView")).Length;
            //Debug.WriteLine("Add : " + nbIndiagrams + " -> " + nbIndiagramsAfterAdd);
            Assert.AreEqual(nbIndiagrams + 1, nbIndiagramsAfterAdd);*/
        }

        [Test]
        public void d_DeleteIndiagramInCategoryTest()
        {
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            int nbIndiagrams = app.Query(c => c.Class("IndiagramView")).Length;
            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp[temp.Length - 2]; // Le dernier element est la fleche "next", donc on prend celui d'avant
            app.Tap(ind.Id);
            app.Tap("Delete");
            app.Tap("Delete");

            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            int nbIndiagramsAfterDelete = app.Query(c => c.Class("IndiagramView")).Length;
            //Debug.WriteLine("Delete : " + nbIndiagrams + " -> " + nbIndiagramsAfterDelete);
            app.Back();
            Assert.AreEqual(nbIndiagrams - 1, nbIndiagramsAfterDelete);
        }

        /// <summary>
        /// Effectue automatiquement l'ajout d'un indiagram puis va le supprime via la page d'edit
        /// </summary>
        [Test]
        public void D_DeleteFromEdit()
        {
            //vu que l'indiagram a ete delete lors de l'execution de la methode C
            //creation d'un indiagram
            app.Tap("Add");
            app.EnterText(c => c.Class("EditText"), "TestIndiagram");
            app.DismissKeyboard();
            app.Tap("Ok");
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            
            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp[temp.Length - 2]; 
            app.Tap(ind.Id);
            app.Tap("See");
            app.Tap("Delete");
            app.Tap("Delete");
        }


        [Test]
        public void e_BrowseCategoryTest()
        {
            app.Tap("Collection management");
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));

            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp[1];
            app.Tap(ind.Id);

            app.WaitForElement(c => c.Id("buttonPanel"));
            app.Tap("button3");

            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            temp = app.Query(c => c.Class("IndiagramView"));
            if (temp.Length > 0)
                Assert.Pass();

        }


        /* 
        /// <summary>
        /// Permet de verifier que le reset des paramètres a bien été effectuer en 
        /// Puisque l'on a pas accès aux services impossible de le faire tourner
        /// </summary>
        [Test]
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

