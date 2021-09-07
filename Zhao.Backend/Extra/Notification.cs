using System.Timers;

namespace Zhao.Backend.Extra
{
    public static class Notification
    {
        private static string _message = "";
        private static Timer _timer;

        public static string Message
        {
            get => _message;
            set
            {
                _timer = new Timer(5000);
                _timer.Elapsed += OnTimeEvent;
                _message = value;
                _timer.Enabled = true;
            }
        }

        private static void OnTimeEvent(object sender, ElapsedEventArgs e)
        {
            _message = "";
            _timer.Stop();
            _timer.Dispose();
        }
    }
}