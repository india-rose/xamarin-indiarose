using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndiaRose.Data.Model;
using IndiaRose.Interfaces;
using Storm.Mvvm.Events;
using Storm.Mvvm.Inject;

namespace IndiaRose.Business.ViewModels.User
{
    public class UserHomeViewModel : AbstractBrowserViewModel
    {
        protected ITextToSpeechService TtsService
        {
            get { return LazyResolver<ITextToSpeechService>.Service; }
        }
        private List<Indiagram> _toplayList; 
        public string BotBackgroundColor
        {
            get { return SettingsService.BottomBackgroundColor; }
        }

        public List<Indiagram> ToPlayedList
        {
            get { return _toplayList; }
            set { SetProperty(ref _toplayList, value); }
        }

        public event EventHandler ListChanged;

        public UserHomeViewModel()
        {
            ToPlayedList=new List<Indiagram>();
        }
        protected override void IndiagramSelectedAction(Indiagram indiagram)
        {
            TtsService.ReadText(indiagram.Text);
            if (indiagram.IsCategory)
            {
                PushCategory((Category) indiagram);
            }
            else
            {
                ToPlayedList.Add(indiagram);
                this.RaiseEvent(ListChanged);
                if(SettingsService.IsBackHomeAfterSelectionEnabled)
                    while (PopCategory()){}
            }
        }
        
    }
}
