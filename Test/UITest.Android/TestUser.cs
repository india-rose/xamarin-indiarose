using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;

namespace UITest.Android
{
    [TestFixture]
    public class TestsUser
    {
        AndroidApp app;

        public TestsUser()
        {
            // J'ai déplacé cette partie dans le constructeur pour éviter de relancer l'app à chaque test (c'est ce qui se passe quand ce code est dans BeforeEachTest())
            app = ConfigureApp
                .Android
                //.ApkFile ("../../../../Android/Application.Android/bin/Debug/org.indiarose.apk")
                //.LaunchableActivity("md5e9eb850b4bfdb148cd4a09650ab85c90.AdminSplashscreenActivity")
                .StartApp();
        }

       /* [Test]
        public void AA_Test()
        {
            app.Repl();
        }
        */
        /// <summary>
        /// Rentre dans la categorie objet puis divers 
        /// et utilise le bouton retour pour retourner a l'acceuil
        /// Pas opti car on utilise les coordonees voir si on peut pas recupper les indiagramViews
        /// </summary>
        [Test]
        public void ButtonBackTest()
        {
            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp.First();
            app.Tap(ind.Id);
            temp = app.Query(c => c.Class("ImageView"));
            app.Tap(temp[1].Id);
           // app.TapCoordinates(1250, 30);
        }

        [Test]
        public void B_EnterCorrectionError()
        {
            var temp = app.Query(c => c.Class("IndiagramView"));
            int nbIndView = temp.Length;
            var ind = temp[temp.Length - 1];
            app.DragCoordinates(ind.Rect.CenterX, ind.Rect.CenterY, temp[0].Rect.CenterX, ind.Rect.CenterY);
            int newNb = app.Query(c => c.Class("IndiagramView")).Length;

            Assert.IsTrue(newNb == nbIndView);
        }

        /// <summary>
        /// ajoute un indiagram dans la sentenceAreaView
        /// </summary>
        [Test]
        public void C_AddIndiaInSentence()
        {
            //rentre dans une categorie
            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp.First();
            app.Tap(ind.Id);

            //choisi un indiagram
            temp = app.Query(c => c.Class("IndiagramView"));
            ind = temp.First();
            app.Tap(ind.Id);
        }

        [Test]
        public void D_EnterCorrection()
        {
            var temp = app.Query(c => c.Class("IndiagramView"));
            int nbIndView = temp.Length;
            var ind = temp[temp.Length - 1];
            app.DragCoordinates(ind.Rect.CenterX, ind.Rect.CenterY, temp[0].Rect.CenterX, ind.Rect.CenterY);

            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            int newNb = app.Query(c => c.Class("IndiagramView")).Length;

            Debug.WriteLine(newNb + " " + nbIndView);
            Assert.IsTrue(newNb < nbIndView);
            //temp = app.Query(c => c.Class("ImageView"));
            //app.Tap(temp[1].Id);
        }
        
    }
}
