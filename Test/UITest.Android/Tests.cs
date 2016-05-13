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
            Debug.WriteLine("Add : " + nbIndiagrams + " -> " + nbIndiagramsAfterAdd);
            Assert.AreEqual(nbIndiagrams + 1, nbIndiagramsAfterAdd);
        }

        [Test]
        public void DeleteIndiagramTest()
        {
            app.Tap("Collection management");
            app.WaitForElement(c => c.Class("IndiagramBrowserView"));

            int nbIndiagrams = app.Query(c => c.Class("IndiagramView")).Length;
            var temp = app.Query(c => c.Class("IndiagramView"));
            var ind = temp[temp.Length - 2]; // Le dernier element est la fleche "next", donc on prend celui d'avant
            app.Tap(ind.Id);
            app.Tap("Delete");
            app.Tap("Delete");

            app.WaitForElement(c => c.Class("IndiagramBrowserView"));
            int nbIndiagramsAfterDelete = app.Query(c => c.Class("IndiagramView")).Length;
            Debug.WriteLine("Delete : " + nbIndiagrams + " -> " + nbIndiagramsAfterDelete);
            Assert.AreEqual(nbIndiagrams - 1, nbIndiagramsAfterDelete);
        }
    }
}

