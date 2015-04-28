using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
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
        private bool _redSquare;

        public bool RedSquare
        {
            get
            {
                return _redSquare;
            }
            set
            {
                _redSquare = value;
                RefreshImage();
            }
        }

        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                if (Equals(_sourcePath, value))
                {
                    return;
                }
                _sourcePath = value;
                if (value != null)
                {
                    RefreshImage();
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

            ViewGroup.LayoutParams top = LayoutParameters;
            top.Height = Size;
            top.Width = Size;
            Post(() =>
            {
                LayoutParameters = top;
            });

            if (SourcePath != null)
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
                    return;
                }
            }
            if (RedSquare)
                Post(() => SetImageDrawable(new ColorDrawable(Color.Red)));
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