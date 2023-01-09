using MBRS_API_DEMO.Utils;

namespace MBRS_API.Models
{
    public class TimerManager
    {

        private Timer? _timer;
        private AutoResetEvent? _autoResetEvent;
        private Action? _action;
        public DateTime TimerStarted { get; set; }
        public bool IsTimerStarted { get; set; }
        public void PrepareTimer(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 1000, 5000);
            TimerStarted = Common.ConvertUTCDateTime();
            IsTimerStarted = true;
        }
        public void Execute(object? stateInfo)
        {
            //(DateTime.Now - TimerStarted).TotalSeconds > 180
            _action();
            if ((Common.ConvertUTCDateTime() - TimerStarted).TotalSeconds > 60)
            {
                IsTimerStarted = false;
                _timer.Dispose();
            }
        }

    }
}
