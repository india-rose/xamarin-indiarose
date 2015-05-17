using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using IndiaRose.Framework.Converters;
using IndiaRose.Interfaces;
using Java.Util;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{

    public class SentenceAreaView : RelativeLayout
    {
        private readonly object _lock = new object();

        public event EventHandler ListChanged;

        private List<IndiagramView> _toPlayView;
        public List<IndiagramView> ToPlayView
        {
            get { return _toPlayView; }
            set
            {
                value.ForEach(x =>
                {
                    x.Id = ActId++;
                    x.Touch += Remove;
                });
                _toPlayView = value;
                Post(RefreshLayout);
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
        protected int ReadingIndex;
        protected int MaxNumberOfIndiagram;
        protected bool IsReading { get; set; }
        protected Timer MDelayReadingTimer = new Timer();
        private IndiagramView _playButton;


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

        public void Initialize()
        {
            _toPlayView = new List<IndiagramView>();
            Id = 0x2A;
            ActId = Id;
            MaxNumberOfIndiagram = Width / IndiagramView.DefaultWidth - 1;

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
                Post(RefreshLayout);
                /*try
            {
                Mapper.emit(this, "indiagramAdded", _view);
                Mapper.connect(_view, "touchEvent", this, "indiagramEvent");
            }
            catch (MapperException e)
            {
                Log.Wtf("PhraseArea", e);
            }*/
                return true;
            }
            return false;
        }

        public void Remove(object sender, TouchEventArgs e)
        {

            if (sender != null && !IsReading && ToPlayView.Count > 0)
            {
                RemoveIndiagram((IndiagramView)sender);
                this.RaiseEvent(ListChanged);
            }
        }

        public void RemoveAll()
        {
            if (!IsReading && ToPlayView.Count > 0)
            {
                Post(RemoveAllHandler);
            }
        }

        protected void RemoveAllHandler()
        {
            ToPlayView.ForEach(RemoveIndiagram);
            ActId = Id;
            this.RaiseEvent(ListChanged);
        }

        protected void RemoveIndiagram(IndiagramView view)
        {
            RemoveView(view);
            ToPlayView.Remove(view);

            Post(RefreshLayout);
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
                Post(Invalidate);
            }
        }
        public void Read(object sender, TouchEventArgs touchEventArgs)
        {
            if (!IsReading && ToPlayView.Count > 0)
            {
                // if the reading process is not already launch and there is at
                // least one indiagram in the sentence.
                if (!IsReading && ToPlayView.Count > 0)
                {
                    ReadingIndex = 0;
                    IsReading = true;

                    /*try
                    {
                        Mapper.emit(this, "startReading");
                        Mapper.connect(m_voiceEngine, "readingComplete",
                            this, "endReading");
                    }
                    catch (MapperException e)
                    {
                        Log.Wtf("SentenceArea", e);
                    }*/

                    ReadSentence();
                    Post(RemoveAll);
                }
            }
        }

        protected void ReadSentence()
        {
            //if (IsReading)
            while (IsReading)
            {
                // if there is more view to read.
                if (ReadingIndex < ToPlayView.Count)
                {
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
                    }
                    if (v.Indiagram.HasCustomSound)
                        MediaService.PlaySound(v.Indiagram.SoundPath);
                    else
                        TextToSpeechService.ReadText(v.Indiagram.Text);
                    TextToSpeechService.Silence((long)(SettingsService.TimeOfSilenceBetweenWords*1000));
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
                    /*try
                    {
                        Mapper.disconnect(m_voiceEngine, "readingComplete",
                            this, "endReading");
                        Mapper.emit(this, "completeReading");
                    }
                    catch (MapperException e)
                    {
                        Log.Wtf("SentenceArea", e);
                    }*/
                }
            }
        }

        public void EndReading(Indiagram indiagram)
        {
            /*
        long _value;
        if ((long) AppData.settings.wordsReadingDelay < (long) 0.6)
        {
            _value = (long) (0.6*1000);
        }
        else
        {
            _value = (long) AppData.settings.wordsReadingDelay*1000;
        }
        if (m_isReading)
        {
            m_delayReadingTimer.Cancel();
            m_delayReadingTimer.Purge();
            m_delayReadingTimer = new Timer();
            m_delayReadingTimer.Schedule(new TimerTask()
            {
                
            public void run() {
                m_readingIndex++;
                readSentence();
            }
        }
        ,
            _value)
            ;
        }*/
        }
    }
}