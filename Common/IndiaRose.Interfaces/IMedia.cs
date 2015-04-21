namespace IndiaRose.Interfaces
{
    interface IMedia
    {
        void StartWrite(string path);
        string StopWrite();
        void StartRead(string path);
        string StopRead();
    }
}
