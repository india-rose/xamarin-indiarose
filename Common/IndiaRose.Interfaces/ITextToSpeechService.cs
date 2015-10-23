using System;
using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    public interface ITextToSpeechService
    {
	    event EventHandler SpeakingCompleted;
	    bool IsReading { get; }
	    void Close();
	    void PlayIndiagram(Indiagram indiagram);
    }
}
