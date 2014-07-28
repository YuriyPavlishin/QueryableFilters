using System;
using System.Linq.Expressions;

namespace QueryableFilters
{
    internal class Ensure
    {
        public static void ExpressionDateTime<T, TValue>(Expression<Func<T, TValue>> field)
        {
            var type = typeof(TValue);
            if (type == typeof(DateTime) || type == typeof(DateTime?))
                return;

            throw new ArgumentException(string.Format("{0} expression field should be DateTime or DateTime?", field));
        }
    }
}
