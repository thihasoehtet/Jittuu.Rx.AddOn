# Jittuu.Rx.AddOn
Add-on extensions for C# Reactive Extension and also available via [NuGet](http://www.nuget.org/packages/Jittuu.Rx.AddOn/).

### Observable.CompleteWhen
* `CompleteWhen<T>(Func<Exception, bool> predicate)`

`Observable.CompleteWhen` is a overload of `Observable.Catch` which allow you to swallow the `Exception` and then complete the source based on the predicate.

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

```C#
source.CompleteWhen(ex => ex is ArgumentNullException && ex.Message == "arg1 is null")
```


* Observable.DelayComplete
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
