using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Reactive.Testing;
using System.Reactive.Linq;
using System.Linq;

namespace Jittuu.Rx.AddOn.Tests
{
    [TestClass]
    public class ObservableDoOnErrorTests
    {
        [TestMethod]
        public async Task should_be_able_do_necessary_update_while_do_on_error_async()
        {
            var scheduler = new TestScheduler();

            var count = 0;
            var raw = Observable.Defer(() =>
            {
                count++;
                if (count == 4)
                {
                    return Observable.Throw<int>(new Exception("Count=4"));
                }

                return Observable.Return(count);
            }).Repeat();

            var observer = scheduler.CreateObserver<int>();
            using (raw.DoOnErrorAsync(async ex =>
            {
                await TestTask(ex);
            }).Subscribe(observer))
            {
                await Task.Delay(5000);
                Assert.AreEqual(4, observer.Messages.Count);
                Assert.AreEqual(3, observer.Messages.Where(m => m.Value.Kind == System.Reactive.NotificationKind.OnNext).Count());
                Assert.AreEqual(1, observer.Messages.Where(m => m.Value.Kind == System.Reactive.NotificationKind.OnError).Count());
            }
        }

        private Task TestTask(Exception ex)
        {
            return Task.Run(() => { });
        }

    }
}
