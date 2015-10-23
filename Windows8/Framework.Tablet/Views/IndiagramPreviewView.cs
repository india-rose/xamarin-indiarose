using System;
using System.Globalization;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace IndiaRose.Framework.Views
{
    class IndiagramPreviewView
    {
        private Canvas _reinforcerView;
        private Image _indiagramView;

        private int _indiagramSize;
        private Color _reinforcerColor;
        private bool _reinforcerEnabled;

        public int IndiagramSize
        {
            get { return _indiagramSize; }
            set
            {
                if (_indiagramSize != value)
                {
                    _indiagramSize = value;
                    RefreshSize();
                }
            }
        }

        private void RefreshSize()
        {
            throw new NotImplementedException();
        }

        public Color ReinforcerColor
        {
            get { return _reinforcerColor; }
            set
            {
                if (!Equals(_reinforcerColor, value))
                {
                    _reinforcerColor = value;
                    RefreshColor();
                }
            }
        }

        private void RefreshColor()
        {
            if (!ReinforcerEnabled)
            {
                _reinforcerView.Background = new SolidColorBrush(Colors.Transparent);
                
            }
            else
            {
                Invalidate();
            }
        }
            


        public Brush ColorToBrush(Color c) // color = "#E7E44D"
        {
            var color = c.ToString();

            color = color.Replace("#", "");
            if (color.Length == 6)
            {
                return new SolidColorBrush(ColorHelper.FromArgb(255,
                    byte.Parse(color.Substring(0, 2), NumberStyles.HexNumber),
                    byte.Parse(color.Substring(2, 2), NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), NumberStyles.HexNumber)));
            }
            else
            {
                return null;
            }
        }

        private void Post(Action action)
        {
            throw new NotImplementedException();
        }

        private void Invalidate()
        {
            throw new NotImplementedException();
        }

        public bool ReinforcerEnabled
        {
            get { return _reinforcerEnabled; }
            set
            {
                if (_reinforcerEnabled != value)
                {
                    _reinforcerEnabled = value;
                    RefreshColor();
                }
            }
        }
    }
}
