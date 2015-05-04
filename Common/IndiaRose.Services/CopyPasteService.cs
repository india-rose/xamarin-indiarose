
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
            _indiagram = isCategory ? new Category() : new Indiagram();
			_indiagram.CopyFrom(indiagram, true);
            HasBuffer = true;
        }

        public Indiagram Paste()
        {
	        if (HasBuffer)
	        {
		        Indiagram result = _indiagram.IsCategory ? new Category() : new Indiagram();
		        result.CopyFrom(_indiagram, true);
		        return result;
	        }
	        return null;
        }
    }
}
