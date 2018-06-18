using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Yanyitec.Meta
{
    public class Property: IProperty
    {
        public Property(MemberInfo memberInfo,IClass Class) {
            this.Class = Class;
            this.MemberInfo = memberInfo;
            var valueType = memberInfo.MemberType == MemberTypes.Field ? (memberInfo as FieldInfo).FieldType : (memberInfo as PropertyInfo).PropertyType;
            this.ValueType = valueType;
            
            ParameterExpression objExpr = Expression.Parameter(typeof(object),"obj");
            var extractExpr = Expression.Parameter(typeof(bool),"isExtract");
            var getterLamda = Expression.Lambda<Func<object, bool, object>>(
                GenGetterExpression(memberInfo,objExpr,extractExpr,false)
                ,objExpr
                ,extractExpr
            );
            this.GetValue = getterLamda.Compile();

            var valueExpr = Expression.Parameter(typeof(object),"value");
            objExpr = Expression.Parameter(typeof(object), "obj");

            var setterLamda = Expression.Lambda<Action<object, object>>(
                GenSetterExpression(memberInfo,objExpr,valueExpr,false)
                ,objExpr
                ,valueExpr
            );
            this.SetValue = setterLamda.Compile();

        }

        public void CheckNullable(Type valueType) {
            if (valueType.FullName.StartsWith("System.Nullable`1"))
            {
                this.IsNullable = true;
                this.EntitativeType = valueType.GetGenericArguments()[0];
            }

            else
            {
                this.IsNullable = false;
                this.EntitativeType = valueType;
            }
        }
        public bool CheckEnumerable(Type valueType) {
            var ifs = valueType.GetInterfaces();
            
            foreach (var ifc in ifs) {
                if (ifc.FullName.StartsWith("System.Collections.Generic.IEnumerable`1")) {
                    var ev = ifc.GetGenericArguments()[0];
                    
                    if (ev.FullName.StartsWith("System.Collections.Generic.KeyValuePair`1")) {
                        var ts = ev.GetGenericArguments();
                        this.KeyType = ts[0];
                        this.EntitativeType = ts[1];
                    } else {
                        this.EntitativeType = ev;
                    }
                    return this.IsEnumerable = true;
                }
                
            }

            return valueType.BaseType == typeof(object)?false:CheckEnumerable(valueType.BaseType);
        }
        public IClass Class { get; private set; }
        public IClass SubsidaryClass { get; private set; }
        public MemberInfo MemberInfo { get; private set; }
        public string Name { get { return this.MemberInfo.Name; } }
        
        public bool IsNullable { get; private set; }

        public bool IsEnumerable { get; private set; }

        public Type ValueType { get; private set; }

        public Type EntitativeType { get; private set; }

        public Type KeyType { get; private set; }

        public Func<object, bool, object> GetValue { get; }
        public Action<object, object> SetValue { get; }
        //static Type NullableType = typeof(Nullable<>);


        protected static Expression GenGetterExpression(MemberInfo memberInfo,ParameterExpression argObjExpr, ParameterExpression argExtractExpr, bool objTyped) {
            Expression objExpr = null;
            if (objTyped)
            {
                objExpr = argObjExpr;
            }
            else {
                objExpr = Expression.Convert(argObjExpr, memberInfo.DeclaringType);
            }
            Expression memberAccessExpr = Expression.MakeMemberAccess(objExpr,memberInfo);
            Expression valueExpr = null;

            var valueType = memberInfo.MemberType == MemberTypes.Field ? (memberInfo as FieldInfo).FieldType : (memberInfo as PropertyInfo).PropertyType;
            //if (valueType.IsClass) return valueExpr;
            if (!valueType.IsClass && valueType.FullName.StartsWith("System.Nullable`1"))
            {
                //var hasValuePropertyInfo = valueType.GetProperty("HasValue");
                var valuePropertyInfo = valueType.GetProperty("Value");
                valueExpr = Expression.Condition(
                    argExtractExpr
                    ,Expression.Convert(Expression.Property(memberAccessExpr,valuePropertyInfo),typeof(object))
                    ,Expression.Convert(memberAccessExpr,typeof(object))
                );
            }
            else
            {
                valueExpr = Expression.Convert(memberAccessExpr, typeof(object));
            }
            return valueExpr;

        }

        protected static Expression GenSetterExpression(MemberInfo memberInfo, ParameterExpression argObjExpr, ParameterExpression argValueExpr, bool objTyped)
        {
            Expression objExpr = null;
            if (objTyped)
            {
                objExpr = argObjExpr;
            }
            else
            {
                objExpr = Expression.Convert(argObjExpr, memberInfo.DeclaringType);
            }
            Expression memberAccessExpr = Expression.MakeMemberAccess(objExpr, memberInfo);
            Expression valueExpr = null;
            var valueType = memberInfo.MemberType == MemberTypes.Field ? (memberInfo as FieldInfo).FieldType : (memberInfo as PropertyInfo).PropertyType;
            if (!valueType.IsClass && valueType.FullName.StartsWith("System.Nullable`1"))
            {
                var isNullExpr = Expression.Equal(argValueExpr, Expression.Constant(null, typeof(object)));
                var nullExpr = Expression.New(valueType);

                Type innerType = valueType.GetGenericArguments()[0];
                var getValueTypeExpr = Expression.Call(argValueExpr, valueType.GetMethod("GetType"));
                var isNullableExpr = Expression.Equal(Expression.Constant(valueType), getValueTypeExpr);

                var boxExpr = Expression.New(valueType.GetConstructor(new Type[] { innerType }), Expression.Convert(argValueExpr, innerType));
                valueExpr = Expression.Condition(
                    isNullExpr,
                    nullExpr,
                    Expression.Condition(isNullableExpr, Expression.Convert(argValueExpr,valueType), boxExpr)
                );
            }
            else {
                valueExpr = Expression.Convert(argValueExpr, valueType);
            }
            

            return Expression.Assign(memberAccessExpr,valueExpr);

        }
    }
}
