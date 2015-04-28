
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
{
    public class CopyPasteService : ICopyPasteService
    {
        private Indiagram _indiagram;

        public void Copy(Indiagram indiagram, bool categ)
        {
            _indiagram = categ ? new Category(indiagram) : new Indiagram(indiagram);
        }

        public Indiagram Paste()
        {
            return _indiagram;
        }
    }
}
