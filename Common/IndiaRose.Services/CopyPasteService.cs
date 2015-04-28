
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;

namespace IndiaRose.Services
{
    public class CopyPasteService : ICopyPasteService
    {
        private Indiagram _indiagram;

        public void Copy(Indiagram indiagram)
        {
            _indiagram = indiagram.IsCategory ? new Category(indiagram) : new Indiagram(indiagram);
        }

        public void Paste(Indiagram indiagram)
        {
            indiagram.Edit(_indiagram);
        }
    }
}
