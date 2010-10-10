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
using System.Reflection;
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious.Impl
{
	public class ClassMapper<TEntity> : AbstractPropertyContainerMapper<TEntity>, IClassMapper<TEntity>
		where TEntity : class
	{
		private readonly HbmClass classMapping = new HbmClass();
		private readonly HbmId id;

		public ClassMapper(HbmMapping mapDoc, MemberInfo idProperty) : base(mapDoc)
		{
			var toAdd = new[] {classMapping};
			classMapping.name = TypeUtils.GetClassName(mapDoc, typeof (TEntity));
			id = new HbmId {name = idProperty.Name};
			classMapping.Item = id;
			mapDoc.Items = mapDoc.Items == null ? toAdd : mapDoc.Items.Concat(toAdd).ToArray();
		}

		internal HbmId Id
		{
			get { return id; }
		}

		#region Overrides of AbstractClassMapping<TEntity>

		protected override void AddProperty(object property)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			var toAdd = new[] {property};
			classMapping.Items = classMapping.Items == null ? toAdd : classMapping.Items.Concat(toAdd).ToArray();
		}

		#endregion

		#region Implementation of IRootClass<TEntity>

		public void Discriminator()
		{
			classMapping.discriminator = new HbmDiscriminator();
		}

		#endregion
	}
}