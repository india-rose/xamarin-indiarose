using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
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
                //.LaunchableActivity("md5e9eb850b4bfdb148cd4a09650ab85c90.AdminSplashscreenActivity")

                .StartApp();
        }

        /*[Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }*/

        [Test]
        public void DummyTest()
        {
            //Assert.AreEqual(1, 1);
            app.Repl();
        }
    }
}

