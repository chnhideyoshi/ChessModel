using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

namespace ChessModel
{
    public interface IInputData
    {

    }
    public class WorkingThread
    {
        public WorkingThread()
        {
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            mutex = new Mutex(false, "M1", out createNew);
        }
        bool IsWaitingInput = true;
        Mutex mutex;
        BackgroundWorker worker = new BackgroundWorker();
        bool createNew;
        IInputData data = null;
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Work != null)
                   Work();
        }
        public DoWork Work;
        public bool StartThreadCircle()
        {
            if (worker.IsBusy)
            {
                return false;
            }
            try
            {
                worker.RunWorkerAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public bool SendInput(IInputData d)
        {
            if (IsWaitingInput)
            {
                mutex.WaitOne();
                data = d;
                IsWaitingInput = false;
                mutex.ReleaseMutex();
                return true;
            }
            else
            {
                return false;
            }
        }
        public int CheckInputInterval = 1000;
        public IInputData ReceiveInput()
        {
            while (IsWaitingInput)
            {
                Thread.Sleep(CheckInputInterval);
                if (EventHappened != null)
                    EventHappened(ThreadState.WaitingForInput, null);
            }
            if (EventHappened != null)
                EventHappened(ThreadState.InputReceived, data);
            mutex.WaitOne();
            IsWaitingInput = true;
            mutex.ReleaseMutex();
            return data;
        }
        public event EventHappenedEventHandler EventHappened;
    }
    public delegate void EventHappenedEventHandler(ThreadState message, object args);
    public delegate void DoWork();
    public enum ThreadState
    {
        WaitingForInput = 1, BeginWork = 3, WorkCompeletd = 4, InputReceived = 5,
    }
}
