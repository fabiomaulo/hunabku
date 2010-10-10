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
	public interface IMapper
	{
		void Assembly(Assembly assembly);
		void NameSpace(string @namespace);

		void Class<TEntity, TPoid>(Expression<Func<TEntity, TPoid>> idProperty, Action<IIdMapper> idMapping,
		                           Action<IClassMapper<TEntity>> classMapping) where TEntity : class;

		void Subclass<TEntity>(Action<ISubclassMapper<TEntity>> mapping) where TEntity : class;
		void JoinedSubclass<TEntity>(Action<IJoinedSubclassMapper<TEntity>> mapping) where TEntity : class;
		HbmMapping GetCompiledMapping();
	}
}