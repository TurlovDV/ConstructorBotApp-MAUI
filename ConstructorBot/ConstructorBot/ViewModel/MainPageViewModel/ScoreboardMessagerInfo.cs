using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.MainPageViewModel
{
    public class ScoreboardMessagerInfo : INotifyPropertyChanged
    {
        double messageCount;
        double profileCount;
        double ping;
        DateTime time;

        public double MessageCount
        {
            get => messageCount;
            set
            {
                messageCount = value;
                OnPropertyChanged();
            }
        }

        public double ProfileCount
        {
            get => profileCount;
            set
            {
                profileCount = value;
                OnPropertyChanged();
            }
        }

        public double Ping
        {
            get => ping;
            set
            {
                ping = value;
                OnPropertyChanged();
            }
        }

        public DateTime Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
