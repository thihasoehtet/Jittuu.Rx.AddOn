using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;

namespace Jittuu.Rx.AddOn
{
    public abstract class AsyncObserverBase<T> : ObserverBase<T>, IAsyncObserver<T>
    {
        private readonly Action<Exception> _onNextCoreError;

        protected AsyncObserverBase()
            : this(ex => { })
        { }

        protected AsyncObserverBase(Action<Exception> onNextCoreError)
        {
            if (onNextCoreError == null)
                throw new ArgumentNullException("onNextCoreError");
            this._onNextCoreError = onNextCoreError;
        }

        protected abstract Task OnNextCoreAsync(T value);                

        protected override void OnNextCore(T value)
        {
            var task = OnNextCoreAsync(value);
            task.ContinueWith(t => _onNextCoreError(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }

        public Task OnNextAsync(T value)
        {
            return OnNextCoreAsync(value);
        }
    }
}
