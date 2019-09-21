using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace BLEPrototype.Extensions
{
    public static class BLEExtensions
    {

        public static T DisposeWith<T>(this T item, CompositeDisposable compositeDisposable) 
            where T : IDisposable
        {
            if (compositeDisposable == null)
                throw new ArgumentNullException(nameof(compositeDisposable));

            compositeDisposable.Add(item);

            return item;
        }

    }
}
