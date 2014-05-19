using System;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jittuu.Rx.AddOn.Tests
{
    [TestClass]
    public class ObservableWithLatestTests
    {
        [TestMethod]
        public void Should_notify_when_left_right_received()
        {
            var left = new Subject<int>();
            var right = new Subject<int>();

            var withLatest = left.WithLatest(right, (l, r) => l + r);
            var received = new List<int>();
            using (withLatest.Subscribe(i => received.Add(i)))
            {
                left.OnNext(1);
                right.OnNext(1);

                Assert.AreEqual(1, received.Count);
                Assert.AreEqual(2, received[0]);
            }
        }

        [TestMethod]
        public void Should_notify_twice_when_left_right_left_received()
        {
            var left = new Subject<int>();
            var right = new Subject<int>();

            var withLatest = left.WithLatest(right, (l, r) => l + r);
            var received = new List<int>();
            using (withLatest.Subscribe(i => received.Add(i)))
            {
                left.OnNext(1);
                right.OnNext(1);
                left.OnNext(2);

                Assert.AreEqual(2, received.Count);
                Assert.AreEqual(2, received[0]);
                Assert.AreEqual(3, received[1]);
            }
        }

        [TestMethod]
        public void Should_notify_twice_when_left_right_left_right_received()
        {
            var left = new Subject<int>();
            var right = new Subject<int>();

            var withLatest = left.WithLatest(right, (l, r) => l + r);
            var received = new List<int>();
            using (withLatest.Subscribe(i => received.Add(i)))
            {
                left.OnNext(1);
                right.OnNext(1);
                left.OnNext(2);
                right.OnNext(2);

                Assert.AreEqual(2, received.Count);
                Assert.AreEqual(2, received[0]);
                Assert.AreEqual(3, received[1]);
            }
        }

        [TestMethod]
        public void Should_notify_twice_when_left_right_left_right_right_received()
        {
            var left = new Subject<int>();
            var right = new Subject<int>();

            var withLatest = left.WithLatest(right, (l, r) => l + r);
            var received = new List<int>();
            using (withLatest.Subscribe(i => received.Add(i)))
            {
                left.OnNext(1);
                right.OnNext(1);
                left.OnNext(2);
                right.OnNext(2);
                right.OnNext(3);

                Assert.AreEqual(2, received.Count);
                Assert.AreEqual(2, received[0]);
                Assert.AreEqual(3, received[1]);
            }
        }

        [TestMethod]
        public void Should_notify_twice_when_left_right_left_right_right_left_received()
        {
            var left = new Subject<int>();
            var right = new Subject<int>();

            var withLatest = left.WithLatest(right, (l, r) => l + r);
            var received = new List<int>();
            using (withLatest.Subscribe(i => received.Add(i)))
            {
                left.OnNext(1);
                right.OnNext(1);
                left.OnNext(2);
                right.OnNext(2);
                right.OnNext(3);
                left.OnNext(4);

                Assert.AreEqual(3, received.Count);
                Assert.AreEqual(2, received[0]);
                Assert.AreEqual(3, received[1]);
                Assert.AreEqual(7, received[2]);
            }
        }

        [TestMethod]
        public void Should_notify_with_null_when_right_do_not_have_data()
        {
            var bets = new Subject<Currency>();
            var ex = new Subject<ExRate>();

            var betsWithExRate = from g in ex.GroupBy(x => x.Currency)
                                 from b in bets.Where(b => b.Code == g.Key).WithLatest(g, (b, x) =>
                                 {
                                     b.ExRate = x.Rate;
                                     return b;
                                 })
                                 select b;

            var received = new List<Currency>();
            using (betsWithExRate.Subscribe(i => received.Add(i)))
            {
                ex.OnNext(new ExRate { Currency = "CNY", Rate = null });
                bets.OnNext(new Currency { Id = 1, Code = "CNY" });
                ex.OnNext(new ExRate { Currency = "CNY", Rate = 2.5m });
                ex.OnNext(new ExRate { Currency = "MYR", Rate = 2.5m });
                bets.OnNext(new Currency { Id = 2, Code = "CNY" });
                ex.OnNext(new ExRate { Currency = "CNY", Rate = 2.6m });
                bets.OnNext(new Currency { Id = 3, Code = "CNY" });

                Assert.AreEqual(3, received.Count);
                Assert.AreEqual(null, received[0].ExRate);
                Assert.AreEqual(2.5m, received[1].ExRate);
                Assert.AreEqual(2.6m, received[2].ExRate);
            }
        }

        private class Currency
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public decimal? ExRate { get; set; }
        }

        private class ExRate
        {
            public string Currency { get; set; }
            public decimal? Rate { get; set; }
        }
    }
}
