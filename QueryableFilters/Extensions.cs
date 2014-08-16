﻿using System;
﻿using System.Linq;
using System.Linq.Expressions;

namespace QueryableFilters
{
    public static class Extensions
    {
        public static IQueryable<T> FilterEquals<T, TValue>(this IQueryable<T> qSource, Expression<Func<T, TValue>> field, TValue value, bool skipNullArg = true)
        {
            if (value == null)
            {
                if (skipNullArg)
                    return qSource;
                
                return qSource.ApplyWhereIsNull(field);
            }
                
            if (typeof(TValue) == typeof(string))
            {
                var str = (string)(object)value;
                str = str.Trim();
                if (str == "")
                    return qSource;
                value = (TValue)(object)str;
            }

            Expression fieldFilter = ExpressionClosureFactory.GetField(value);
            var underlyingType = Nullable.GetUnderlyingType(field.Body.Type);
            if (underlyingType != null)
            {
                var convertedToUnderline = Expression.Convert(fieldFilter, underlyingType);
                fieldFilter = Expression.Convert(convertedToUnderline, field.Body.Type);
            }
            
            var constraint = Expression.Equal(field.Body, fieldFilter);

            return qSource.ApplyWhere(constraint, field.Parameters);
        }

        public static IQueryable<T> FilterContains<T>(this IQueryable<T> qSource, Expression<Func<T, string>> field, string value, bool skipEmptyArg = true, bool trimArg = true)
        {
            if (skipEmptyArg)
            {
                if (string.IsNullOrWhiteSpace(value))
                    return qSource;
            }
            else
            {
                if (value == null)
                    return qSource.ApplyWhereIsNull(field);
            }

            string formattedValue = value;
            if (trimArg)
            {
                formattedValue = (value ?? "").Trim();
            }

            var containsmethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var constraint = Expression.Call(field.Body, containsmethod, ExpressionClosureFactory.GetField(formattedValue));

            return qSource.ApplyWhere(constraint, field.Parameters);
        }

        public static IQueryable<T> FilterDateRange<T>(this IQueryable<T> qSource,
                                                                            Expression<Func<T, DateTime>> field,
                                                                            DateTime? from, DateTime? to, bool ignoreTime = true)
        {
            return qSource.FilterDateRangeInner(field, from, to, ignoreTime);
        }

        public static IQueryable<T> FilterDateRange<T>(this IQueryable<T> qSource,
                                                                            Expression<Func<T, DateTime?>> field,
                                                                            DateTime? from, DateTime? to, bool ignoreTime = true)
        {
            return qSource.FilterDateRangeInner(field, from, to, ignoreTime);
        }

        public static IQueryable<T> FilterOnDate<T>(this IQueryable<T> qSource,
                                                                            Expression<Func<T, DateTime>> field,
                                                                            DateTime? date)
        {
            return qSource.FilterOnDateInner(field, date);
        }

        public static IQueryable<T> FilterOnDate<T>(this IQueryable<T> qSource,
                                                                            Expression<Func<T, DateTime?>> field,
                                                                            DateTime? date)
        {
            return qSource.FilterOnDateInner(field, date);
        }

        


        public static IQueryable<T> FilterDateRange<T, TValueFrom, TValueTo>(this IQueryable<T> qSource,
                                                                            Expression<Func<T, TValueFrom>> fieldFrom,
                                                                            Expression<Func<T, TValueTo>> fieldTo,
                                                                            DateTime? from, DateTime? to, bool ignoreTime = true)
        {
            Ensure.ExpressionDateTime(fieldFrom);
            Ensure.ExpressionDateTime(fieldTo);

            if (from == null && to == null)
                return qSource;

            if (from != null)
            {
                qSource = qSource.ApplyWhereDateGreaterThanOrEqual(fieldTo, from.Value, ignoreTime);
            }
            if (to != null)
            {
                qSource = qSource.ApplyWhereDateLessThan(fieldFrom, to.Value, ignoreTime);
            }

            return qSource;
        }

        
    }
}
