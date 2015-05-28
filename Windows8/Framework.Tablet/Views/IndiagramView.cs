//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.ApplicationModel.Appointments.AppointmentsProvider;
//using Windows.Foundation;
//using Windows.Security.ExchangeActiveSyncProvisioning;
//using Windows.UI;
//using Windows.UI.StartScreen;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Automation.Peers;
//using Windows.UI.Xaml.Controls;
//using Windows.UI.Xaml.Media;
//using Windows.UI.Xaml.Media.Imaging;
//using Windows.UI.Xaml.Shapes;
//using IndiaRose.Data.Model;
//using IndiaRose.Framework.Helper;
//using IndiaRose.Interfaces;
//using Storm.Mvvm.Inject;
//using Storm.Mvvm.Services;

//namespace IndiaRose.Framework.Views
//{
//	public class IndiagramView : Control
//	{
//		//fait avec les conseilles de julien
//		#region Fields
//		/**
//		 * Paint object to draw the image.
//		 */
//		private readonly Canvas _picturePainter = new Canvas();
//		/**
//		 * Paint object to write the text.
//		 */
//		private readonly Canvas _textPainter = new Canvas();
//		/**
//		 * Paint object to draw background.
//		 */
//		private readonly Canvas _backgroundPainter = new Canvas();
//		/**
//		 * Rect of the view to draw background.
//		 */
//		private Rect _backgroundRect;
//		/**
//		 * Indiagram relative to this view.
//		 */
//		private Indiagram _indiagram;
//		/**
//		 * Current width of the view.
//		 */
//		private int _width;
//		/**
//		 * Current height of the view.
//		 */
//		private int _height;
//		/**
//		 * width of the image.
//		 */
//		private int _pictureWidth;
//		/**
//		 * height of the image.
//		 */
//		private int _pictureHeight;
//		/**
//		 * Left margin from left of the view to the image left edge.
//		 */
//		private int _marginLeft;
//		/**
//		 * Top margin from top of the view to the image top edge.
//		 */
//		private int _marginTop;
//		/**
//		 * Color of the text.
//		 */
//		private uint _textColor;
//		/**
//		 * Boolean to know if the text fill on one line or more.
//		 */
//		private bool _isOneLineText = true;

//		private uint _backgroundColor;
//		#endregion
//		// fait
//		#region Services

//		protected IDispatcherService DispatcherService
//		{
//			get { return LazyResolver<IDispatcherService>.Service; }
//		}

//		protected static ISettingsService SettingsService
//		{
//			get { return LazyResolver<ISettingsService>.Service; }
//		}

//		#endregion

//		#region Properties

//		public Indiagram Indiagram
//		{
//			get { return _indiagram; }
//			set
//			{
//				if (!Equals(_indiagram, value))
//				{
//					_indiagram = value;
//					RefreshDimension();
//				}
//			}
//		}

//		public uint TextColor
//		{
//			get { return _textColor; }
//			set
//			{
//				if (_textColor != value)
//				{
//					_textColor = value;
//					_textPainter.SetValue(TextBlock.ForegroundProperty, value);
//				}
//			}
//		}

//		public uint BackgroundColor
//		{
//			get { return _backgroundColor; }
//			set
//			{
//				if (_backgroundColor != value)
//				{
//					_backgroundColor = value;
//					SolidColorBrush colorBrush = new SolidColorBrush(_backgroundColor.ToColor());
//					_backgroundPainter.Background = colorBrush;
//				}
//			}
//		}

//		public int RealHeight
//		{
//			get { return _height; }
//		}

//		#endregion

//		// todo constructeur

//		private void Initialize()
//		{
//			_picturePainter.SetValue(Image.StyleProperty, Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));
//			_textPainter.SetValue(TextBlock.FontStyleProperty,SettingsService.FontName);
//			_textPainter.SetValue(TextBlock.ForegroundProperty, _textColor.ToColor());
//			//h
//			_textPainter.SetValue(TextBlock.FontSizeProperty,SettingsService.FontSize);
//			_textPainter.SetValue(TextBlock.HorizontalAlignmentProperty,Stretch);
//			_backgroundPainter.Background = new SolidColorBrush(Colors.Transparent);
//			_pictureWidth = _pictureHeight = SettingsService.IndiagramDisplaySize;
//		}

//		protected void RefreshDimension()
//		{
//			_textPainter.Measure(new Size(0,0));
//			_textPainter.Arrange(new Rect(0,0,0,0));
//			float textWidth = (float) _textPainter.ActualWidth; 
//			int textHeight = (int) _textPainter.ActualHeight;

//			if (string.IsNullOrEmpty(_indiagram.Text))
//			{
//				textHeight = 0;
//			}

//			_marginLeft = _pictureWidth / 10;
//			_marginTop = _pictureHeight / 10;

//			_width = _marginLeft * 2 + _pictureWidth;
//			//height take account the fact that some words will needs two or more lines to be displayed.
//			_height = _marginTop * 2 + _pictureHeight + (textHeight * (int)(textWidth / (_pictureWidth + 1) + 1));

//			SetMinimumWidth(_width);
//			SetMinimumHeight(_height);
//			Measure(new Size(_width, _height));

//			_isOneLineText = (textWidth <= _pictureWidth);
//			_backgroundRect = new Rect(0, 0, _width, _height);
//		}

//		protected void OnDraw(Canvas canvas)
//		{
//			base.OnDraw(canvas);
//			canvas.DrawRect(_backgroundRect, _backgroundPainter);

//			//draw picture or a red rectangle if error with picture
//			try
//			{
//				BitmapImage image = ImageHelper.LoadImage(_indiagram.ImagePath, _pictureWidth, _pictureHeight);
//				canvas.DrawBitmap(image, _marginLeft, _marginTop, _picturePainter);
//			}
//			catch (Exception)
//			{/* 
//			  * cree un rect 
//			  * metre dans un rectgeo
//			  * le metre en enfant du canvas
//			  * afficher les enfant du canva
//			  */
//				Rect image = new Rect(_marginLeft, _marginTop, _pictureWidth + _marginLeft, _pictureHeight + _marginTop);
//				RectangleGeometry drawGeometry = new RectangleGeometry();
//				drawGeometry.Rect = image;
//				canvas.Children.Add(drawGeometry);
//			}

//			if (!_indiagram.IsEnabled)
//			{
//				canvas.DrawRect(_marginLeft, _marginTop, _pictureWidth + _marginLeft, _pictureHeight + _marginTop, new SolidColorBrush(Color.FromArgb(0, 0, 0, 128)));
//			}

//			if (!string.IsNullOrEmpty(_indiagram.Text))
//			{
//				//write text
//				int yindex = _marginTop + _pictureHeight + SettingsService.FontSize;
//				int xindex = _marginLeft + _pictureWidth / 2;
//				String text = _indiagram.Text;

//				if (_isOneLineText)
//				{
//					canvas.DrawText(text, xindex, yindex, _textPainter);
//				}
//				else
//				{
//					int txtOffset = 0;

//					while (txtOffset < text.Length)
//					{
//						int textSize = _textPainter.BreakText(text, txtOffset, text.Length, true, _pictureWidth, null);
//						string text2 = text.Substring(txtOffset, textSize);

//						canvas.DrawText(text2, xindex, yindex, _textPainter);

//						yindex += SettingsService.FontSize;
//						txtOffset += textSize;
//					}
//				}
//			}
//		}

//		// rien a faire
//		public static int DefaultWidth
//		{
//			get
//			{
//				return (int)(SettingsService.IndiagramDisplaySize * 1.2);
//			}
//		}
//		// rien a fair
//		public static int DefaultHeight
//		{
//			get
//			{
//				return (int)(SettingsService.IndiagramDisplaySize * 1.2 + SettingsService.FontSize);
//			}
//		}
//	}
//}

