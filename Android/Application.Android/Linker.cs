using Android.Views;
using Android.Widget;
using Storm.Mvvm.Components;

namespace IndiaRose.Application
{
	class Linker
	{
		public void Include(View v)
		{
			v.Click += delegate { };
			v.ContextMenuCreated += delegate { };
			v.Drag += delegate { };
			v.FocusChange += delegate { };
			v.GenericMotion += delegate { };
			v.LayoutChange += delegate { };
			v.LongClick += delegate { };
			v.SystemUiVisibilityChange += delegate { };
			v.Touch += delegate { };
			v.ViewAttachedToWindow += delegate { };
			v.ViewDetachedFromWindow += delegate { };

			v.Enabled = true;
		}

		public void Include(CheckBox v)
		{
			bool r = v.Checked;
			v.Checked = false;
			v.CheckedChange += delegate { };
		}

		public void Include(SeekBar v)
		{
			v.Progress = 0;
			int x = v.Progress;
			v.ProgressChanged += delegate { };
		}

		public void Include(EditText v) { v.TextChanged += delegate { }; }

		public void Include(TextView v) { v.Text = v.Text + ""; }

		public void Include(AbsListView v) { v.Adapter = null; }

		public void Include(AbsSpinner v) { v.Adapter = null; }

		public void Include(BindableSpinner v)
		{
			object a = v.CurrentItem;
			v.CurrentItem = null;
			v.CurrentItemChanged += delegate { };
		}

		public void Include(BindableListView v)
		{
			object a = v.CurrentItem;
			v.CurrentItem = null;
			v.CurrentItemChanged += delegate { };
		}
	}
}