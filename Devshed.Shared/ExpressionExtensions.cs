namespace Devshed.Shared
{
    using System;
    using System.Linq.Expressions;

    /// <summary> Helps with expressions. </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets a member name from an expression.
        /// </summary>
        /// <typeparam name="TDelegate"> The expression delegate type to use. </typeparam>
        /// <param name="expression"> The expression as value. Example: e =&lt; e.Username </param>
        /// <returns></returns>
        public static string GetMemberName<TDelegate>(this Expression<TDelegate> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                var method = expression.Body as MethodCallExpression;
                if (method != null)
                {
                    return method.Method.Name;
                }
            }

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            if (body != null)
            {
                return body.Member.Name;
            }

            throw new InvalidOperationException(
                "Could not determine name of member or method (" + expression.ToString() + "). "
                + " Check if the expression contains a narrow conversion,"
                + " which may infer a hidden function.");
        }
    }
}
