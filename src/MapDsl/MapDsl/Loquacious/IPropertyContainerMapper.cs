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
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MapDsl.Loquacious
{
	public interface IPropertyContainerMapper<TEntity> where TEntity : class
	{
		void Property<TProperty>(Expression<Func<TEntity, TProperty>> property);

		void Component<TComponent>(Expression<Func<TEntity, TComponent>> property,
		                           Action<IComponentMapper<TComponent>> mapping) where TComponent : class;

		void ManyToOne<TProperty>(Expression<Func<TEntity, TProperty>> property) where TProperty : class;
		void OneToOne<TProperty>(Expression<Func<TEntity, TProperty>> property, Action<IOneToOneMapper<TProperty>> mapping) where TProperty : class;

		void Set<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
											 Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping);

		void Bag<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
											 Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping);

		void List<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
												Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping);

		void Map<TKey, TElement>(Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
														 Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping);

		void Set<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
											 Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping,
		                   Action<ICollectionElementRelation<TElement>> mapping) where TElement : class;

		void Bag<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
											 Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping,
		                   Action<ICollectionElementRelation<TElement>> mapping) where TElement : class;

		void List<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
												Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping,
		                    Action<ICollectionElementRelation<TElement>> mapping) where TElement : class;

		void Map<TKey, TElement>(Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
														 Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping,
		                         Action<ICollectionElementRelation<TElement>> mapping) where TElement : class;
	}
}