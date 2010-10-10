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
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious.Impl
{
	public class CollectionElementRelation<TElement> : ICollectionElementRelation<TElement> where TElement : class
	{
		private readonly Action<object> elementRelationshipAssing;
		private readonly HbmMapping mapping;

		public CollectionElementRelation(HbmMapping mapping, Action<object> elementRelationshipAssing)
		{
			this.mapping = mapping;
			this.elementRelationshipAssing = elementRelationshipAssing;
		}

		#region Implementation of ICollectionElementRelation<TElement>

		public void OneToMany()
		{
			var hbm = new HbmOneToMany {@class = TypeUtils.GetClassName(mapping, typeof (TElement))};
			elementRelationshipAssing(hbm);
		}

		public void ManyToMany()
		{
			var hbm = new HbmManyToMany {@class = TypeUtils.GetClassName(mapping, typeof (TElement))};
			elementRelationshipAssing(hbm);
		}

		public void Component(Action<IComponentElementMapper<TElement>> componentMapping)
		{
			var hbm = new HbmCompositeElement {@class = TypeUtils.GetClassName(mapping, typeof (TElement))};
			componentMapping(new ComponentElementMapper<TElement>(mapping, hbm));
			elementRelationshipAssing(hbm);
		}

		#endregion
	}
}