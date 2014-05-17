using System;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace Jittuu.Rx.AddOn
{
    public static class TaskTimeoutAfter
    {
        public static Task TimeoutAfter(this Task task, TimeSpan timeout)
        {            
            return task.ToObservable().Timeout(timeout).ToTask();
        }

        public static Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            return task.ToObservable().Timeout(timeout).ToTask();
        }

    }
}
