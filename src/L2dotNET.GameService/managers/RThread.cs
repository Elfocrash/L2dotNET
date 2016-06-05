using System;
using System.Threading;
using log4net;

namespace L2dotNET.GameService.Managers
{
    public class RThread
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RThread));

        public Action PerformAction;
        public Action[] PerformActions;
        public RThreadType type;
        private Thread thread;
        public object id;
        public int StartWait;
        public int Ticks;
        public int TickSleep;
        public bool debug = false;

        ~RThread()
        {
            if (debug)
                log.Info($"Thread[{id}] was erased");
        }

        public void Start()
        {
            switch (type)
            {
                case RThreadType.general:
                    thread = new Thread(DoWorkGeneral);
                    break;
                case RThreadType.general_tick:
                    thread = new Thread(DoWorkGeneralTick);
                    break;
                case RThreadType.actions:
                    thread = new Thread(DoWorkActions);
                    break;
            }

            thread.Start();
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
            if (thread.IsAlive)
                thread.Abort();

            thread = null;
            OnEnd();
        }

        private void OnEnd()
        {
            ThreadPoolManager.getInstance().CloseMe(this);
        }
    }
}