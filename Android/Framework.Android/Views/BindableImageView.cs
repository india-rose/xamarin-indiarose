using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IndiaRose.Framework.Views
{
    public class BindableImageView : ImageView
    {
        private string _sourcePath;
        private int _size;
        private bool _displayDefaultRedSquare = true;

        public bool DisplayDefaultRedSquare
        {
            get
            {
                return _displayDefaultRedSquare;
            }
            set
            {
	            if (value != _displayDefaultRedSquare)
	            {
		            _displayDefaultRedSquare = value;
		            RefreshImage();
	            }
            }
        }

        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                if (!Equals(_sourcePath, value))
                {
					_sourcePath = value;
					if (value != null)
					{
						RefreshImage();
					}
                }
            }
        }

        public int Size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    RefreshImage();
                }
            }
        }

        private void RefreshImage()
        {
            ViewGroup.LayoutParams parameters = LayoutParameters;
            parameters.Height = Size;
            parameters.Width = Size;
            Post(() =>
            {
                LayoutParameters = parameters;
            });

            if (SourcePath != null)
            {
	            try
	            {
		            Bitmap originalImage = BitmapFactory.DecodeFile(SourcePath);
		            if (originalImage != null)
		            {
			            Bitmap image = Bitmap.CreateScaledBitmap(originalImage, Size, Size, true);
			            Post(() =>
			            {
							SetImageBitmap(image);
							Invalidate();
			            });
			            return; //do not put default color
		            }
	            }
	            catch (Exception)
	            {
		            //TODO : log error
	            }
            }

			Post(() =>
			{
				SetImageDrawable(new ColorDrawable(DisplayDefaultRedSquare ? Color.Red : Color.Transparent));
				Invalidate();
			});
        }

        protected BindableImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public BindableImageView(Context context)
            : base(context)
        {
        }

        public BindableImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
        }

        public BindableImageView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

    }
}