using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableDelayComplete
    {
        public static IObservable<T> DelayComplete<T>(this IObservable<T> source, TimeSpan dueTime)
        {
            return source.DelayComplete(dueTime, Scheduler.Default);
        }

        public static IObservable<T> DelayComplete<T>(this IObservable<T> source, Func<TimeSpan> dueTimeSelector)
        {
            return source.DelayComplete(dueTimeSelector, Scheduler.Default);
        }

        public static IObservable<T> DelayComplete<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return DelayComplete(source, () => dueTime, scheduler);
        }

        public static IObservable<T> DelayComplete<T>(this IObservable<T> source, Func<TimeSpan> dueTimeSelector, IScheduler scheduler)
        {
            //  ys = xs.DelayComplete(dueTime)
            //
            //
            //  xs        -----o------o-----o--|->
            //                 |      |     |  ---dueTime--| 
            //                 v      v     v              v
            //  ys        -----o------o-----o--------------|->
            //

            //
            //  xs        -----o------o-----o---X-->
            //                 |      |     |   |
            //                 v      v     v   v
            //  ys        -----o------o-----o---X-->
            //

            return Observable.Create<T>(observer =>
            {
                IDisposable scheduling = null;

                var subscription = source.Subscribe(
                                            observer.OnNext,
                                            observer.OnError,
                                            onCompleted: () => scheduling = scheduler.Schedule(dueTimeSelector(), () => observer.OnCompleted()));

                return new CompositeDisposable(scheduling, subscription);
            });
        }
    }
}
