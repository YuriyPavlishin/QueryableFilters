using System;
using System.Linq.Expressions;

namespace QueryableFilters
{
    internal class ExpressionClosureFactory
    {
        public static MemberExpression GetField<TValue>(TValue value)
        {
            var closure = new ExpressionClosureField<TValue>
            {
                ValueProperty = value
            };

            return Expression.Field(Expression.Constant(closure), "ValueProperty");
        }

        public static MemberExpression GetDateTimeField(Expression propertyExpression, DateTime value)
        {
            var isNullableField = Nullable.GetUnderlyingType(propertyExpression.Type) != null;
            if (isNullableField)
            {
                DateTime? nullableDate = value;
                return GetField(nullableDate);
            }

            return GetField(value);
        }

        class ExpressionClosureField<T>
        {
            public T ValueProperty;
        }
    }
}
