#region License
// NHibernate, Relational Persistence for Idiomatic .NET
// 
// This copyrighted material is made available to anyone wishing to use, modify,
// copy, or redistribute it subject to the terms and conditions of the GNU
// Lesser General Public License, as published by the Free Software Foundation.
// 
// This program is distributed in the hope that it will be useful, 
// but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License
// for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this distribution; if not, write to:
// Free Software Foundation, Inc.
// 51 Franklin Street, Fifth Floor
// Boston, MA  02110-1301  USA
#endregion
using System;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious
{
	public interface IGenerator {}

	public static class Generators
	{
		static Generators()
		{
			Native = new NativeGenerator();
		}

		public static IGenerator Native { get; private set; }

		public static IGenerator Foreign<TEntity, TResult>(Expression<Func<TEntity, TResult>> property)
		{
			return new ForeignGenerator(TypeUtils.DecodeMemberAccessExpression(property));
		}

		#region Nested type: AbstractGenerator

		public abstract class AbstractGenerator
		{
			public abstract HbmGenerator GetCompiledGenerator();
		}

		#endregion

		#region Nested type: ForeignGenerator

		private class ForeignGenerator : AbstractGenerator, IGenerator
		{
			private readonly string refProperty;

			public ForeignGenerator(MemberInfo referencedProperty)
			{
				refProperty = referencedProperty.Name;
			}

			public override HbmGenerator GetCompiledGenerator()
			{
				return new HbmGenerator
				       	{@class = "foreign", param = new[] {new HbmParam {name = "property", Text = new[] {refProperty}}}};
			}
		}

		#endregion

		#region Nested type: NativeGenerator

		private class NativeGenerator : AbstractGenerator, IGenerator
		{
			public override HbmGenerator GetCompiledGenerator()
			{
				return new HbmGenerator {@class = "native"};
			}
		}

		#endregion
	}
}