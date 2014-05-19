using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;

namespace Jittuu.Rx.AddOn
{
    public sealed class AnnonymousAsyncObserver<T> : AsyncObserverBase<T>
    {
        private readonly Func<T, Task> _onNextAsync;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        public AnnonymousAsyncObserver(Func<T, Task> onNextAsync, Action<Exception> onError, Action onCompleted, Action<Exception> onNextCoreError)
            : base(onNextCoreError)
        {
            if (onNextAsync == null)
                throw new ArgumentNullException("onNextAsync");
            if (onError == null)
                throw new ArgumentNullException("onError");
            if (onCompleted == null)
                throw new ArgumentNullException("onCompleted");

            this._onNextAsync = onNextAsync;
            this._onError = onError;
            this._onCompleted = onCompleted;
        }

        public AnnonymousAsyncObserver(Func<T, Task> onNextAsync, Action<Exception> onError, Action onCompleted)
            : this(onNextAsync, onError, onCompleted, ex => { })
        { }

        public AnnonymousAsyncObserver(Func<T, Task> onNextAsync, Action<Exception> onError)
            : this(onNextAsync, onError, () => { })
        { }

        public AnnonymousAsyncObserver(Func<T, Task> onNextAsync, Action onCompleted)
            : this(onNextAsync, ex => { }, onCompleted)
        { }

        public AnnonymousAsyncObserver(Func<T, Task> onNextAsync)
            : this(onNextAsync, ex => { })
        { }

        protected override Task OnNextCoreAsync(T value)
        {
            return _onNextAsync(value);
        }

        protected override void OnCompletedCore()
        {
            _onCompleted();
        }

        protected override void OnErrorCore(Exception error)
        {
            _onError(error);
        }
    }
}
