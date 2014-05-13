using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableRetryIf
    {
        public static IObservable<T> RetryIf<T>(this IObservable<T> source, Func<Exception, bool> predicate)
        {
            return source.RetryIf(predicate, Scheduler.Default);
        }

        public static IObservable<T> RetryIf<T>(this IObservable<T> source, Func<Exception, bool> predicate, IScheduler scheduler)
        {
            IObservable<T> retry = null;
            retry = source.Catch<T, Exception>(ex =>
            {
                if (predicate(ex))
                {
                    return retry;
                }

                return Observable.Throw<T>(ex);
            });

            return retry;
        }

        public static IObservable<T> RetryIf<T>(this IObservable<T> source, Func<Exception, bool> predicate, int maxRetryCount, IScheduler scheduler)
        {
            IObservable<T> retry = null;
            var currentRetry = 0;
            retry = source.Catch<T, Exception>(ex =>
            {
                if (predicate(ex) && ++currentRetry <= maxRetryCount)
                {
                    return retry;
                }

                return Observable.Throw<T>(ex);
            });

            return retry;
        }
    }
}
