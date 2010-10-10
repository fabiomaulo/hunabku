using System;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Type;

namespace MapDsl
{
	public sealed class TypeUtils
	{
		public static MemberInfo DecodeMemberAccessExpression<TEntity, TResult>(Expression<Func<TEntity, TResult>> expression)
		{
			if (expression.Body.NodeType != ExpressionType.MemberAccess)
			{
				if ((expression.Body.NodeType == ExpressionType.Convert) && (expression.Body.Type == typeof (object)))
				{
					return ((MemberExpression) ((UnaryExpression) expression.Body).Operand).Member;
				}
				throw new Exception(string.Format("Invalid expression type: Expected ExpressionType.MemberAccess, Found {0}",
				                                  expression.Body.NodeType));
			}
			return ((MemberExpression) expression.Body).Member;
		}

		public static string GetClassName(HbmMapping mapDoc, Type type)
		{
			var typeAssembly = type.Assembly.GetName().Name;
			var typeNameSpace = type.Namespace;
			string assembly = null;
			if (!typeAssembly.Equals(mapDoc.assembly))
			{
				assembly = typeAssembly;
			}
			string @namespace = null;
			if (!typeNameSpace.Equals(mapDoc.@namespace))
			{
				@namespace = typeNameSpace;
			}
			if (!string.IsNullOrEmpty(assembly) && !string.IsNullOrEmpty(@namespace))
			{
				return type.AssemblyQualifiedName;
			}
			if (!string.IsNullOrEmpty(assembly) && string.IsNullOrEmpty(@namespace))
			{
				return string.Concat(type.Name, ", ", assembly);
			}
			if (string.IsNullOrEmpty(assembly) && !string.IsNullOrEmpty(@namespace))
			{
				return type.FullName;
			}

			return type.Name;
		}

		public static string GetTypeName<T>()
		{
			string typeName;
			var typeElement = typeof(T);
			var nhType = TypeFactory.HeuristicType(typeElement.AssemblyQualifiedName);
			if (nhType != null)
			{
				typeName = nhType.Name;
			}
			else
			{
				typeName = typeElement.FullName + ", " + typeElement.Assembly.GetName().Name;
			}
			return typeName;
		}

	}
}