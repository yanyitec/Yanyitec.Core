using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Yanyitec.DI
{
    public class CreateInstanceGenerator
    {
        public CreateInstanceGenerator(DependentItem item)
        {
            this.DependentItem = item;
            

            //this.InstanceExpr = Expression.Parameter(item.SubstantiveType,"inst");
            //this.Locals.Add(this.InstanceExpr);
        }
        public DependentItem DependentItem { get; set; }

        public Func<object> Generate() {
            var item = this.DependentItem;
            this.Codes = new List<Expression>();
            //this.Locals = new List<ParameterExpression>();
            this.TypedLocals = new Dictionary<Guid, ParameterExpression>();
            this.GenNewExpression(item, null);
            var labelTarget = Expression.Label("ret");
            var label = Expression.Label(labelTarget, Expression.Constant(null, item.SubstantiveType));
            this.Codes.Add(Expression.Return(labelTarget, this.InstanceExpr));
            this.Codes.Add(label);
            var block = Expression.Block(this.TypedLocals.Values, this.Codes);
            var lamda = Expression.Lambda<Func<object>>(block);
            return lamda.Compile();
        }

        public List<Expression> Codes { get; set; }
        public Dictionary<Guid,ParameterExpression> TypedLocals { get; set; }
        //public List<ParameterExpression> Locals { get; set; }
        public ParameterExpression InstanceExpr { get; set; }

        Expression GenNewExpression(DependentItem item, string varName)
        {
            var ctors = item.SubstantiveType.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            ConstructorInfo ctorInfo = null;
            ConstructorInfo noArgsCtorInfo = null;
            ParameterInfo[] args = null;
            var injCount = 0;
            foreach (var ctor in ctors)
            {
                var attr1 = ctor.GetCustomAttribute<NonInjectAttribute>();
                if (attr1 != null) continue;
                var attr = ctor.GetCustomAttributes<InjectAttribute>();
                if (attr != null)
                {
                    ctorInfo = ctor;
                    args = ctorInfo.GetParameters();
                    injCount++;
                }
                else
                {
                    args = ctor.GetParameters();
                    if (args.Length == 0) noArgsCtorInfo = ctor;
                }
            }
            if (injCount > 1) throw new InvalidProgramException("More than one Contructors were marked [Inject]");
            if (ctorInfo == null) ctorInfo = noArgsCtorInfo;
            if (ctorInfo == null) throw new InvalidProgramException("No constructors were found for injection");

            if (args == null) args = ctorInfo.GetParameters();
            return GenNewExpression(item, varName,ctorInfo,args);
        }

        Expression GenNewExpression(DependentItem currentItem, string varName, ConstructorInfo ctor, ParameterInfo[] args)
        {
            var local = Expression.Parameter(ctor.DeclaringType, varName??"inst");
            if (this.InstanceExpr == null) this.InstanceExpr = local;
            this.TypedLocals.Add(ctor.DeclaringType.GUID,local);
            var paraExprs = new List<Expression>();
            foreach (var paraInfo in args) {
                var type = paraInfo.ParameterType;
                ParameterExpression existedExpr = null;
                if (this.TypedLocals.TryGetValue(type.GUID, out existedExpr)) {
                    paraExprs.Add(existedExpr);
                    continue;
                }
                var name = paraInfo.Name;
                var depItem = currentItem.FindDepedentItem(type, name);
                if (depItem == null)
                {
                    paraExprs.Add(Expression.Constant(paraInfo.ParameterType.GetDefaultValue(), paraInfo.ParameterType));
                    continue;
                }
                if (depItem.Lifecycle == DependenceLifecycles.Constant)
                {
                    paraExprs.Add(Expression.Constant(GetConstValue(depItem),paraInfo.ParameterType));
                }
                if (this.TypedLocals.TryGetValue(depItem.SubstantiveType.GUID, out existedExpr))
                {
                    paraExprs.Add(existedExpr);
                    continue;
                }
                var subVarName = varName + "_" + paraInfo.Name;
                existedExpr = Expression.Parameter(paraInfo.ParameterType,varName + "_" + paraInfo.Name);
                paraExprs.Add(existedExpr);
                var instExpr = GenNewExpression(depItem,subVarName);
                this.Codes.Add(Expression.Assign(existedExpr, instExpr));
            }
            var newExpr = Expression.New(ctor, paraExprs);
            this.Codes.Add(Expression.Assign(local,newExpr));
            return local;
        }

        object GetConstValue(DependentItem item) {
            var value = item.CreateInstance();
            if (value == null) return null;
            if (value.GetType() == item.SubstantiveType) return value;
            return item.SubstantiveType.GetValue(value.ToString());
        }

    }
}
