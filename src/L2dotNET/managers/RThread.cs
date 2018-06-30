using System;
using System.Threading;
using NLog;


namespace L2dotNET.Managers
{
    public class RThread
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public Action PerformAction;
        public Action[] PerformActions;
        public RThreadType Type;
        private Thread _thread;
        public object Id;
        public int StartWait;
        public int Ticks;
        public int TickSleep;
        public bool Debug = false;

        ~RThread()
        {
            if (Debug)
                Log.Info($"Thread[{Id}] was erased");
        }

        public void Start()
        {
            switch (Type)
            {
                case RThreadType.General:
                    _thread = new Thread(DoWorkGeneral);
                    break;
                case RThreadType.GeneralTick:
                    _thread = new Thread(DoWorkGeneralTick);
                    break;
                case RThreadType.Actions:
                    _thread = new Thread(DoWorkActions);
                    break;
            }

            _thread.Start();
        }

        private void DoWorkActions()
        {
            if (StartWait > 0)
                Thread.Sleep(StartWait);

            for (int a = 0; a < Ticks; a++)
            {
                if (TickSleep > 0)
                    Thread.Sleep(TickSleep);

                PerformActions[a]();
            }

            OnEnd();
        }

        private void DoWorkGeneralTick()
        {
            if (StartWait > 0)
                Thread.Sleep(StartWait);

            for (int a = 0; a < Ticks; a++)
            {
                if (TickSleep > 0)
                    Thread.Sleep(TickSleep);

                PerformAction();
            }

            OnEnd();
        }

        private void DoWorkGeneral()
        {
            if (StartWait > 0)
                Thread.Sleep(StartWait);

            PerformAction();
            OnEnd();
        }

        public void AbortMe()
        {
            if (_thread.IsAlive)
                _thread.Abort();

            _thread = null;
            OnEnd();
        }

        private void OnEnd()
        {
            ThreadPoolManager.GetInstance().CloseMe(this);
        }
    }
}