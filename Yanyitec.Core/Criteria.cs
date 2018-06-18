using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Newtonsoft.Json.Schema;

namespace Yanyitec
{
    public class Criteria<T>
    {
        
        public Criteria(Expression<Func<T, bool>> initCriteria=null)
        {
            if (initCriteria != null)
            {
                this.Parameter = initCriteria.Parameters[0];
                this._Expression = initCriteria.Body;
            }
            else {
                this.Parameter = System.Linq.Expressions.Expression.Parameter(typeof(T), "entity");
            }

        }

        
        [Newtonsoft.Json.JsonIgnore]
        public ParameterExpression Parameter { get; protected set; }
        protected Expression _Expression;
        [Newtonsoft.Json.JsonIgnore]
        public Expression<Func<T, bool>> Expression
        {
            get {
                if (this._Expression == null) return null;
                return System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(this._Expression, this.Parameter);
            }
            
            
        }



        public Criteria<T> AndAlso(Expression<Func<T, bool>> criteria)
        {
            if (criteria == null) return this;
            if (this._Expression == null)
            {
                this.Parameter = criteria.Parameters[0];
                this._Expression = criteria.Body;
            }
            else
            {
                //this._Expression = System.Linq.Expressions.Expression.AndAlso(this._Expression, criteria);
                this._Expression = System.Linq.Expressions.Expression.AndAlso(this._Expression, Convert(criteria, criteria.Parameters[0]));
            }
            return this;
        }

        public Criteria<T> AndAlso(Criteria<T> criteria)
        {
            return this.AndAlso(criteria.Expression);
        }

        public Criteria<T> OrElse(Expression<Func<T, bool>> criteria)
        {
            if (this._Expression == null)
            {
                this.Parameter = criteria.Parameters[0];
                this._Expression = criteria.Body;
            }
            else
            {
                //this._Expression = System.Linq.Expressions.Expression.OrElse(this._Expression, criteria);
                this._Expression = System.Linq.Expressions.Expression.OrElse(this.Expression, Convert(criteria, criteria.Parameters[0]));
            }
            return this;
        }

        public Criteria<T> OrElse(Criteria<T> criteria)
        {
            return this.OrElse(criteria.Expression);
        }

        Expression Convert(Expression expr, ParameterExpression param)
        {
            if (expr == param) return this.Parameter;
            BinaryExpression bExpr = null;
            UnaryExpression uExpr = null;
            switch (expr.NodeType)
            {
                case ExpressionType.Lambda:
                    var lamda = (expr as LambdaExpression);
                    return Convert(lamda.Body,lamda.Parameters[0]);
                case ExpressionType.Constant:
                    return expr;
                case ExpressionType.And:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.And(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.Add:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Add(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.AndAlso:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.OrElse(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.MemberAccess:
                    var member = expr as MemberExpression;
                    return System.Linq.Expressions.Expression.MakeMemberAccess(Convert(member.Expression,param),member.Member);
                case ExpressionType.Call:
                    var call = expr as MethodCallExpression;
                    var list = new List<Expression>();
                    foreach (var arg in call.Arguments)
                    {
                        list.Add(Convert(arg, param));
                    }
                    return System.Linq.Expressions.Expression.Call(Convert(call.Object, param), call.Method, list);
                case ExpressionType.Convert:
                    uExpr = expr as UnaryExpression;
                    return System.Linq.Expressions.Expression.Convert(Convert(uExpr.Operand, param), uExpr.Type);
                case ExpressionType.Divide:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Divide(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.Equal:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Equal(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.GreaterThan:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.GreaterThan(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.GreaterThanOrEqual:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.GreaterThanOrEqual(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.LessThan:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.LessThan(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.LessThanOrEqual:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.LessThan(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.LeftShift:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.LeftShift(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.Multiply:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Multiply(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.Negate:
                    uExpr = expr as UnaryExpression;
                    return System.Linq.Expressions.Expression.Negate(Convert(uExpr.Operand, param));
                case ExpressionType.Not:
                    uExpr = expr as UnaryExpression;
                    return System.Linq.Expressions.Expression.Not(Convert(uExpr.Operand, param));
                case ExpressionType.NotEqual:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.NotEqual(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.Or:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Or(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.OrElse:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.OrElse(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.Power:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Power(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.RightShift:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.RightShift(Convert(bExpr.Left, param), Convert(bExpr.Right, param));
                case ExpressionType.Subtract:
                    bExpr = expr as BinaryExpression;
                    return System.Linq.Expressions.Expression.Subtract(Convert(bExpr.Left, param), Convert(bExpr.Right, param));

            }
            throw new NotSupportedException();
        }

        public static Criteria<T> operator &(Criteria<T> criteria, Expression<Func<T, bool>> expr)
        {
            return criteria == null ? new Criteria<T>(expr) : criteria.AndAlso(expr);
        }

        public static Criteria<T> operator |(Criteria<T> criteria, Expression<Func<T, bool>> expr)
        {
            return criteria == null ? new Criteria<T>(expr) : criteria.OrElse(expr);
        }

        public static implicit operator Criteria<T>(Expression<Func<T, bool>> expr)
        {
            return new Criteria<T>(expr);
        }
    }
}