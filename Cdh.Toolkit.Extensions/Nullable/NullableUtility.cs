using System;

namespace Cdh.Toolkit.Extensions.Nullable
{
    public static class NullableUtility
    {
        public static TResult? Convert<TSource, TResult>(this TSource? source, Func<TSource, TResult> conversion)
            where TSource : struct
            where TResult : struct
        {
            Check.ArgumentIsNotNull(conversion, "conversion");

            return source.HasValue ? conversion(source.Value) : (TResult?)null;
        }
    }
}

