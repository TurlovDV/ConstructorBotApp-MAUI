using Android.App;
using Android.Content;
using Android.Net.Wifi.Aware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConstructorBot;
using ConstructorBotCore.UserModel.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resource = ConstructorBot.Resource;

namespace ConstructorBot
{
    [Service(ForegroundServiceType = Android.Content.PM.ForegroundService.TypeDataSync)]
    public class ServiceDomainBot : Service, IServiceDomainBot
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            if (intent.Action == "START_SERVICE")
            {
                System.Diagnostics.Debug.WriteLine("Фоновая работа включена");
                RegisterNotification();
            }
            else if (intent.Action == "STOP_SERVICE")
            {
                System.Diagnostics.Debug.WriteLine("Фоновая работа выключена");
                StopForeground(true);
                StopSelfResult(startId);
            }
            return StartCommandResult.NotSticky;
        }

        public void Start()
        {
            Intent startService = new Intent(MainActivity.ActivityCurrent, typeof(ServiceDomainBot));
            startService.SetAction("START_SERVICE");
            MainActivity.ActivityCurrent.StartService(startService);
        }

        public void Stop()
        {
            Intent stopIntent = new Intent(MainActivity.ActivityCurrent, this.Class);
            stopIntent.SetAction("STOP_SERVICE");
            MainActivity.ActivityCurrent.StartService(stopIntent);
        }

        private void RegisterNotification()
        {
            NotificationChannel channel = new NotificationChannel("ServicioChannel", "Demo de servicio", NotificationImportance.Max);
            NotificationManager manager = (NotificationManager)MainActivity.ActivityCurrent.GetSystemService(Context.NotificationService);
            manager.CreateNotificationChannel(channel);
            Notification notification = new Notification.Builder(this, "ServicioChannel")
                .SetContentTitle("Бот включен")
                .SetSmallIcon(Resource.Drawable.constructor)
                //.SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha)
                .SetOngoing(true)
                .Build();

            StartForeground(100, notification);
        }
    }
}