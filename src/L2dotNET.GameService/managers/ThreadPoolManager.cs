using System;
using System.Collections.Generic;
using log4net;

namespace L2dotNET.GameService.Managers
{
    public class ThreadPoolManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ThreadPoolManager));

        private static readonly ThreadPoolManager Instance = new ThreadPoolManager();

        public static ThreadPoolManager GetInstance()
        {
            return Instance;
        }

        public Dictionary<object, RThread> Threads = new Dictionary<object, RThread>();

        public void ExecuteGeneral(Action action, int wait = 0, object id = null)
        {
            RThread thread = new RThread
                             {
                                 Type = RThreadType.General,
                                 Id = id,
                                 StartWait = wait,
                                 PerformAction = action
                             };

            if (id != null)
            {
                Threads.Add(thread.Id, thread);
            }

            thread.Start();

            Log.Info($"threads {Threads.Count}");
        }

        public void ExecuteGeneralTicks(Action action, int ticks, int tickWait = 1000, int wait = 0, object id = null)
        {
            RThread thread = new RThread
                             {
                                 Type = RThreadType.GeneralTick,
                                 Id = id,
                                 StartWait = wait,
                                 PerformAction = action,
                                 Ticks = ticks,
                                 TickSleep = tickWait
                             };

            if (id != null)
            {
                Threads.Add(thread.Id, thread);
            }

            thread.Start();
        }

        public void ExecuteActions(Action[] actions, int tickWait = 1000, int wait = 0, object id = null)
        {
            RThread thread = new RThread
                             {
                                 Type = RThreadType.Actions,
                                 Id = id,
                                 StartWait = wait,
                                 PerformActions = actions,
                                 Ticks = actions.Length,
                                 TickSleep = tickWait
                             };

            if (id != null)
            {
                Threads.Add(thread.Id, thread);
            }

            thread.Start();
        }

        public void ExecuteActionParams(int wait, int tickWait, params Action[] at)
        {
            ExecuteActions(at, tickWait, wait);
        }

        public void StopThread(object id)
        {
            if (Threads.ContainsKey(id))
            {
                Threads[id].AbortMe();
            }
        }

        public void CloseMe(RThread thread)
        {
            if (thread.Id != null)
            {
                lock (Threads)
                {
                    Threads.Remove(thread.Id);
                }
            }

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();
        }
    }
}