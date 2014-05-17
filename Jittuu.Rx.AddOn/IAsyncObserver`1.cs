using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jittuu.Rx.AddOn
{
    public interface IAsyncObserver<T>
    {
        void OnCompleted();
        void OnError(Exception error);
        Task OnNextAsync(T value);
    }
}
