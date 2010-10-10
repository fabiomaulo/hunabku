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

namespace MapDsl.Loquacious
{
	public interface ICollectionPropertiesMapper<TEntity, TElement> where TEntity : class
	{
		bool Inverse { get; set; }
		bool Mutable { get; set; }
		string Where { get; set; }
		int BatchSize { get; set; }
		CollectionLazy Lazy { get; set; }
		void Key(Action<IKeyMapper<TEntity>> keyMapping);
		void OrderBy<TProperty>(Expression<Func<TElement, TProperty>> property);
		void Sort();
	}
}