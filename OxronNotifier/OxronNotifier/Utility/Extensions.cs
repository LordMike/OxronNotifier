using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace OxronNotifier.Utility
{
    public static class Extensions
    {
        public static void BindText<T, TProp>(this TextBox txtBox, T dataSource, Expression<Func<T, TProp>> expression)
        {
            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("'expression' should be a member expression");

            txtBox.DataBindings.Add(Nameof<TextBox>.Property(box => box.Text), dataSource, body.Member.Name);
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }
    }
}