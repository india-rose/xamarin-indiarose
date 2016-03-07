using IndiaRose.Data.Model;
using Storm.Mvvm;

namespace IndiaRose.Data.UIModel
{
    /// <summary>
    /// Classe contenant un Indiagram
    /// </summary>
    public class IndiagramContainer : NotifierBase
    {
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
