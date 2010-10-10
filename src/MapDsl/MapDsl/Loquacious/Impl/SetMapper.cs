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
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious.Impl
{
	public class SetMapper<TEntity, TElement> : ICollectionPropertiesMapper<TEntity, TElement> where TEntity : class
	{
		private readonly KeyMapper<TEntity> keyMapper;
		private readonly HbmSet mapping;

		public SetMapper(HbmSet mapping)
		{
			this.mapping = mapping;
			if (mapping.Key == null)
			{
				mapping.key = new HbmKey();
			}
			keyMapper = new KeyMapper<TEntity>(mapping.Key);
		}

		#region Implementation of ICollectionPropertiesMapper<TElement>

		public void Key(Action<IKeyMapper<TEntity>> keyMapping)
		{
			keyMapping(keyMapper);
		}

		public bool Inverse
		{
			get { return mapping.Inverse; }
			set { mapping.inverse = value; }
		}

		public bool Mutable
		{
			get { return mapping.Mutable; }
			set { mapping.mutable = value; }
		}

		public void OrderBy<TProperty>(Expression<Func<TElement, TProperty>> property)
		{
			// TODO: read the mapping of the element to know the column of the property (second-pass)
			mapping.orderby = TypeUtils.DecodeMemberAccessExpression(property).Name;
		}

		public string Where
		{
			get { return mapping.Where; }
			set { mapping.where = value; }
		}

		public int BatchSize
		{
			get { return mapping.BatchSize.GetValueOrDefault(-1); }
			set
			{
				mapping.batchsizeSpecified = true;
				mapping.batchsize = value;
			}
		}

		public CollectionLazy Lazy
		{
			get
			{
				if (!mapping.Lazy.HasValue)
				{
					return CollectionLazy.Lazy;
				}
				switch (mapping.Lazy.Value)
				{
					case HbmCollectionLazy.True:
						return CollectionLazy.Lazy;
					case HbmCollectionLazy.False:
						return CollectionLazy.NoLazy;
					case HbmCollectionLazy.Extra:
						return CollectionLazy.Extra;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set
			{
				mapping.lazySpecified = true;
				switch (value)
				{
					case CollectionLazy.Lazy:
						mapping.lazy = HbmCollectionLazy.True;
						break;
					case CollectionLazy.NoLazy:
						mapping.lazy = HbmCollectionLazy.False;
						break;
					case CollectionLazy.Extra:
						mapping.lazy = HbmCollectionLazy.Extra;
						break;
				}
			}
		}

		public void Sort()
		{
			mapping.sort = "natural";
		}

		#endregion
	}
}