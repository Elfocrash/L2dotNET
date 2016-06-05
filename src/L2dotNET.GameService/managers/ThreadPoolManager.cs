using System;
using System.Collections.Generic;
using log4net;

namespace L2dotNET.GameService.Managers
{
    public class ThreadPoolManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ThreadPoolManager));

        private static readonly ThreadPoolManager instance = new ThreadPoolManager();

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

            log.Info($"threads {threads.Count}");
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
}