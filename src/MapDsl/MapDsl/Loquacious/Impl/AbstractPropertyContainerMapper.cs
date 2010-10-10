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
using System.Reflection;
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious.Impl
{
	public abstract class AbstractPropertyContainerMapper<TEntity> : IPropertyContainerMapper<TEntity>
		where TEntity : class
	{
		private readonly HbmMapping mapDoc;

		protected AbstractPropertyContainerMapper(HbmMapping mapDoc)
		{
			this.mapDoc = mapDoc;
		}

		protected HbmMapping MapDoc
		{
			get { return mapDoc; }
		}

		protected abstract void AddProperty(object property);

		protected string GetClassName()
		{
			Type type = typeof (TEntity);
			return TypeUtils.GetClassName(MapDoc, type);
		}

		#region Implementation of IClass<TEntity>

		public void Property<TProperty>(Expression<Func<TEntity, TProperty>> property)
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			AddProperty(memberInfo);
		}

		internal void AddProperty(MemberInfo memberInfo)
		{
			var hbmProperty = new HbmProperty {name = memberInfo.Name};
			AddProperty(hbmProperty);
		}

		public void Component<TComponent>(Expression<Func<TEntity, TComponent>> property,
		                                  Action<IComponentMapper<TComponent>> mapping) where TComponent : class
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmComponent {name = memberInfo.Name};
			mapping(new ComponentMapper<TComponent>(MapDoc, hbm));
			AddProperty(hbm);
		}

		public void ManyToOne<TProperty>(Expression<Func<TEntity, TProperty>> property) where TProperty : class
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmManyToOne {name = memberInfo.Name};
			AddProperty(hbm);
		}

		public void OneToOne<TProperty>(Expression<Func<TEntity, TProperty>> property, Action<IOneToOneMapper<TProperty>> mapping) where TProperty : class
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmOneToOne {name = memberInfo.Name};
			mapping(new OneToOneMapper<TProperty>(hbm));
			AddProperty(hbm);
		}

		public void Set<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
															Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping)
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmSet {name = memberInfo.Name};
			collectionMapping(new SetMapper<TEntity, TElement>(hbm));
			hbm.Item = new HbmElement {type1 = TypeUtils.GetTypeName<TElement>()};
			AddProperty(hbm);
		}

		public void Bag<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
															Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping)
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmBag {name = memberInfo.Name};
			collectionMapping(new BagMapper<TEntity, TElement>(hbm));
			hbm.Item = new HbmElement {type1 = TypeUtils.GetTypeName<TElement>()};
			AddProperty(hbm);
		}

		public void List<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
															 Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping)
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmList {name = memberInfo.Name};
			collectionMapping(new ListMapper<TEntity, TElement>(hbm));
			hbm.Item1 = new HbmElement {type1 = TypeUtils.GetTypeName<TElement>()};
			AddProperty(hbm);
		}

		public void Map<TKey, TElement>(Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
																		Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping)
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmMap {name = memberInfo.Name};
			collectionMapping(new MapMapper<TEntity, TKey, TElement>(hbm));
			hbm.Item1 = new HbmElement {type1 = TypeUtils.GetTypeName<TElement>()};
			AddProperty(hbm);
		}

		public void Set<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
															Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping,
		                          Action<ICollectionElementRelation<TElement>> mapping) where TElement : class
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmSet {name = memberInfo.Name};
			collectionMapping(new SetMapper<TEntity, TElement>(hbm));
			mapping(new CollectionElementRelation<TElement>(MapDoc, rel => hbm.Item = rel));
			AddProperty(hbm);
		}

		public void Bag<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
															Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping,
		                          Action<ICollectionElementRelation<TElement>> mapping) where TElement : class
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmBag {name = memberInfo.Name};
			collectionMapping(new BagMapper<TEntity, TElement>(hbm));
			mapping(new CollectionElementRelation<TElement>(MapDoc, rel => hbm.Item = rel));
			AddProperty(hbm);
		}

		public void List<TElement>(Expression<Func<TEntity, IEnumerable<TElement>>> property,
															 Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping,
		                           Action<ICollectionElementRelation<TElement>> mapping) where TElement : class
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmList {name = memberInfo.Name};
			collectionMapping(new ListMapper<TEntity, TElement>(hbm));
			mapping(new CollectionElementRelation<TElement>(MapDoc, rel => hbm.Item1 = rel));
			AddProperty(hbm);
		}

		public void Map<TKey, TElement>(Expression<Func<TEntity, IDictionary<TKey, TElement>>> property,
																		Action<ICollectionPropertiesMapper<TEntity, TElement>> collectionMapping,
		                                Action<ICollectionElementRelation<TElement>> mapping) where TElement : class
		{
			var memberInfo = TypeUtils.DecodeMemberAccessExpression(property);
			var hbm = new HbmMap {name = memberInfo.Name};
			collectionMapping(new MapMapper<TEntity, TKey, TElement>(hbm));
			mapping(new CollectionElementRelation<TElement>(MapDoc, rel => hbm.Item1 = rel));
			AddProperty(hbm);
		}

		#endregion
	}
}