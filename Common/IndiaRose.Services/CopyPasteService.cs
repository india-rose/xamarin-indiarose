
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm;

namespace IndiaRose.Services
{
    public class CopyPasteService : NotifierBase,ICopyPasteService
    {
        private Indiagram _indiagram;
        private bool _indiaPresent;
        public bool IndiaPresent
        {
            get { return _indiaPresent; }
            set { SetProperty(ref _indiaPresent, value); }
        }

        public void Copy(Indiagram indiagram, bool categ)
        {
            _indiagram = categ ? new Category(indiagram) : new Indiagram(indiagram);
            IndiaPresent = true;
        }

        public Indiagram Paste()
        {
            return _indiagram;
        }
    }
}
