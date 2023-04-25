using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace ConstructorBot.ViewModel.MainPageViewModel
{
    public class LogicUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ObservableCollection<SaveMessageItem> SaveAnswers { get; set; }

        //private bool isView { get; set; }
        //public bool IsView 
        //{
        //    get => isView;  
        //    set
        //    {
        //        isView = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //public void OnPropertyChanged([CallerMemberName] string prop = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        //}
    }
}
