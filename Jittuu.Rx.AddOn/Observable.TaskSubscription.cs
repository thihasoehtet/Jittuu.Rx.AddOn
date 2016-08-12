using System;
using System.Threading.Tasks;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableTaskSubscription
    {
        public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> onNext, Action<Exception> onError)
        {
            Action<T> action = v =>
            {
                onNext(v).ContinueWith(t => onError, TaskContinuationOptions.OnlyOnFaulted);
            };
            return source.Subscribe(action);
        }

        public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> onNext)
        {
            return source.SubscribeAsync<T>(onNext: onNext, onError: e => { });
        }
    }
}
