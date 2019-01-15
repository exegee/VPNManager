using Prism.Events;
using Prism.Regions;
using System;
using System.Globalization;
using System.Windows.Threading;
using VPNManager.Core;
using VPNManager.Core.Events;

namespace StatusBar.ViewModels
{
    public class StatusBarViewModel : ViewModelBase, IStatusBarViewModel
    {
        #region Private Members
        private DispatcherTimer _clockTimer;
        #endregion

        #region Constructor
        public StatusBarViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base(regionManager, eventAggregator)
        {
            InitializeClockTimer();
            EventAggregator.GetEvent<StatusBarMessageUpdateEvent>().Subscribe(StatusBarMessageUpdate);
            EventAggregator.GetEvent<StatusBarInterfaceMessageUpdateEvent>().Subscribe(StatusBarInterfaceMessageUpdate);
            EventAggregator.GetEvent<StatusBarActiveConnectionMessageUpdateEvent>().Subscribe(StatusBarActiveConnectionMessageUpdate);
        }

        

        #endregion

        #region Properties
        /// <summary>
        /// Message in the status bar
        /// </summary>
        private string _message = "Ready";
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        /// <summary>
        /// Current date and time in the status bar
        /// </summary>
        private string _currentDate = DateTime.Now.ToString("F", new CultureInfo("pl-PL"));
        public string CurrentDate
        {
            get { return _currentDate; }
            set { SetProperty(ref _currentDate, value); }
        }

        /// <summary>
        /// Active connection message
        /// </summary>
        private string _activeConnectionMessage = "Not connected";
        public string ActiveConnectionMessage
        {
            get { return _activeConnectionMessage; }
            set { SetProperty(ref _activeConnectionMessage, value); }
        }
        /// <summary>
        /// Interface message
        /// </summary>
        private string _interfaceMessage;
        public string InterfaceMessage
        {
            get { return _interfaceMessage; }
            set { SetProperty(ref _interfaceMessage, value); }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Initializes clock timer and updates current date and time through event handler
        /// </summary>
        private void InitializeClockTimer()
        {
            _clockTimer = new DispatcherTimer();
            _clockTimer.Tick += new EventHandler((e, s) =>
            {
                // Converts datetime to Thursday, April 10, 2008 6:30:00 AM
                var currentdate = DateTime.Now;
                CurrentDate = currentdate.ToString("F", new CultureInfo("pl-PL"));
            });
            _clockTimer.Interval = new TimeSpan(0, 0, 1);
            _clockTimer.Start();
        }
        /// <summary>
        /// Updates message displayed in status bar
        /// </summary>
        /// <param name="message"></param>
        private void StatusBarMessageUpdate(string message)
        {
            Message = message;
        }

        private void StatusBarActiveConnectionMessageUpdate(string message)
        {
            ActiveConnectionMessage = message;
        }

        private void StatusBarInterfaceMessageUpdate(string message)
        {
            InterfaceMessage = message;
        }

        #endregion
    }
}
