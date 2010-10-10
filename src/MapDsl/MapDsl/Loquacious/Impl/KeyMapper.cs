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
using NHibernate.Cfg.MappingSchema;

namespace MapDsl.Loquacious.Impl
{
	public class KeyMapper<TEntity> : IKeyMapper<TEntity>
	{
		private readonly HbmKey mapping;

		public KeyMapper(HbmKey mapping)
		{
			this.mapping = mapping;
			this.mapping.column1 = typeof (TEntity).Name.ToLowerInvariant() + "_key";
		}

		#region Implementation of IKeyMapper<TEntity>

		public bool NotNull
		{
			get { return mapping.notnullSpecified ? mapping.notnull : false; }
			set
			{
				mapping.notnullSpecified = true;
				mapping.notnull = value;
			}
		}

		public bool Update
		{
			get { return mapping.updateSpecified ? mapping.update : false; }
			set
			{
				mapping.updateSpecified = true;
				mapping.update = value;
			}
		}

		public bool Unique
		{
			get { return mapping.uniqueSpecified ? mapping.unique : false; }
			set
			{
				mapping.uniqueSpecified = true;
				mapping.unique = value;
			}
		}

		public string Column
		{
			get { return mapping.column1; }
			set { mapping.column1 = value; }
		}

		public void OnDeleteCascade()
		{
			mapping.ondelete = HbmOndelete.Cascade;
		}

		#endregion
	}
}