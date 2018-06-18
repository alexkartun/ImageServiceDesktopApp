using ImageServiceDesktopApp.Controllers;
using ImageServiceDesktopApp.Loggers;
using ImageServiceDesktopApp.Loggers.Models;
using ImageServiceDesktopApp.Models;
using ImageServiceDesktopApp.ServiceServers;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;


namespace ImageServiceDesktopApp
{

    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };


    public partial class ImageService : ServiceBase
    {
        private IImageServer imageServer;

        public ImageService()
        {
            InitializeComponent();
            string eventSourceName = ConfigurationManager.AppSettings["SourceName"];
            string logName = ConfigurationManager.AppSettings["LogName"];
            
            eventLogger = new EventLog
            {
                Source = eventSourceName,
                Log = logName
            };

            ILoggingService imageLogger = new LoggingService();
            imageLogger.MessageRecieved += OnMsg;
            IImageController imageController = new ImageController(new ImageServiceModel(imageLogger));

            imageServer = new ImageServer(imageController);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        protected override void OnStart(string[] args)
        {

            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_START_PENDING,
                dwWaitHint = 100000
            };

            SetServiceStatus(ServiceHandle, ref serviceStatus);

            imageServer.StartService();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(ServiceHandle, ref serviceStatus);

        }

        protected override void OnStop()
        {

            ServiceStatus serviceStatus = new ServiceStatus
            {
                dwCurrentState = ServiceState.SERVICE_STOP_PENDING,
                dwWaitHint = 100000
            };

            SetServiceStatus(ServiceHandle, ref serviceStatus);

            imageServer.StopService();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(ServiceHandle, ref serviceStatus);

        }

        /// <summary>
        /// On message recieved update the write entry of event logger.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message">Message recieved with status.</param>
        private void OnMsg(object sender, MessageRecievedEventArgs message)
        {
            eventLogger.WriteEntry(message.Message, ConvertStatToEventLogEntry(message.Status));
        }

        /// <summary>
        /// Convert MessageTypeEnum to EventLogEntryType.
        /// </summary>
        /// <param name="status">status to convert.</param>
        /// <returns></returns>
        private static EventLogEntryType ConvertStatToEventLogEntry(MessageTypeEnum status)
        {
            if (status == MessageTypeEnum.INFO) return EventLogEntryType.Information;

            else if (status == MessageTypeEnum.WARNING) return EventLogEntryType.Warning;

            else return EventLogEntryType.Error;
        }

    }
}