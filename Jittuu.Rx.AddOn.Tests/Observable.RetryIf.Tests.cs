using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reactive.Testing;
using System.Reactive.Linq;
using System.Linq;
using System.Reactive;

namespace Jittuu.Rx.AddOn.Tests
{
    [TestClass]
    public class ObservableRetryIfTests
    {
        [TestMethod]
        public void RetryIf_predicate_return_true()
        {
            var count = 0;
            Func<int, TimeSpan> _repeatInterval = (i) => TimeSpan.FromSeconds(1 * i);
            var scheduler = new TestScheduler();
            var raw = Observable.Defer(() =>
            {
                count++;
                if (count == 1)
                {
                    return Observable.Throw<int>(new TestException());
                }

                if (count == 3)
                    return Observable.Throw<int>(new Exception());

                return Observable.Return(count);
            }).Repeat();


            var observer = scheduler.CreateObserver<int>();
            using (raw.RetryIf(ex => ex is TestException, scheduler).Subscribe(observer))
            {
                Assert.AreEqual(2, observer.Messages.Count);
                Assert.AreEqual(1, observer.Messages.Where(m => m.Value.Kind == System.Reactive.NotificationKind.OnNext).Count());
                Assert.AreEqual(1, observer.Messages.Where(m => m.Value.Kind == System.Reactive.NotificationKind.OnError).Count());
            }

        }

        [TestMethod]
        public void RetryIf_predicate_return_true_test2()
        {
            var scheduler = new TestScheduler();
            var raw = new WagerBetDetailObservable();

            var observer = scheduler.CreateObserver<int>();
            using (raw.RetryIf(ex => ex is TestException).Subscribe(observer))
            {
                Assert.AreEqual(2, observer.Messages.Count);
                Assert.AreEqual(2, observer.Messages.Where(m => m.Value.Kind == System.Reactive.NotificationKind.OnNext).Count());
            }
        }

        internal class WagerBetDetailObservable : ObservableBase<int>
        {
            IObservable<int> _observable;
            private bool hasPending;
            private int count = 0;

            public WagerBetDetailObservable()
            {
                hasPending = false;

                _observable = Observable
                                .Defer(() =>
                                {
                                    count++;

                                    return Observable.Return(count);
                                });
            }

            protected override IDisposable SubscribeCore(IObserver<int> observer)
            {
                return _observable.Subscribe(onNext: c =>
                {
                    if (c == 1)
                    {
                        hasPending = true;
                    }
                    else
                    {
                        hasPending = false;
                    }

                    observer.OnNext(count);
                },
                        onCompleted: () =>
                        {
                            if (hasPending)
                            {
                                observer.OnError(new TestException());
                            }

                        });
            }
        }

        [Serializable]
        public class TestException : Exception
        {
            public TestException() { }
            public TestException(string message) : base(message) { }
            public TestException(string message, Exception inner) : base(message, inner) { }
            protected TestException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
}
