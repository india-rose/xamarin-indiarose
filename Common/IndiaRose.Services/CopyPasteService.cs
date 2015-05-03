
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm;

namespace IndiaRose.Services
{
    public class CopyPasteService : NotifierBase,ICopyPasteService
    {
        private Indiagram _indiagram;
        private bool _hasBuffer;
        public bool HasBuffer
        {
			get { return _hasBuffer; }
			set { SetProperty(ref _hasBuffer, value); }
        }

        public void Copy(Indiagram indiagram, bool isCategory)
        {
            _indiagram = isCategory ? new Category(indiagram) : new Indiagram(indiagram);
            HasBuffer = true;
        }

        public Indiagram Paste()
        {
	        if (HasBuffer)
	        {
				return _indiagram.IsCategory ? new Category(_indiagram) : new Indiagram(_indiagram);
	        }
	        return null;
        }
    }
}
