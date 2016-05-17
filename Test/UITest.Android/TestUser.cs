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

        /// <summary>
        /// Rentre dans la categorie objet puis divers 
        /// et utilise le bouton retour pour retourner a l'acceuil
        /// Pas opti car on utilise les coordonees voir si on peut pas recupper les indiagramViews
        /// </summary>
        [Test]
        public void A_ButtonBackTest()
        {
            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp.First();
            app.Tap(ind.Id);
            app.TapCoordinates(1250, 30);
        }

        /// <summary>
        /// ajoute un indiagram dans la sentenceAreaView
        /// </summary>
        [Test]
        public void B_AddIndiaInSentence()
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

    }
}
