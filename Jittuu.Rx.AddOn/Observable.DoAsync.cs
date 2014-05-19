using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableDoAsync
    {
        public static IObservable<T> DoAsync<T>(this IObservable<T> source, IAsyncObserver<T> observer)
        {
            return source.SelectMany(async value =>
            {
                try
                {
                    await observer.OnNextAsync(value);
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
                return value;
            });
        }

        public static IObservable<T> DoAsync<T>(this IObservable<T> source, Func<T, Task> onNextAsync)
        {
            return source.DoAsync(new AnnonymousAsyncObserver<T>(onNextAsync));
        }

        public static IObservable<T> DoAsync<T>(this IObservable<T> source, Func<T, Task> onNextAsync, Action<Exception> onError)
        {
            return source.DoAsync(new AnnonymousAsyncObserver<T>(onNextAsync, onError));
        }
    }
}
