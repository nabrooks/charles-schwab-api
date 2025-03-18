using System.Reactive.Disposables;

namespace NbTrader.Brokers.Extensions
{
    public static class CompositeDisposableExtensions
    {
        public static void AddRange(this CompositeDisposable compositeDisposable, params IDisposable[] disposables)
        {
            foreach (var disposable in disposables)
            {
                compositeDisposable.Add(disposable);
            }
        }
    }
}
