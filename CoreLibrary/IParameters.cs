using System;
using System.Collections.Generic;

namespace CoreLibrary {
    public interface IParameters<T> {
        IEnumerable<T> GetParameters(Func<T> createParameter);
    }
    public interface IPagingParameters<T> : IParameters<T> {
        IEnumerable<T> GetFilterParameters(Func<T> createParameter);
    }
}