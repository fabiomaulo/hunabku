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
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious.Impl
{
	public class ComponentMapper<TComponent> : AbstractPropertyContainerMapper<TComponent>, IComponentMapper<TComponent>
		where TComponent : class
	{
		private readonly HbmComponent component;

		public ComponentMapper(HbmMapping mapping, HbmComponent component) : base(mapping)
		{
			this.component = component;
			component.@class = GetClassName();
		}

		#region Overrides of AbstractClassMapping<TComponent>

		protected override void AddProperty(object property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			var toAdd = new[] {property};
			component.Items = component.Items == null ? toAdd : component.Items.Concat(toAdd).ToArray();
		}

		#endregion
	}
}