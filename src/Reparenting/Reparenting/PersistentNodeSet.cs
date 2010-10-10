using System.Linq;
using Iesi.Collections.Generic;
using NHibernate.Collection.Generic;
using NHibernate.Engine;

namespace Reparenting
{
	public class PersistentNodeSet : PersistentGenericSet<Node>
	{
		public PersistentNodeSet() {}

		public PersistentNodeSet(ISessionImplementor session) : base(session) {}

		public PersistentNodeSet(ISessionImplementor session, ISet<Node> original) : base(session, original) {}

		public override System.Collections.ICollection GetOrphans(object snapshot, string entityName)
		{
			return base.GetOrphans(snapshot, entityName)
				.Cast<Node>()
				.Where(n=> ReferenceEquals(null,n.Parent))
				.ToArray();
		}
	}
}