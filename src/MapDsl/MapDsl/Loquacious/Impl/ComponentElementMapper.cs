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
using System.Linq;
using System.Linq.Expressions;
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious.Impl
{
	public class ComponentElementMapper<TComponent> : IComponentElementMapper<TComponent> where TComponent : class
	{
		private readonly HbmCompositeElement component;
		private readonly HbmMapping mapDoc;

		public ComponentElementMapper(HbmMapping mapDoc, HbmCompositeElement component)
		{
			this.mapDoc = mapDoc;
			this.component = component;
		}

		#region Implementation of IComponentElement<TComponent>

		public void Property<TProperty>(Expression<Func<TComponent, TProperty>> property)
		{
			var hbmProperty = new HbmProperty {name = TypeUtils.DecodeMemberAccessExpression(property).Name};
			AddProperty(hbmProperty);
		}

		public void Component<TNestedComponent>(Expression<Func<TComponent, TNestedComponent>> property,
		                                        Action<IComponentElementMapper<TNestedComponent>> mapping)
			where TNestedComponent : class
		{
			var hbm = new HbmNestedCompositeElement {name = TypeUtils.DecodeMemberAccessExpression(property).Name};
			mapping(new ComponentNestedElementMapper<TNestedComponent>(mapDoc, hbm));
			AddProperty(hbm);
		}

		public void ManyToOne<TProperty>(Expression<Func<TComponent, TProperty>> property) where TProperty : class
		{
			var hbm = new HbmManyToOne {name = TypeUtils.DecodeMemberAccessExpression(property).Name,};
			AddProperty(hbm);
		}

		#endregion

		protected void AddProperty(object property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			var toAdd = new[] {property};
			component.Items = component.Items == null ? toAdd : component.Items.Concat(toAdd).ToArray();
		}
	}
}