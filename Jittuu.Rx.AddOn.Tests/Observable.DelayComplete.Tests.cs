using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSupport;
using System.Reactive.Concurrency;

namespace Jittuu.Rx.AddOn.Tests
{
    [TestClass]
    public class ObservableDelayTests
    {
        private IObservable<Unit> _obs;
        private TestScheduler _scheduler;

        [TestInitialize]
        public void Setup()
        {
            _scheduler = new TestScheduler();
            _obs = Observable.Return(Unit.Default);
        }

        [TestMethod]
        public void Should_complete_after_delay()
        {
            var received = false;
            _obs.DelayComplete(5.Seconds(), _scheduler)
                .Subscribe(
                    onNext: _ => { },
                    onCompleted: () => received = true);

            _scheduler.AdvanceBy(6.Seconds());

            Assert.AreEqual(true, received);
        }

        [TestMethod]
        public void Should_not_complete_before_delay()
        {
            var received = false;
            _obs.DelayComplete(5.Seconds(), _scheduler)
                .Subscribe(
                    onNext: _ => { },
                    onCompleted: () => received = true);

            _scheduler.AdvanceBy(4.Seconds());

            Assert.AreEqual(false, received);
        }

        [TestMethod]
        public void Should_receive_error_before_delay()
        {
            var obs = Observable.Throw<Exception>(new Exception()).DelayComplete(5.Seconds());

            var receivedError = false;

            obs.Subscribe(
                onNext: _ => { },
                onError: _ => receivedError = true);
            _scheduler.AdvanceBy(4.Seconds());

            Assert.AreEqual(true, receivedError);
        }

        [TestMethod]
        public void Should_receive_next_before_delay()
        {
            var received = false;
            _obs.DelayComplete(5.Seconds(), _scheduler)
                .Subscribe(_ => received = true);

            _scheduler.AdvanceBy(4.Seconds());

            Assert.AreEqual(true, received);

        }
    }
}
