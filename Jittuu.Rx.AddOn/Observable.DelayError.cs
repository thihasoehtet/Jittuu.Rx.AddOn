using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableDelayError
    {
        public static IObservable<T> DelayError<T>(this IObservable<T> source, TimeSpan dueTime)
        {
            return source.DelayError(dueTime, Scheduler.Default);
        }

        public static IObservable<T> DelayError<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return source.DelayError(() => dueTime, scheduler);
        }

        public static IObservable<T> DelayError<T>(this IObservable<T> source, Func<TimeSpan> dueTime, IScheduler scheduler)
        {
            //  ys = xs.DelayError(dueTime)
            //
            //  xs        -----o------o-----o---|-->
            //                 |      |     |   |
            //                 v      v     v   v
            //  ys        -----o------o-----o---|-->
            //

            //
            //  xs        -----o------o-----o--X->
            //                 |      |     |  --duetime-| 
            //                 v      v     v            v
            //  ys        -----o------o-----o------------X->
            //

            return Observable.Create<T>(observer =>
            {
                return source.Subscribe(observer.OnNext,
                                        ex => scheduler.Schedule(dueTime(), () => observer.OnError(ex)),
                                        observer.OnCompleted);
            });
        }

        public static IObservable<T> DelayError<T, TException>(this IObservable<T> source, TimeSpan dueTime, TimeSpan exceptionDueTime) where TException : Exception
        {
            return source.DelayError<T, TException>(dueTime, exceptionDueTime, Scheduler.Default);
        }

        public static IObservable<T> DelayError<T, TException>(this IObservable<T> source, TimeSpan dueTime, Func<TException, TimeSpan> exceptionDueTime) where TException : Exception
        {
            return source.DelayError<T, TException>(dueTime, exceptionDueTime, Scheduler.Default);
        }

        public static IObservable<T> DelayError<T, TException>(this IObservable<T> source, TimeSpan dueTime, Func<TException, TimeSpan> exceptionDueTime, IScheduler scheduler) where TException : Exception
        {
            //  ys = xs.DelayError(dueTime, exceptionDueTime)
            //
            //  xs        -----o------o-----o---|-->
            //                 |      |     |   |
            //                 v      v     v   v
            //  ys        -----o------o-----o---|-->
            //

            // X is not TException
            //
            //  xs        -----o------o-----o--X->
            //                 |      |     |  --duetime-| 
            //                 v      v     v            v
            //  ys        -----o------o-----o------------X->
            //

            // X is TException
            //
            //  xs        -----o------o-----o--X->
            //                 |      |     |  --exceptionDueTime--| 
            //                 v      v     v                      v
            //  ys        -----o------o-----o----------------------X->
            //

            return Observable.Create<T>(observer =>
            {
                return source.Subscribe(observer.OnNext,
                                        ex =>
                                        {
                                            if (ex is TException)
                                                scheduler.Schedule(exceptionDueTime((TException)ex), () => observer.OnError(ex));
                                            else
                                                scheduler.Schedule(dueTime, () => observer.OnError(ex));
                                        },
                                        observer.OnCompleted);
            });
        }

        public static IObservable<T> DelayError<T, TException>(this IObservable<T> source, TimeSpan dueTime, TimeSpan exceptionDueTime, IScheduler scheduler) where TException : Exception
        {
            return source.DelayError<T, TException>(dueTime, _ => exceptionDueTime, scheduler);
        }

        public static IObservable<T> DelayError<T, TException1, TException2>(this IObservable<T> source, TimeSpan dueTime, TimeSpan exceptionDueTime, IScheduler scheduler)
        {
            //  ys = xs.DelayError(dueTime, exceptionDueTime)
            //
            //  xs        -----o------o-----o---|-->
            //                 |      |     |   |
            //                 v      v     v   v
            //  ys        -----o------o-----o---|-->
            //

            // X is not TException
            //
            //  xs        -----o------o-----o--X->
            //                 |      |     |  --duetime-| 
            //                 v      v     v            v
            //  ys        -----o------o-----o------------X->
            //

            // X is TException
            //
            //  xs        -----o------o-----o--X->
            //                 |      |     |  --exceptionDueTime--| 
            //                 v      v     v                      v
            //  ys        -----o------o-----o----------------------X->
            //

            return Observable.Create<T>(observer =>
            {
                return source.Subscribe(observer.OnNext,
                                        ex =>
                                        {
                                            if (ex is TException1)
                                                scheduler.Schedule(exceptionDueTime, () => observer.OnError(ex));
                                            else if (ex is TException2)
                                                scheduler.Schedule(exceptionDueTime, () => observer.OnError(ex));
                                            else
                                                scheduler.Schedule(dueTime, () => observer.OnError(ex));
                                        },
                                        observer.OnCompleted);
            });
        }

    }
}
