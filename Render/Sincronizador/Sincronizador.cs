using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System;
using System.Threading;
using Render.Resources.Helper;
namespace Render.Sincronizador
{
    [Service]
    public class Sincronizador : Service
    {
        private Sincronizacion s = new Sincronizacion();
        static readonly string TAG = "X:" + typeof(Sincronizador).Name;
        static readonly int TimerWait = Constantes.TiempoSincronizacion;
        Timer timer;
        DateTime startTime;
        bool isStarted = false;
        public override void OnCreate()
        {
            base.OnCreate();
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug(TAG, $"OnStartCommand called at {startTime}, flags={flags}, startid={startId}");
            if (isStarted)
            {
                TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
                Log.Debug(TAG, $"This service was already started, it's been running for {runtime:c}.");
            }
            else
            {
                startTime = DateTime.UtcNow;
                Log.Debug(TAG, $"Starting the service, at {startTime}.");
                timer = new Timer(HandleTimerCallback, startTime, 0, TimerWait);
                isStarted = true;
            }
            return StartCommandResult.NotSticky;
        }
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        public override void OnDestroy()
        {
            timer.Dispose();
            timer = null;
            isStarted = false;

            TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
            Log.Debug(TAG, $"Simple Service destroyed at {DateTime.UtcNow} after running for {runtime:c}.");
            base.OnDestroy();
        }
        void HandleTimerCallback(object state)
        {
            TimeSpan runTime = DateTime.UtcNow.Subtract(startTime);
            Log.Debug(TAG, $"This service has been running for {runTime:c} (since ${state}).");
            s.Sincronizar();
        }
    }
}