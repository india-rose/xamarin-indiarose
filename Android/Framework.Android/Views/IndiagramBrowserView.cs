using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{
	public class IndiagramBrowserView : RelativeLayout
	{
		#region Private fields

		private int _columnCount;
		private int _lineCount;
		private Indiagram _hiddenIndiagram;
		private IndiagramView _backupView;
		private IndiagramView[][] _displayableViews;
		public IndiagramView NextButton{ get; set; }

		#endregion

		#region Private backing properties fields

		private int _count;
		private int _offset;
		private List<Indiagram> _indiagrams;
		private uint _textColor;
		private ICommand _indiagramSelected;
		private ICommand _nextCommand;

		#endregion

		#region Public events

		public event EventHandler CountChanged;

		#endregion

		#region Public properties

		public int Count
		{
			get { return _count; }
			set { SetProperty(ref _count, value, () => CountChanged); }
		}

		public int Offset
		{
			get { return _offset; }
			set
			{
				if (SetProperty(ref _offset, value))
				{
					RefreshDisplay();
				}
			}
		}

		public List<Indiagram> Indiagrams
		{
			get { return _indiagrams; }
			set
			{
				if (SetProperty(ref _indiagrams, value))
				{
					RefreshDisplay();
				}
			}
		}

		public uint TextColor
		{
			get { return _textColor; }
			set
			{
				if (SetProperty(ref _textColor, value))
				{
					RefreshTextColor();
				}
			}
		}

		public ICommand IndiagramSelected
		{
			get { return _indiagramSelected; }
			set { SetProperty(ref _indiagramSelected, value); }
		}

		public ICommand NextCommand
		{
			get { return _nextCommand; }
			set { SetProperty(ref _nextCommand, value); }
		}

		public ICommand IndiagramViewSelectedCommand { get; set; }

		#endregion

		#region Constructors

		protected IndiagramBrowserView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Initialize();
		}

		public IndiagramBrowserView(Context context) : base(context)
		{
			Initialize();
		}

		public IndiagramBrowserView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();
		}

		public IndiagramBrowserView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Initialize();
		}

		private void Initialize()
		{
		    string path = LazyResolver<IStorageService>.Service.ImageNextArrowPath;
			NextButton = new IndiagramView(Context)
			{
				TextColor = 0,
				Id = 0x20,
				Indiagram = new Indiagram()
				{
					Text = "next",
					ImagePath = path
				}
			};

			NextButton.Touch += OnNextTouch;
		}

		#endregion

		#region Private methods

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			if (Reset())
			{
				RefreshDisplay();
			}
		}

		private bool Reset()
		{
			// check if changed

			// calculate how many views we can put in there
			int newColumnCount = Width / IndiagramView.DefaultWidth;
			int newLineCount = Height / IndiagramView.DefaultHeight;

			if (newColumnCount != _columnCount || newLineCount != _lineCount)
			{
				_columnCount = newColumnCount;
				_lineCount = newLineCount;
			}
			else
			{
				return false;
			}

			// unregister from events on views and delete them
			if (_displayableViews != null)
			{
				foreach (IndiagramView[] views in _displayableViews.Where(x => x != null))
				{
					foreach (IndiagramView view in views.Where(x => x != null))
					{
						view.Touch -= OnIndiagramTouch;
					}
				}
				RemoveAllViews();
				_displayableViews = null;
			}

			// create array to store views and all needed views
			int viewId = 0x2a;
			_displayableViews = new IndiagramView[_lineCount][];
			for (int line = 0; line < _lineCount; ++line)
			{
				_displayableViews[line] = new IndiagramView[_columnCount - ((line == 0) ? 1 : 0)];
				for (int column = 0; column < _displayableViews[line].Length; ++column)
				{
					IndiagramView view = new IndiagramView(Context)
					{
						TextColor = TextColor,
						Id = viewId++,
					};
					view.Touch += OnIndiagramTouch;
					_displayableViews[line][column] = view;
				}
			}
			_backupView = new IndiagramView(Context)
			{
				TextColor = TextColor,
				Id = ++viewId,
			};

			return true;
		}

		private void RefreshDisplay()
		{
			if (Indiagrams == null || _displayableViews == null || _lineCount == 0 || _columnCount == 0)
			{
				return;
			}

			RemoveAllViews();

			List<Indiagram> toDisplay = Indiagrams.Where((o, i) => i >= Offset && !Indiagram.AreSameIndiagram(_hiddenIndiagram, o)).ToList();
			int displayCount = 0;
			int index = 0;
			int currentHeight = 0;
			bool stop = false;
			for (int line = 0; line < _lineCount; ++line)
			{
				int lineHeight = 0;
				int lineCount = 0;
				for (int column = 0; column < _displayableViews[line].Length; ++column)
				{
					if (index >= toDisplay.Count)
					{
						stop = true;
						break;
					}
					
					IndiagramView view = _displayableViews[line][column];
					view.Indiagram = toDisplay[index++];
					LayoutParams param = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

					if (line == 0)
					{
						param.AddRule(LayoutRules.AlignParentTop);
					}
					else
					{
						param.AddRule(LayoutRules.Below, _displayableViews[line - 1][0].Id);
					}
					if (column == 0)
					{
						param.AddRule(LayoutRules.AlignParentLeft);
					}
					else
					{
						param.AddRule(LayoutRules.RightOf, _displayableViews[line][column - 1].Id);
					}

					lineCount++;
					AddView(view, param);						

					if (lineHeight < view.RealHeight)
					{
						lineHeight = view.RealHeight;
					}
				}

				currentHeight += lineHeight;
				if (currentHeight > Height)
				{
					stop = true;
					for (int column = 0; column < lineCount; ++column)
					{
						RemoveView(_displayableViews[line][column]);
					}
				}
				else
				{
					displayCount += lineCount;
				}

				if (stop)
				{
					break;
				}
			}

			LayoutParams forbutton = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			forbutton.AddRule(LayoutRules.AlignParentRight);
			forbutton.AddRule(LayoutRules.AlignParentTop);
			AddView(NextButton, forbutton);

			Count = displayCount;

			Invalidate();
		}

		private void RefreshTextColor()
		{
			if (_displayableViews != null)
			{
				foreach (IndiagramView[] views in _displayableViews)
				{
					foreach (IndiagramView view in views)
					{
						view.TextColor = _textColor;
					}
				}
			}
		}

		private void OnIndiagramTouch(object sender, TouchEventArgs touchEventArgs)
		{
			IndiagramView senderView = sender as IndiagramView;

			if (senderView == null)
			{
				return;
			}

			if (touchEventArgs.Event.ActionMasked == MotionEventActions.Down)
			{
				Indiagram indiagram = senderView.Indiagram;

				ICommand command = IndiagramViewSelectedCommand;
				if (command != null && command.CanExecute(senderView))
				{
					command.Execute(senderView);
				}

				command = IndiagramSelected;
				if (command != null && command.CanExecute(indiagram))
				{
					command.Execute(indiagram);
				}
			}
		}

		private void OnNextTouch(object sender, TouchEventArgs touchEventArgs)
		{
			if (touchEventArgs.Event.ActionMasked == MotionEventActions.Down)
			{
				ICommand command = NextCommand;
				if (command != null && command.CanExecute(null))
				{
					command.Execute(null);
				}
			}
		}

		#endregion

		#region Private tools methods

		private bool SetProperty<T>(ref T storage, T value)
		{
			if (Equals(storage, value))
			{
				return false;
			}

			storage = value;
			return true;
		}

		private bool SetProperty<T>(ref T storage, T value, Func<EventHandler> eventGetter)
		{
			if (Equals(storage, value))
			{
				return false;
			}

			storage = value;

			EventHandler handler = eventGetter();
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
			return true;
		}

		#endregion

		#region drag and drop methods

		public void SwitchViewForDragAndDrop(IndiagramView view)
		{
			for (int i = 0; i < _lineCount; ++i)
			{
				for (int j = 0; j < _columnCount; ++j)
				{
					if (_displayableViews[i][j] == view)
					{
						_backupView.Touch += OnIndiagramTouch;
						view.Touch -= OnIndiagramTouch;

						_displayableViews[i][j] = _backupView;
						_backupView = view;
						RemoveView(view);
						return;
					}
				}
			}
		}

		public void HideIndiagram(Indiagram indiagram)
		{
			_hiddenIndiagram = indiagram;
		}

		public void ShowAllIndiagrams()
		{
			_hiddenIndiagram = null;
		}

		public void RefreshView()
		{
			RefreshDisplay();
		}

		#endregion
	}
}