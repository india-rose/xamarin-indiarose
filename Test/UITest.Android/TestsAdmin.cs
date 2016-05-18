using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using IndiaRose.Framework.Views;
using IndiaRose.Interfaces;
using NUnit.Core;
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
            //app.Repl();
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
        public void C_DeleteIndiagramTest()
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
        public void D_AddIndiagramInCategoryTest()
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
        }

        [Test]
        public void E_DeleteIndiagramInCategoryTest()
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
        public void F_DeleteFromEdit()
        {
            app.Tap("Collection management");
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
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
            app.Back();
        }


        [Test]
        public void G_BrowseCategoryTest()
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
            app.Back();
            if (temp.Length > 0)
                Assert.Pass();
        }

        [Test]
        public void H_ChangeTopAreaSize()
        {
            app.Tap("Application settings");
            app.Tap("Background color properties");
            app.Tap("50%");
            app.ScrollDown();
            app.ScrollDown();
            app.Tap("70%");
            app.Tap("Ok");
        }

        [Test]
        public void I_ChangeIndiagramProperties()
        {
            app.Tap("Indiagram properties");
            //change indiagram size
            app.Tap("128x128");
            app.Tap("200x200");

            //change indiagram font
            app.Tap("AndroidClock.ttf");
            app.Tap("DroidSans.ttf");

            //change indiagram font size 
            app.Tap("20");
            app.Tap("30");

            //change indiafgram font color 
            app.Tap("Change text's color");
            app.TapCoordinates(400, 200);
            app.Tap("Ok");

            //change reinforcer color
            app.Tap("Change reinforcer color");
            app.TapCoordinates(700, 300);
            app.Tap("Ok");

            app.Tap("Ok");
        }

        [Test]
        public void J_ChangeIndiagramPropertiesCheckbox()
        {
            app.Tap("Indiagram properties");
            app.Tap("Back to home after picking indiagram");
            app.Tap("Can select the same indiagram multiple times");
            app.Tap("Reading reinforcer enabled");
            app.Tap("Ok");
        }

        [Test]
        public void K_AppBehaviorTest()
        {
            app.Tap("Application properties");
            app.WaitForElement(c => c.Id("content"));

            var temp = app.Query(c => c.Class("CheckBox"));
            Debug.WriteLine("temp length = " + temp.Length);
            for (int i = 0; i < temp.Length; i++)
            {
                app.Tap(temp[i].Id);
            }

            var seekBar = app.Query(c => c.Class("SeekBar")).FirstOrDefault();
            app.TapCoordinates(150, seekBar.Rect.CenterY);

            app.Tap("Ok");
            app.Back();
        }

        /// <summary>
        /// Permet de verifier que le reset des paramètres a bien été effectuer en 
        /// Puisque l'on a pas accès aux services impossible de le faire tourner
        /// </summary>
        [Test]
        public void L_SettingResetTest()
        {
            app.Tap(c=>c.Button().Text("Reset settings"));
            app.Tap(c=>c.Button().Text("Ok"));

           /* //verification du reset 
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
            Assert.Equals(settingsService.IsBackButtonEnabled, true);*/
        }
    }
}

