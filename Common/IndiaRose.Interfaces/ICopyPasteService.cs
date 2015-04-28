using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    public interface ICopyPasteService
    {
        void Copy(Indiagram indiagram, bool categ);
        Indiagram Paste();
    }
}
