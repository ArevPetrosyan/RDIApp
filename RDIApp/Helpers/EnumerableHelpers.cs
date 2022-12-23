using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms.Internals;

namespace RDIApp.Helpers
{
    public static class EnumerableHelpers
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return null;

            var result = new ObservableCollection<T>();
            source.ForEach(result.Add);
            return result;
        }
    }
}
