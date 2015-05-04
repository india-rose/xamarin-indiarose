using IndiaRose.Data.Model;
using Storm.Mvvm;

namespace IndiaRose.Data.UIModel
{
    public class IndiagramContainer : NotifierBase
    {
	    private static int _idCounter = 0;

	    public int Id = ++_idCounter;

        private Indiagram _indiagram;

        public Indiagram Indiagram
        {
            get { return _indiagram; }
            set { SetProperty(ref _indiagram, value); }
        }

        public IndiagramContainer(Indiagram indiagram)
        {
            Indiagram = indiagram;
        }
    }
}
