﻿using System;
using System.Threading.Tasks;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableTaskSubscription
    {
        public static IDisposable Subscribe<T>(this IObservable<T> source, Func<T, Task> onNext, Action<Exception> onError)
        {
            Action<T> action = v =>
            {
                onNext(v).ContinueWith(t => onError, TaskContinuationOptions.OnlyOnFaulted);
            };
            return source.Subscribe(action);
        }

        public static IDisposable Subscribe<T>(this IObservable<T> source, Func<T, Task> onNext)
        {
            return source.Subscribe<T>(onNext: onNext, onError: e => { });
        }
    }
}
