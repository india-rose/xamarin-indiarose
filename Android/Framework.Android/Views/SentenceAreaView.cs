using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using IndiaRose.Framework.Converters;
using IndiaRose.Interfaces;
using Java.Lang;
using Java.Util;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{

    public class SentenceAreaView : RelativeLayout
    {

        public event EventHandler ListChanged;
        public event EventHandler MaxNumberChanged;

        private List<IndiagramView> _toPlayView;
        public List<IndiagramView> ToPlayView
        {
            get { return _toPlayView; }
            set
            {
                if (!_changing)
                {
                    value.ForEach(x =>
                    {
                        x.Id = ActId++;
                        x.Touch += Remove;
                    });
                    _toPlayView = value;
                    RefreshLayout();
                    CanAdd = ToPlayView.Count < MaxNumberOfIndiagram;
                }
            }
        }

        protected ITextToSpeechService TextToSpeechService
        {
            get { return LazyResolver<ITextToSpeechService>.Service; }
        }
        protected ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }
        protected IMediaService MediaService
        {
            get { return LazyResolver<IMediaService>.Service; }
        }

        protected int ActId;
        private bool _changing;
        protected int ReadingIndex;
        public int MaxNumberOfIndiagram { get; set; }
        protected bool IsReading { get; set; }
        private IndiagramView _playButton;
		private bool _canAdd;

		public bool CanAdd { get
			{ return _canAdd; }
			set
			{
				_canAdd = value;
				this.RaiseEvent (MaxNumberChanged); 
			}
		}

        public SentenceAreaView(Context context)
            : base(context)
        {
            Initialize();
        }

        public SentenceAreaView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public SentenceAreaView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }
		public void RefreshMaxNumberofIndia(int width){
			MaxNumberOfIndiagram = width / IndiagramView.DefaultWidth - 1;
			CanAdd = ToPlayView.Count < MaxNumberOfIndiagram;
		}
        public void Initialize()
        {
            _toPlayView = new List<IndiagramView>();
            Id = 0x2A;
            ActId = Id;
            MaxNumberOfIndiagram = Width / IndiagramView.DefaultWidth - 1;
            CanAdd = true;

            //Init play button
            _playButton = new IndiagramView(Context)
            {
                TextColor = 0,
                Id = 0x30,
                Indiagram = new Indiagram()
                {
                    Text = "play",
                    ImagePath = LazyResolver<IStorageService>.Service.ImagePlayButtonPath
                }
            };
            _playButton.Touch += Read;
            var lp = new LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentRight);
            lp.AddRule(LayoutRules.CenterVertical);

            AddView(_playButton, lp);
        }
        public bool CanAddIndiagram()
        {
            return (ToPlayView.Count < MaxNumberOfIndiagram && !IsReading);
        }

        public bool Add(IndiagramView view)
        {
            if (CanAddIndiagram())
            {
                view.Id = ActId++;
                ToPlayView.Add(view);
                RefreshLayout();
                return true;
            }
            return false;
        }

        public void Remove(object sender, TouchEventArgs e)
        {
            if (e.Event.ActionMasked == MotionEventActions.Down)
            {
                if (sender != null && !IsReading && ToPlayView.Count > 0)
                {
                    _changing = true;
                    RemoveIndiagram((IndiagramView) sender);
                    this.RaiseEvent(ListChanged);
                    _changing = false;
                }
            }
        }

        public void RemoveAll()
        {
            if (!IsReading && ToPlayView.Count > 0)
            {
                RemoveAllHandler();
            }
        }

        protected void RemoveAllHandler()
        {
			ToPlayView.ForEach(RemoveView);
			ToPlayView.Clear ();
			RefreshLayout ();
            ActId = Id;
            this.RaiseEvent(ListChanged);
        }

        protected void RemoveIndiagram(IndiagramView view)
        {
            RemoveView(view);
            ToPlayView.Remove(view);

            RefreshLayout();
        }

        public bool HasIndiagram(Indiagram item)
        {
            return ToPlayView.Any(t => t.Indiagram.Equals(item));
        }

        public List<Indiagram> GetIndiagramsList()
        {
            List<Indiagram> result = new List<Indiagram>();
            ToPlayView.ForEach(x => result.Add(x.Indiagram));

            return result;
        }

        protected void RefreshLayout()
        {
            ToPlayView.ForEach(RemoveView);

            for (int i = 0; i < ToPlayView.Count; ++i)
            {
                LayoutParams lp = new LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);
                lp.AddRule(LayoutRules.CenterVertical);

                if (i > 0)
                {
                    lp.AddRule(LayoutRules.RightOf, ToPlayView[i - 1].Id);
                }
                else
                {
                    lp.AddRule(LayoutRules.AlignParentLeft);
                }

                AddView(ToPlayView[i], lp);
            }
			Post(Invalidate);
        }
        public void Read(object sender, TouchEventArgs touchEventArgs)
        {
            if (touchEventArgs.Event.ActionMasked == MotionEventActions.Down)
            {
                // if the reading process is not already launch and there is at
                // least one indiagram in the sentence.
                if (!IsReading && ToPlayView.Count > 0)
                {
                    ReadingIndex = 0;
                    IsReading = true;
                    ReadSentence();
                }
            }
        }

        protected void ReadSentence()
        {
            if (IsReading)
            {
                // if there is more view to read.
                if (ReadingIndex < ToPlayView.Count)
                {
                    if (ReadingIndex > 0)
                    {
                        TextToSpeechService.Silence((long)(SettingsService.TimeOfSilenceBetweenWords * 1000));
                    }
                    if (ReadingIndex > 0
                        && SettingsService.IsReinforcerEnabled)
                    {
                        //0 = Color.Transparent
                        // disable reinforcer background on the last read indiagram.
                        ToPlayView[ReadingIndex - 1].BackgroundColor = 0;
                    }
                    IndiagramView v = ToPlayView[ReadingIndex];
                    if (SettingsService.IsReinforcerEnabled)
                    {
                        var colorconverter = new ColorStringToIntConverter();
                        v.BackgroundColor = (uint)colorconverter.Convert(SettingsService.ReinforcerColor, null, null, null);
                        Post(Invalidate);
                    }
                    if (v.Indiagram.HasCustomSound)
                        MediaService.PlaySound(v.Indiagram.SoundPath, ReadSentence);
                    else
                    {
                        TextToSpeechService.ReadText(v.Indiagram.Text);
                        while (TextToSpeechService.IsSpeaking)
                        {
                            Thread.Yield();
                        }
                        ReadingIndex++;
                        ReadSentence();
                    }
                    ReadingIndex++;
                }
                else
                {
                    if (ToPlayView.Count > 0)
                    {
                        //0 = Color.Transparent
                        ToPlayView[ToPlayView.Count - 1].BackgroundColor = 0;
                    }
                    IsReading = false;
                    RemoveAll();
                }
            }
        }
    }
}