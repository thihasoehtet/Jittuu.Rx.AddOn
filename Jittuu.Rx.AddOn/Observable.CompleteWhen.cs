using System;
using System.Reactive.Linq;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableCompleteWhen
    {
        public static IObservable<T> CompleteWhen<T>(this IObservable<T> source, Func<Exception, bool> predicate)
        {
            return source.Catch<T, Exception>(ex =>
            {
                if (predicate(ex))
                {
                    return Observable.Empty<T>();
                }

                return Observable.Throw<T>(ex);
            });
        }
    }
}
