using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    public interface ICopyPasteService
    {
        void Copy(Indiagram indiagram);
        void Paste(Indiagram indiagram);
    }
}
