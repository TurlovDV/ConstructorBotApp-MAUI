using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ConstructorBot.Model.Action;
using ConstructorBot.Services.PopupService;
using ConstructorBot.View.PopupConstructorPageView;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConstructorBot.ViewModel.PopupConstructorViewModel
{
    public partial class PopupPutBoxViewModel : ObservableObject
    {        
        [ObservableProperty]
        public ActionBox actionBoxTap;

        IMessageService messageService;
        
        public PopupPutBoxViewModel(ActionBox actionBoxTap)
        {
            messageService = ServiceProvider.GetService<IMessageService>();
            ActionBoxTap = actionBoxTap;
        }

        [RelayCommand]
        public async void CloseThisPopup()
        {
            await MopupService.Instance.PopAsync();
        }

        [RelayCommand]
        public async void AddMedia()
        {
            var file = await FilePicker.PickAsync(PickOptions.Images);

            if (file == null)
                return;

            if (new FileInfo(file.FullPath).Length / 1024 / 1024 > 10)
            {
                await messageService.ShowAsync("Ограничение", "Файлы более 10МБ добавить невозможно");
                return;
            }
            if (ActionBoxTap.MediaItems.Count > 5)
            {
                await messageService.ShowAsync("Ограничение", "Более 5 файлов добавить нельзя");
                return;
            }

            if (file.ContentType.Split('/')[0] == "image")
            {
                ActionBoxTap.MediaItems.Add(new MediaItem()
                {
                    Name = file.FileName,
                    Type = MediaType.Photo,
                    Source = file.FullPath,
                    PathMediaSource = file.FullPath,
                });
            }
        }

        [RelayCommand]
        public void RemoveMedia(object sender) 
        {
            ActionBoxTap.MediaItems.Remove(sender as MediaItem);
        }

        [RelayCommand]
        public async void OpenPhoto(object image)
        {
            var imageSource = (image as Image).Source;
            string pathImage = imageSource.ToString().Split(' ')[1];

            await MopupService.Instance.PushAsync(new PopupImageZoomView(pathImage));
        }

        [RelayCommand]
        public async void OpenPopupAddButtons()
        {
            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new PopupAddButtonsView(ActionBoxTap));
        }

        [RelayCommand]
        public async void OpenPopupPutRequest()
        {
            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PushAsync(new PopupPutRequestView(ActionBoxTap));
        }

        [RelayCommand]
        public void NotSaveRequest()
        {
            ActionBoxTap.NameSaveMessage = null;
        }
    }
}
