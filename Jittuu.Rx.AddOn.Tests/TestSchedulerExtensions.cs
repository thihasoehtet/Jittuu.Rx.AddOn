using System;
using Microsoft.Reactive.Testing;

namespace Jittuu.Rx.AddOn.Tests
{
    public static class TestSchedulerExtensions
    {
        public static void AdvanceBy(this TestScheduler scheduler, TimeSpan duration)
        {
            scheduler.AdvanceBy(duration.Ticks);
        }
    }
}
