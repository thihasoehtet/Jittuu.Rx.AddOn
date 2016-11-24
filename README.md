# Jittuu.Rx.AddOn
Add-on extensions for C# Reactive Extension and also available via [NuGet](http://www.nuget.org/packages/Jittuu.Rx.AddOn/).

### Observable.CompleteWhen
* `IObservable<T> CompleteWhen<T>(Func<Exception, bool> predicate)`

`Observable.CompleteWhen` is an overload of `Observable.Catch` which allow to swallow the `Exception` and then complete the source based on the predicate.

```
ys = xs.CompleteWhen(true)

xs  -----o------o-----o--X--->
         |      |     |  |
         v      v     v  v
ys  -----o------o-----o--|--->

ys = xs.CompleteWhen(false)

xs  -----o------o-----o--X--->
         |      |     |  |
         v      v     v  v
ys  -----o------o-----o--X--->
```

```cs
source.CompleteWhen(ex => ex is ArgumentNullException && ex.Message == "arg1 is null")
```

### Observable.DelayComplete
* `IObservable<T> DelayComplete<T>(TimeSpan dueTime)`
* `IObservable<T> DelayComplete<T>(Func<TimeSpan> dueTimeSelector)`
* `IObservable<T> DelayComplete<T>(TimeSpan dueTime, IScheduler scheduler)`
* `IObservable<T> DelayComplete<T>(Func<TimeSpan> dueTimeSelector, IScheduler scheduler)`

`Observable.DelayComplete` allow to delay the source complete which is useful to delay before resubscribing/repeating the source.

```
ys = xs.DelayComplete(dueTime)


xs        -----o------o-----o--|->
               |      |     |  ---dueTime--| 
               v      v     v              v
ys        -----o------o-----o--------------|->


            
xs        -----o------o-----o---X-->
               |      |     |   |
               v      v     v   v
ys        -----o------o-----o---X-->            
```

```cs
var obs = Observable.DeferAsync(CrawlWebsiteDataAsync)
         .DelayComplete(RandomDelay)
         .Repeat();
```

* Observable.DelayError
* Observable.DoOnError
* Observable.RetryIf
* Observable.DoAsync
* Observable.WithLatest
* Observable.SubscribeAsync
* Task.TimeoutAfter
* TaskSubscription (Observable.SubscribeAsync)
* IAsyncObserver
* AsyncObserverBase
* AnnonymousAsyncObserver
