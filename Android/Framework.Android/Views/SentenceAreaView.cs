﻿using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Java.Util;
using Storm.Mvvm.Inject;

namespace IndiaRose.Framework.Views
{

    public class SentenceArea : RelativeLayout, View.IOnTouchListener
    {
        private readonly object _lock = new object();

        protected List<IndiagramView> ToPlayView = new List<IndiagramView>();

        protected ITextToSpeechService TextToSpeechService
        {
            get { return LazyResolver<ITextToSpeechService>.Service; }
        }

        protected ISettingsService SettingsService
        {
            get { return LazyResolver<ISettingsService>.Service; }
        }

        protected int ActId;
        protected int ReadingIndex;
        protected int MaxNumberOfIndiagram;
        protected bool IsReading { get; set; }
        protected Timer MDelayReadingTimer = new Timer();


        public SentenceArea(Context context)
            : base(context)
        {
            Initialize();
        }

        public SentenceArea(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public SentenceArea(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public void Initialize()
        {
            Id = 0x2A;
            ActId = Id;
            MaxNumberOfIndiagram = Width / IndiagramView.DefaultWidth - 1;

            SetOnTouchListener(this);

            // Init play button
            LayoutParams lp = new LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);
            lp.AddRule(LayoutRules.AlignParentRight);
            lp.AddRule(LayoutRules.CenterVertical);

            /*try
            {
                IndiagramView view = AppData.playButtonIndiagram.getView();
                this.m_layout.AddView(view, lp);

                Mapper.connect(view, "touchEvent", this, "PlayButtonEvent");
            }
            catch (Mapper e)
            {
                Log.Wtf("IndiagramBrowser", e);
            }*/
        }

        public bool CanAddIndiagram()
        {
            return (ToPlayView.Count < MaxNumberOfIndiagram && !IsReading);
        }

        public bool Add(IndiagramView view)
        {
            if (CanAddIndiagram())
            {
                lock (_lock)
                {
                    view.Id = ActId++;
                    ToPlayView.Add(view);
                    Post(RefreshLayout);
                }
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
            else
            {
                return false;
            }
        }

        public void Remove(IndiagramView view)
        {
            if (!IsReading && ToPlayView.Count > 0)
            {
                RemoveIndiagram(view);
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
            IndiagramView[] views = ToPlayView.ToArray();
            foreach (IndiagramView view in views)
            {
                RemoveIndiagram(view);
            }

            ToPlayView.Clear();
            ActId = Id;
        }

        protected void RemoveIndiagram(IndiagramView view)
        {
            lock (_lock)
            {
                RemoveView(view);
                ToPlayView.Remove(view);

                Post(RefreshLayout);
            }

            /*try
            {
                Mapper.disconnect(_view);
                Mapper.emit(this, "indiagramRemoved", _view);
            }
            catch (MapperException e)
            {
                Log.Wtf("PhraseArea", e);
            }*/
        }

        public bool HasIndiagram(Indiagram item)
        {
            lock (_lock)
            {
                for (int i = 0; i < ToPlayView.Count; ++i)
                {
                    if (ToPlayView[i].Indiagram.Equals(item))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public List<Indiagram> GetIndiagramsList()
        {
            lock (_lock)
            {
                List<Indiagram> result = new List<Indiagram>();
                ToPlayView.ForEach(x=>result.Add(x.Indiagram));

                return result;
            }
        }

        protected void RefreshLayout()
        {
            // TODO Normalement Bon
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
        }

        private void PlayButtonEvent(IndiagramView view, MotionEvent _event /*,EventResult _result*/)
        {
            throw new NotImplementedException();
            /*try
            {
                Mapper.emit(this, "PlayButtonEvent", _view, _event, _result);
            }
            catch (Exception e)
            {
                Log.Wtf("SentenceArea", e);
            }*/
        }

        private void IndiagramEvent(IndiagramView _view, MotionEvent _event /*,EventResult _result*/)
        {
            throw new NotImplementedException();
            /*
            try
            {
                Mapper.emit(this, "IndiagramEvent", _view, _event, _result);
            }
            catch (MapperException e)
            {
                Log.Wtf("SentenceArea", e);
            }*/
        }

        bool IOnTouchListener.OnTouch(View _view, MotionEvent _event)
        {
            throw new NotImplementedException();
            /*
            Log.Wtf("SentenceArea", "onTouch layout !!");
            try
            {
                EventResult result = new EventResult();
                Mapper.emit(this, "layoutEvent", _view, _event, result);
                return result.eventResult;
            }
            catch (MapperException e)
            {
                Log.Wtf("SentenceArea", e);
            }
            return false;*/
        }

        public void Read()
        {
            if (!IsReading && ToPlayView.Count > 0)
            {
                lock (_lock)
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
                    }
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
                    if (ReadingIndex > 0
                        && SettingsService.IsReinforcerEnabled)
                    {
                        // disable reinforcer background on the last read indiagram.
                        //todo a voir c'est quoi le indiagrambackground
                        //ToPlayView[ReadingIndex - 1].setIndiagramBackground(Color.Transparent);
                    }
                    IndiagramView v = ToPlayView[ReadingIndex];
                    if (SettingsService.IsReinforcerEnabled)
                    {
                        //todo pareil
                        //v.setIndiagramBackground(SettingsService.ReinforcerColor);
                    }
                    TextToSpeechService.ReadText(v.Indiagram.Text);
                }
                else
                {
                    if (ToPlayView.Count > 0)
                    {
                        //idem
                        //ToPlayView[ToPlayView.Count - 1].setIndiagramBackground(Color.Transparent);
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