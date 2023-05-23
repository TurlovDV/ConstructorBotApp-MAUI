using ConstructorBot.SaveData;
using ConstructorBot.ViewModel.ConstructorPageViewModel;
using ConstructorBot.ViewModel.ConstructorPageViewModel.Action;
using ConstructorBot.ViewModel.MainPageViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace ConstructorBot;

public partial class ConstructorPage : ContentPage
{
    public ConstructorViewModel ConstructorViewModel { get; set; }

    public ConstructorPage()
	{
        ConstructorViewModel = new ConstructorViewModel();//ServiceProvider.GetService<ConstructorViewModel>();

        //ConstructorViewModel = ServiceProvider.GetService<ConstructorViewModel>();

        ConstructorViewModel.ActionBoxes = new ObservableCollection<ActionBox>();

        ServiceProvider.GetService<ConstructorViewModel>().ActionBoxes.ToList()
            .ForEach(x => ConstructorViewModel.ActionBoxes.Add(x));

        //ConstructorViewModel.ActionBoxes = new ObservableCollection<ActionBox>();

        InitializeComponent();

        BindingContext = ConstructorViewModel;

        MainMatrix.IsVisible = false;

        Disappearing += async (object sender, EventArgs e) =>
        {
            Storage.SaveActions(ConstructorViewModel.ActionBoxes.ToList());
            //SaveSettingOrActionBoxes.Save(ConstructorViewModel.ActionBoxes.ToList());
        };

        this.Loaded += Loading;
    }

    private void MoveActionBox_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var actionBox = (sender as Frame).BindingContext as ActionBox;
        if (!IsMove || actionBox.IsMainAction)
            return;
        switch (e.StatusType)
        {
            case GestureStatus.Running:
                actionBox.TranslationX += e.TotalX;
                actionBox.TranslationY += e.TotalY;

                //actionBox.OutConnect.ToList()
                //    .ForEach(x =>
                //        x.ConnectionActions.ToList()
                //        .ForEach(y => y.UpdateConnectionLine()));

                ConstructorViewModel.ActionBoxes.ToList()
                    .ForEach(x => x.ConnectionActions.ToList()
                    .ForEach(y =>
                     {
                         if (y.Connect.Id == actionBox.Id)
                             y.UpdateConnectionLine();
                     }));
                break;
        }
    }

    private void MoveAllActionBox_PanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (!IsMove)
            return;
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                ConstructorViewModel.ActionBoxes.ToList()
                    .ForEach(x =>
                    {
                        x.ConnectionActions.ToList()
                        .ForEach(x =>
                        {
                            x.Arrow._moveX = x.Arrow.TranslationX;
                            x.Arrow._moveY = x.Arrow.TranslationY;
                            x.Line._moveX = x.Line.TranslationX;
                            x.Line._moveY = x.Line.TranslationY;
                        });
                        x._moveX = x.TranslationX;
                        x._moveY = x.TranslationY;
                    });
                break;
            case GestureStatus.Running:
                ConstructorViewModel.ActionBoxes.ToList()
                    .ForEach(x =>
                    {
                        x.ConnectionActions.ToList()
                        .ForEach(x =>
                        {
                            x.Arrow.TranslationX = x.Arrow._moveX + e.TotalX;
                            x.Arrow.TranslationY = x.Arrow._moveY + e.TotalY;
                            x.Line.TranslationX = x.Line._moveX + e.TotalX;
                            x.Line.TranslationY = x.Line._moveY + e.TotalY;
                        });
                        x.BaseTranslation(x._moveX + e.TotalX, x._moveY + e.TotalY);
                        //x.TranslationX = x._moveX + e.TotalX;
                        //x.TranslationY = x._moveY + e.TotalY;
                    });
                break;
        }
    }

    double startScale = 1;
    double currentScale;
    double xOffset;
    double yOffset;
    bool IsMove = true;

    private const double MIN_SCALE = 1;
    private const double MAX_SCALE = 4;

    private void ZoomMatrix__PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        if (e.Status == GestureStatus.Started)
        {
            startScale = MainMatrix.Scale;
            MainMatrix.AnchorX = 0;
            MainMatrix.AnchorY = 0;

            new Task(() =>
            {
                IsMove = false;
            }).Start();
        }
        if (e.Status == GestureStatus.Running)
        {
            currentScale += (e.Scale - 1) * startScale;
            currentScale = Math.Max(MIN_SCALE, currentScale);
            currentScale = Math.Min(currentScale, MAX_SCALE);
            MainMatrix.Scale = currentScale;

            double renderedX = MainMatrix.X + xOffset;
            double deltaX = renderedX / Width;
            double deltaWidth = Width / (MainMatrix.Width * startScale);
            double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;
            double renderedY = MainMatrix.Y + yOffset;
            double deltaY = renderedY / Height;
            double deltaHeight = Height / (MainMatrix.Height * startScale);
            double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;
            double targetX = xOffset - (originX * MainMatrix.Width) * (currentScale - startScale);
            double targetY = yOffset - (originY * MainMatrix.Height) * (currentScale - startScale);
            MainMatrix.TranslationX = double.Clamp(targetX, -Content.Width * (currentScale - 1), 0);
            MainMatrix.TranslationY = double.Clamp(targetY, -Content.Height * (currentScale - 1), 0);

        }
        if (e.Status == GestureStatus.Completed)
        {
            new Task(async () =>
            {
                await Task.Delay(100);
                IsMove = true;
            }).Start();

            xOffset = MainMatrix.TranslationX;
            yOffset = MainMatrix.TranslationY;
        }
    }
   
    //private T Clamp<T>(T value, T minimum, T maximum) where T : IComparable
    //{
    //    if (value.CompareTo(minimum) < 0)
    //        return minimum;
    //    else if (value.CompareTo(maximum) > 0)
    //        return maximum;
    //    else
    //        return value;
    //}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        ServiceProvider.GetService<ConstructorViewModel>().ActionBoxes = ConstructorViewModel.ActionBoxes;
        await Shell.Current.GoToAsync("..");
        //await Navigation.PopAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    public void Loading(object sender, EventArgs e)
    {
        //var list = SaveSettingOrActionBoxes.Get();

        CommunityToolkit.Maui.Core.Platform.StatusBar.SetColor(Color.FromArgb("344B6D"));
        CommunityToolkit.Maui.Core.Platform.StatusBar.SetStyle(CommunityToolkit.Maui.Core.StatusBarStyle.LightContent);

        //ConstructorViewModel.ActionBoxes.Clear();
        //list.ToList().ForEach(x => ConstructorViewModel.ActionBoxes.Add(x));
        //ConstructorViewModel.ActionBoxes = ServiceProvider.GetService<ConstructorViewModel>().ActionBoxes;

        LoadingIndicator.IsRunning = false;
        MainMatrix.IsVisible = true;

        ConstructorViewModel.ActionBoxes.ToList().ForEach(x =>
        {
            x.TranslationX += 0.01;
            x.TranslationY += 0.01;
        });
    }
}