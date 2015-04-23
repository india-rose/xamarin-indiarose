using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using IndiaRose.Framework.Converters;
using IndiaRose.Framework.Views;
using IndiaRose.Interfaces;
using Java.Lang;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Inject;

namespace IndiaRose.Application.Activities.Admin.Collection
{
    public partial class ManagementFragment : FragmentBase
    {

		private int _collectionOffset;
		private int _displayCount;
		private List<Indiagram> _displayed;
        private uint _textColor;

        [Binding("TextColor", Converter = typeof(ColorContainerToIntConverter))]
        public uint TextColor
        {
            get { return _textColor;}
            set { SetProperty(ref _textColor, value); }
        }

		[Binding("CollectionOffset")]
		public int CollectionOffset
		{
			get { return _collectionOffset; }
			set
			{
				if (SetProperty(ref _collectionOffset, value))
				{
					Display();
				}
			}
		}

		[Binding("DisplayCount", Mode = BindingMode.TwoWay)]
		public int DisplayCount
		{
			get { return _displayCount; }
			set { SetProperty(ref _displayCount, value); }
		}

		[Binding("Displayed")]
		public List<Indiagram> Displayed
		{
			get { return _displayed; }
			set
		    {
			    if (SetProperty(ref _displayed, value) && value != null)
			    {
				    Display();
			    }
		    }
		}

        protected override View CreateView(LayoutInflater inflater, ViewGroup container)
        {
            return inflater.Inflate(Resource.Layout.Views_Admin_Collection_ManagementPage, container, false);
        }

        protected override ViewModelBase CreateViewModel()
        {
			CollectionLayout.LayoutChange += CollectionLayout_LayoutChange;
            return Container.Locator.AdminManagementViewModel;
        }

		private void CollectionLayout_LayoutChange(object sender, View.LayoutChangeEventArgs e)
		{
			CollectionLayout.LayoutChange -= CollectionLayout_LayoutChange;
			Initialize();
			if (Displayed != null)
			{
				Display();
			}
		}

	    private int _indiagramByLine;
	    private int _numberOfLine;
	    private IndiagramView[,] _displayableViews;

	    private void Initialize()
	    {
			//TODO : check if width and height are correct
			_indiagramByLine = CollectionLayout.Width / IndiagramView.DefaultWidth;
			_numberOfLine = CollectionLayout.Height / IndiagramView.DefaultHeight;

			_displayableViews = new IndiagramView[_numberOfLine,_indiagramByLine];
	    }

	    //protected RelativeLayout CollectionLayout { get; set; }

		private void Display()
		{
			int startOffset = CollectionOffset;
			int count = 0;

			List<Indiagram> toDisplay = Displayed.Where((indiagram, indexL) => indexL >= startOffset).ToList();

			CollectionLayout.RemoveAllViews();
			int index = 0;
			int id = 42;
			for (int line = 0; line < _numberOfLine; ++line)
			{
				for (int column = 0; column < _indiagramByLine; ++column)
				{
					if (_displayableViews[line, column] != null)
					{
						_displayableViews[line, column].Touch -= IndiagramTouched;
					}

					if (index < toDisplay.Count)
					{
						var view = new IndiagramView(Activity)
						{
							Indiagram = toDisplay[index++],
							TextColor = TextColor, 
							Id = id++,
						};
						view.Touch += IndiagramTouched;
						_displayableViews[line, column] = view;
					}
					else
					{
						_displayableViews[line, column] = null;
					}
				}
			}

			int currentHeight = 0;
			bool stop = false;
			
			for (int line = 0; line < _numberOfLine; ++line)
			{
				int maxHeight = 0;
				for (int column = 0; column < _indiagramByLine; ++column)
				{
					if (_displayableViews[line, column] == null)
					{
						stop = true;
						break;
					}

					RelativeLayout.LayoutParams param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

					if (line == 0)
					{
						param.AddRule(LayoutRules.AlignParentTop);
					}
					else
					{
						param.AddRule(LayoutRules.Below, _displayableViews[line - 1, 0].Id);

					}
					if (column == 0)
					{
						param.AddRule(LayoutRules.AlignParentLeft);
					}
					else
					{
						param.AddRule(LayoutRules.RightOf, _displayableViews[line, column - 1].Id);

					}

					count++;
					CollectionLayout.AddView(_displayableViews[line, column], param);

					if (maxHeight < _displayableViews[line, column].RealHeight)
					{
						maxHeight = _displayableViews[line, column].RealHeight;
					}
				}
				
				if (maxHeight > 0)
				{
					currentHeight += maxHeight;

					if (currentHeight > CollectionLayout.Height)
					{
						stop = true;
						for (int column = 0; column < _indiagramByLine && _displayableViews[line, column] != null; ++column)
						{
							CollectionLayout.RemoveView(_displayableViews[line, column]);
							count--;
						}
					}
				}

				if (stop)
				{
					break;
				}
			}

			DisplayCount = count;
		}

	    private void IndiagramTouched(object sender, View.TouchEventArgs touchEventArgs)
	    {
		    throw new NotImplementedException();
	    }

	    // for the botton next

	}
}