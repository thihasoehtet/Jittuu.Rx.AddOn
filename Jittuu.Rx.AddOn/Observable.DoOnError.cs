using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableDoOnError
    {
        public static IObservable<T> DoOnError<T>(this IObservable<T> source, Action<Exception> onError)
        {
            // ys = xs.DoOnError(f());

            // xs       ------o---------o------X->
            //                |         |      |
            //                |         |     f()
            //                |         |      |
            //                v         v      v
            // ys       ------o---------o------X->


            // xs       ------o---------o------|->
            //                |         |      |
            //                v         v      v
            // ys       ------o---------o------|->

            return source.Do(
                onNext: _ => { },
                onError: onError
                );
        }

        public static IObservable<T> DoOnErrorAsync<T>(this IObservable<T> source, Func<Exception, Task> onError)
        {
            return Observable.Create<T>(observer =>
            {
                return source.Subscribe(
                                onNext: observer.OnNext,
                                onError: async ex =>
                                {
                                    try
                                    {
                                        await onError(ex);
                                        observer.OnError(ex);
                                    }
                                    catch (Exception innerException)
                                    {
                                        observer.OnError(innerException);
                                    }
                                },
                                onCompleted: observer.OnCompleted);
            });
        }
    }
}
