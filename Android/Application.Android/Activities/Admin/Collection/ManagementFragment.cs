using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Storm.Mvvm;

namespace IndiaRose.Application.Activities.Admin.Collection
{
    public partial class ManagementFragment : FragmentBase
    {
        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Views_Admin_Collection_ManagementPage, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
            return Container.Locator.AdminManagementViewModel;
        }
    }
}