using CommunityToolkit.Mvvm.ComponentModel;
using ConstructorBot.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.MainPageViewModel
{
    public partial class InfoTable : ObservableObject
    {
        [ObservableProperty]
        private DateTime onStartTiming;
        [ObservableProperty]
        private string namingBot;
        [ObservableProperty]
        private int countMessage;
        [ObservableProperty]
        private int countUsers;
        [ObservableProperty]
        private int pingInternet;
    }
}
