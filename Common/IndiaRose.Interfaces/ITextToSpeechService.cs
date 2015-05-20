using System;
using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    public interface ITextToSpeechService
    {
	    event EventHandler SpeakingCompleted;

        void Close();
	    void PlayIndiagram(Indiagram indiagram);
    }
}
