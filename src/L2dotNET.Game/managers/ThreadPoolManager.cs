using log4net;
using System;
using System.Collections.Generic;
using System.Threading;

namespace L2dotNET.Game.managers
{
    public class ThreadPoolManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ThreadPoolManager));

        private static ThreadPoolManager instance = new ThreadPoolManager();
        public static ThreadPoolManager getInstance()
        {
            return instance;
        }

        public Dictionary<object, RThread> threads = new Dictionary<object, RThread>();


        public void ExecuteGeneral(Action action, int wait = 0, object id = null)
        {
            RThread thread = new RThread();
            thread.type = RThreadType.general;
            thread.id = id;
            thread.StartWait = wait;
            thread.PerformAction = action;

            if (id != null)
                threads.Add(thread.id, thread);

            thread.Start();

            log.Info($"threads { threads.Count }");
        }

        public void ExecuteGeneralTicks(Action action, int ticks, int tickWait = 1000, int wait = 0, object id = null)
        {
            RThread thread = new RThread();
            thread.type = RThreadType.general_tick;
            thread.id = id;
            thread.StartWait = wait;
            thread.PerformAction = action;
            thread.Ticks = ticks;
            thread.TickSleep = tickWait;

            if (id != null)
                threads.Add(thread.id, thread);

            thread.Start();
        }

        public void ExecuteActions(Action[] actions, int tickWait = 1000, int wait = 0, object id = null)
        {
            RThread thread = new RThread();
            thread.type = RThreadType.actions;
            thread.id = id;
            thread.StartWait = wait;
            thread.PerformActions = actions;
            thread.Ticks = actions.Length;
            thread.TickSleep = tickWait;

            if (id != null)
                threads.Add(thread.id, thread);

            thread.Start();
        }

        public void ExecuteActionParams(int wait, int tickWait, params Action[] at)
        {
            ExecuteActions(at, tickWait, wait, null);
        }

        public void StopThread(object id)
        {
            if (threads.ContainsKey(id))
                threads[id].AbortMe();
        }

        public void CloseMe(RThread thread)
        {
            if (thread.id != null)
                lock (threads)
                    threads.Remove(thread.id);

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }
    }

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
                log.Info($"Thread[{ id }] was erased");
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

    public enum RThreadType
    {
        general,
        general_tick,
        loop_tick,
        actions
    }
}
