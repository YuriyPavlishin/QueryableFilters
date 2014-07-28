using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace QueryableFilters
{
    internal static class InternalExtensions
    {
        public static IQueryable<T> ApplyWhereIsNull<T, TValue>(this IQueryable<T> qSource, Expression<Func<T, TValue>> field)
        {
            var constraintNull = Expression.Equal(field.Body, Expression.Constant(null, typeof(TValue)));
            return ApplyWhere(qSource, constraintNull, field.Parameters);
        }

        public static IQueryable<T> ApplyWhereDateGreaterThanOrEqual<T, TValueFrom>(this IQueryable<T> qSource, Expression<Func<T, TValueFrom>> field, DateTime value, bool ignoreTime)
        {
            Ensure.ExpressionDateTime(field);

            if (ignoreTime)
            {
                value = value.Date;
            }

            var constraint = Expression.GreaterThanOrEqual(field.Body, ExpressionClosureFactory.GetDateTimeField(field.Body, value));
            return ApplyWhere(qSource, constraint, field.Parameters);
        }

        public static IQueryable<T> ApplyWhereDateLessThan<T, TValueFrom>(this IQueryable<T> qSource, Expression<Func<T, TValueFrom>> field, DateTime value, bool ignoreTime)
        {
            Ensure.ExpressionDateTime(field);

            if (ignoreTime)
            {
                value = value.Date.AddDays(1);
            }
            var constraint = Expression.LessThan(field.Body, ExpressionClosureFactory.GetDateTimeField(field.Body, value));

            return ApplyWhere(qSource, constraint, field.Parameters);
        }

        public static IQueryable<T> ApplyWhere<T>(this IQueryable<T> qSource, Expression constraint, ReadOnlyCollection<ParameterExpression> parameters)
        {
            var predicateFrom = Expression.Lambda<Func<T, bool>>(constraint, parameters);
            return qSource.Where(predicateFrom);
        }
    }
}
