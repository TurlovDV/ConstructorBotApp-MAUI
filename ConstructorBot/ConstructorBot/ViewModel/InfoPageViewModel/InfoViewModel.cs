using ConstructorBot.Language;
using ConstructorBot.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConstructorBot.ViewModel.InfoPageViewModel
{
    internal class InfoViewModel : INotifyPropertyChanged
    {
        #region Properties

        private int position;
        private bool isViewButton;
        public bool IsViewButton
        {
            get => isViewButton;
            set
            {
                isViewButton = value;
                OnPropertyChanged();
            }
        }
        
        public ObservableCollection<IntroScreenModel> IntroScreens { get; set; } = new ObservableCollection<IntroScreenModel>();
        #endregion

        public InfoViewModel()
        {
            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info1"].ToString(),
                IntroImage = "info1",
                GridRow = 1,
                LayoutOptions = LayoutOptions.Center
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info2"].ToString(),
                IntroImage = "info2",
                GridRow = 1,
                LayoutOptions = LayoutOptions.Center
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info3"].ToString(),
                IntroImage = "info3",
                GridRow = 3,
                LayoutOptions = LayoutOptions.Start
            });
            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info4"].ToString(),
                IntroImage = "info4",
                GridRow = 3,
                LayoutOptions = LayoutOptions.Start
            });

            IntroScreens.Add(new IntroScreenModel
            {
                IntroTitle = LocalizationResourceManager.Instance["info5"].ToString(),
                IntroImage = "info5",
                GridRow = 1,
                LayoutOptions = LayoutOptions.Center
            });
        }

        public ICommand NextCommand => new Command(async () =>
        {
              await AppShell.Current.GoToAsync($"..");
        });

        public ICommand PositionChangedCommand => new Command(() =>
        {
            position++;
            if (position >= IntroScreens.Count - 1)
                IsViewButton = true;
        });

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
