using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndiaRose.Business.ViewModels.User
{
    public class UserHomeViewModel : AbstractBrowserViewModel
    {
        public string BotBackgroundColor
        {
            get { return SettingsService.BottomBackgroundColor; }
        }
    }
}
