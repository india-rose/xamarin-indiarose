using System;

namespace IndiaRose.Interfaces
{
    public interface IMedia
    {
        void StartWrite(string path);
        string StopWrite();
        void StartRead(string path);
        string StopRead(string data);
    }
}
