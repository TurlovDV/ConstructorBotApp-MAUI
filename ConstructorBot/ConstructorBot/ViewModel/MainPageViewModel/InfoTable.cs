using ConstructorBot.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.MainPageViewModel
{
    public class InfoTable : BaseViewModel
    {
        private DateTime onStartTiming;
        private string namingBot;
        private int countMessage;
        private int countUsers;
        private int pingInternet;


        //public DateTime OnStartTiming { get; set; }
        //public string NamingBot { get; set; }
        //public int CountMessage { get; set; }
        //public int CountUsers { get; set; }
        //public int PingInternet { get; set; }

        public int CountMessage
        {
            get => countMessage;
            set
            {
                countMessage = value;
                OnPropertyChanged();
            }
        }

        public int CountUsers
        {
            get => countUsers;
            set
            {
                countUsers = value;
                OnPropertyChanged();
            }
        }

        public int PingInternet
        {
            get => pingInternet;
            set
            {
                pingInternet = value;
                OnPropertyChanged();
            }
        }

        public DateTime OnStartTiming
        {
            get => onStartTiming;
            set
            {
                onStartTiming = value;
                OnPropertyChanged();
            }
        }

        public string NamingBot
        {
            get => namingBot;
            set
            {
                namingBot = value;
                OnPropertyChanged();
            }
        }

    }
}
