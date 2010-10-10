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
using MapDsl.Loquacious.Impl;
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious
{
	public class Mapper : IMapper
	{
		private readonly HbmMapping mapping;

		public Mapper()
		{
			mapping = new HbmMapping();
		}

		#region Implementation of IMapping

		public void Assembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			mapping.assembly = assembly.GetName().Name;
		}

		public void NameSpace(string @namespace)
		{
			if (string.IsNullOrEmpty(@namespace))
			{
				throw new ArgumentNullException("namespace");
			}
			mapping.@namespace = @namespace;
		}

		public void Class<TEntity, TPoid>(Expression<Func<TEntity, TPoid>> idProperty, Action<IIdMapper> idMapping,
		                                  Action<IClassMapper<TEntity>> classMapping) where TEntity : class
		{
			var rc = new ClassMapper<TEntity>(mapping, TypeUtils.DecodeMemberAccessExpression(idProperty));
			idMapping(new IdMapper(rc.Id));
			classMapping(rc);
		}

		public void Subclass<TEntity>(Action<ISubclassMapper<TEntity>> classMapping) where TEntity : class
		{
			var sc = new SubclassMapper<TEntity>(mapping);
			classMapping(sc);
		}

		public void JoinedSubclass<TEntity>(Action<IJoinedSubclassMapper<TEntity>> classMapping) where TEntity : class
		{
			var sc = new JoinedSubclassMapper<TEntity>(mapping);
			classMapping(sc);
		}

		public HbmMapping GetCompiledMapping()
		{
			return mapping;
		}

		#endregion
	}
}