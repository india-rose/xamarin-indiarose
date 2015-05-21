//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Foundation;
//using Windows.UI;
//using Windows.UI.Xaml.Automation.Peers;
//using Windows.UI.Xaml.Media;
//using IndiaRose.Data.Model;

//namespace IndiaRose.Framework.Views
//{
//	class IndiagramView
//	{
//		#region Fields

//		/**
//		 * Paint object to draw the image.
//		 */
//		private readonly Paint _picturePainter = new Paint();
//		/**
//		 * Paint object to write the text.
//		 */
//		private readonly Paint _textPainter = new Paint();
//		/**
//		 * Paint object to draw background.
//		 */
//		private readonly Paint _backgroundPainter = new Paint();
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
//	}
//}
