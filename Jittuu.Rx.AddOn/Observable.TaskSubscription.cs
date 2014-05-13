using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableTaskSubscription
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source, Func<T, Task> onNext, IObserver<Exception> errorObserver)
        {
            Action<T> action = v =>
            {
                onNext(v).ContinueWith(t =>
                {
                    var ae = t.Exception as AggregateException;
                    if (ae != null)
                    {
                        foreach (var ex in ae.Flatten().InnerExceptions)
                        {
                            //_logger.Fatal(ex.Message, ex);
                            errorObserver.OnError(ex);
                        }
                    }
                    else
                    {
                        //_logger.Fatal(t.Exception.Message, t.Exception);
                        errorObserver.OnError(t.Exception);
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
            };
            return source.Subscribe(action);
        }
    }
}
