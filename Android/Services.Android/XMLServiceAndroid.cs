using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IndiaRose.Storage;

namespace IndiaRose.Services.Android
{
    public class XMLServiceAndroid : XmlService
    {
        public XMLServiceAndroid(Stream path) : base(path)
        {
        }
    }
}