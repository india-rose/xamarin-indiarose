/*using System.Collections.Generic;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using IndiaRose.Data.Model;
using Java.Util;

namespace IndiaRose.Framework.Views
{

public class SentenceArea : View.IOnTouchListener
{

    protected RelativeLayout m_layout = null;

    protected List<IndiagramView> m_views = new List<IndiagramView>();

    protected SentenceAreaHandler m_handler = null;

    protected int m_id = 0x2A;

    protected int m_numberOfIndiagram = 0;

    // reading sentence relative variable
    protected VoiceReader m_voiceEngine;
    protected int m_readingIndex = 0;
    protected bool m_isReading = false;
    protected Timer m_delayReadingTimer = new Timer();

    public SentenceArea(RelativeLayout _layout, int _width,
        VoiceReader _voiceEngine)
    {
        m_layout = _layout;
        m_handler = new SentenceAreaHandler(this);

        m_numberOfIndiagram = _width/IndiagramView.DefaultWidth() - 1;
        m_voiceEngine = _voiceEngine;

        this.m_layout.SetOnTouchListener(this);

        // Init play button
        RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(
            RelativeLayout.LayoutParams.WrapContent,
            RelativeLayout.LayoutParams.WrapContent);
        lp.AddRule(RelativeLayout.ALIGN_PARENT_RIGHT);
        lp.AddRule(RelativeLayout.CENTER_VERTICAL);

        try
        {
            IndiagramView view = AppData.playButtonIndiagram.getView();
            this.m_layout.AddView(view, lp);

            Mapper.connect(view, "touchEvent", this, "PlayButtonEvent");
        }
        catch (MapperException e)
        {
            Log.Wtf("IndiagramBrowser", e);
        }
    }

    public bool CanAddIndiagram()
    {
        return (m_views.Count < m_numberOfIndiagram && !IsReading());
    }

    public bool IsReading()
    {
        return m_isReading;
    }

    public bool Add(IndiagramView _view)
    {
        if (CanAddIndiagram())
        {
            synchronized(this)
            {
                _view.Id = m_id++;
                m_views.Add(_view);
                m_handler.SendEmptyMessage(SentenceAreaHandler.REFRESH_MESSAGE);
            }
            try
            {
                Mapper.emit(this, "indiagramAdded", _view);
                Mapper.connect(_view, "touchEvent", this, "indiagramEvent");
            }
            catch (MapperException e)
            {
                Log.Wtf("PhraseArea", e);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Remove(IndiagramView _view)
    {
        if (!IsReading() && m_views.Count > 0)
        {
            RemoveIndiagram(_view);
        }
    }

    public void RemoveAll()
    {
        if (!IsReading() && m_views.Count > 0)
        {
            m_handler.SendEmptyMessage(SentenceAreaHandler.RESET_MESSAGE);
        }
    }

    protected void RemoveAllHandler()
    {
        IndiagramView[] views = m_views.ToArray();
        foreach (IndiagramView view in views)
        {
            RemoveIndiagram(view);
        }

        m_views.Clear();
        m_id = 0x2A;
    }

    protected void RemoveIndiagram(IndiagramView _view)
    {
        synchronized(this)
        {
            m_layout.RemoveView(_view);
            m_views.Remove(_view);

            m_handler.SendEmptyMessage(SentenceAreaHandler.REFRESH_MESSAGE);
        }

        try
        {
            Mapper.disconnect(_view);
            Mapper.emit(this, "indiagramRemoved", _view);
        }
        catch (MapperException e)
        {
            Log.Wtf("PhraseArea", e);
        }
    }

    public bool HasIndiagram(Indiagram _item)
    {
        synchronized(this)
        {
            for (int i = 0; i < m_views.Count; ++i)
            {
                if (m_views[i].Indiagram.Equals(_item))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public List<Indiagram> GetIndiagramsList()
    {
        synchronized(this)
        {
            List<Indiagram> result = new List<Indiagram>();
            foreach (IndiagramView i in m_views)
            {
                result.Add(i.Indiagram);
            }

            return result;
        }
    }

    protected void RefreshLayout()
    {
        Log.Error("Sentence Area", "Je raffraichi");

        // TODO Normalement Bon
        for (int i = 0; i < m_views.Count; ++i)
        {
            m_layout.RemoveView(m_views[i]);
        }

        for (int i = 0; i < m_views.Count; ++i)
        {
            RelativeLayout.LayoutParams lp = new RelativeLayout.LayoutParams(
                RelativeLayout.LayoutParams.WrapContent,
                RelativeLayout.LayoutParams.WrapContent);
            lp.AddRule(RelativeLayout.CENTER_VERTICAL);

            if (i > 0)
            {
                lp.AddRule(RelativeLayout.RIGHT_OF, m_views[i - 1].Id);
            }
            else
            {
                lp.AddRule(RelativeLayout.ALIGN_PARENT_LEFT);
            }

            m_layout.AddView(m_views[i], lp);
        }
    }

    public synchronized void PlayButtonEvent(IndiagramView _view,
        MotionEvent _event, EventResult _result)
    {
        try
        {
            Mapper.emit(this, "PlayButtonEvent", _view, _event, _result);
        }
        catch (MapperException e)
        {
            Log.Wtf("SentenceArea", e);
        }
    }

    public synchronized void IndiagramEvent(IndiagramView _view,
        MotionEvent _event, EventResult _result)
    {
        try
        {
            Mapper.emit(this, "IndiagramEvent", _view, _event, _result);
        }
        catch (MapperException e)
        {
            Log.Wtf("SentenceArea", e);
        }
    }

    public synchronized bool OnTouch(View _view, MotionEvent _event)
    {
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
        return false;
    }

    public void Read()
    {
        if (!m_isReading && m_views.Count > 0)
        {
            synchronized(this)
            {
                // if the reading process is not already launch and there is at
                // least one indiagram in the sentence.
                if (!m_isReading && m_views.Count > 0)
                {
                    m_readingIndex = 0;
                    m_isReading = true;

                    try
                    {
                        Mapper.emit(this, "startReading");
                        Mapper.connect(m_voiceEngine, "readingComplete",
                            this, "endReading");
                    }
                    catch (MapperException e)
                    {
                        Log.Wtf("SentenceArea", e);
                    }

                    ReadSentence();
                }
            }
        }
    }

    protected void ReadSentence()
    {
        Log.Error("Read", "readSentence");
        if (m_isReading)
        {
            // if there is more view to read.
            if (m_readingIndex < m_views.Count)
            {
                if (m_readingIndex > 0
                    && AppData.settings.enableReadingReinforcer)
                {
                    // disable reinforcer background on the last read indiagram.
                    m_views[m_readingIndex - 1]
                        .setIndiagramBackground(Color.Transparent);
                }
                IndiagramView v = m_views[m_readingIndex];
                if (AppData.settings.enableReadingReinforcer)
                {
                    v.setIndiagramBackground(AppData.settings.backgroundReinforcerReading);
                }
                m_voiceEngine.read(v.Indiagram);
            }
            else
            {
                if (m_views.Count > 0)
                {
                    m_views[m_views.Count - 1].setIndiagramBackground(Color.Transparent);
                }
                m_isReading = false;
                try
                {
                    Mapper.disconnect(m_voiceEngine, "readingComplete",
                        this, "endReading");
                    Mapper.emit(this, "completeReading");
                }
                catch (MapperException e)
                {
                    Log.Wtf("SentenceArea", e);
                }
            }
        }
    }

    public void EndReading(Indiagram _indiagram)
    {
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
        }
    }
}*/