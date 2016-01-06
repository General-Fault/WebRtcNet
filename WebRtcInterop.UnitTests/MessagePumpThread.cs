using System.Threading;
using System.Windows.Threading;

namespace WebRtcInterop.UnitTests
{
    public class MessagePumpThread
    {
        //private DispatcherFrame _frame;
        private Thread _thread;
        private readonly ManualResetEvent _threadStartEvent = new ManualResetEvent(false);

        public MessagePumpThread()
        {
            _thread = new Thread(new ThreadStart(Run));
            _thread.IsBackground = true;
            _thread.Name = "Message Pump Thread";
            _thread.SetApartmentState(ApartmentState.STA);
        }


        public void Start()
        {
            _thread.Start();
        }

        private void Run()
        {
            // Create our context, and install it:
            SynchronizationContext.SetSynchronizationContext(
                new DispatcherSynchronizationContext(
                    Dispatcher.CurrentDispatcher));

            _threadStartEvent.Set();
            //_frame = new DispatcherFrame(true);
            //Dispatcher.PushFrame(_frame);
            Dispatcher.Run();
        }

        public void Stop()
        {
            _threadStartEvent.WaitOne();
            Dispatcher.FromThread(_thread).BeginInvokeShutdown(DispatcherPriority.Background);

            //if (_frame != null) _frame.Continue = false;
            _thread.Join();
        }

        public Dispatcher Dispatcher
        {
            get
            {
                _threadStartEvent.WaitOne();
                return Dispatcher.FromThread(_thread);
            }
        }
    }
}
