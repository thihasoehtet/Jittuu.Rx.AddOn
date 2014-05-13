using System;
using System.Reactive.Linq;

namespace Jittuu.Rx.AddOn
{
    public static class ObservableWithLatest
    {
        public static IObservable<TResult> WithLatest<TLeft, TRight, TResult>(
            this IObservable<TLeft> left,
            IObservable<TRight> right,
            Func<TLeft, TRight, TResult> selector)
        {
            // ===================================================================================================
            // 
            //                                     (it won't be signal coz no right data)
            //                                      |
            //                                      v
            // left  (x) [e.g. currencies]          o-------o-----o----------------o---->
            //                                             /|    /|               /|
            //                                            / |   / |              / |
            // right (y) [e.g. exchange rates]      -----o-----o--------o-------o------->
            //                                              |     |                |  
            //                                              v     v                v
            // x.WithLatest(y)                      --------o-----o----------------o---->
            //
            // ==================================================================================================

            // [e.g. usage] 
            // currencies.WithLatest(exchangeRates) will signal only when received currency signal.

            return Observable.Defer(() =>
            {
                int lastIndex = -1;
                return Observable
                    .CombineLatest(
                        left.Select(Tuple.Create<TLeft, int>),
                        right,
                        (l, r) => new { Index = l.Item2, Left = l.Item1, Right = r })
                    .Where(x => x.Index != lastIndex)
                    .Do(x => lastIndex = x.Index)
                    .Select(x => selector(x.Left, x.Right));
            });
        }
    }
}
