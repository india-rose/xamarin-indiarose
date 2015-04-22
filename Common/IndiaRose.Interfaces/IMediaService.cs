using System;

namespace IndiaRose.Interfaces
{
    public interface IMediaService
    {
        void StartWrite(string path);
        string StopWrite();
        void StartRead(string path);
        string StopRead(string data);
    }
}
