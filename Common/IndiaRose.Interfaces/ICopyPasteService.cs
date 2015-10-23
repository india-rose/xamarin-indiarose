using IndiaRose.Data.Model;

namespace IndiaRose.Interfaces
{
    public interface ICopyPasteService
    {
		bool HasBuffer { get; }

        void Copy(Indiagram indiagram, bool isCategory);

        Indiagram Paste();
    }
}
