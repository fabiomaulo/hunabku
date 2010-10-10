using System;
using System.Collections;
using NHibernate.Collection;
using NHibernate.Collection.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.UserTypes;
using Iesi.Collections.Generic;

namespace Reparenting
{
	public class TreeNodesCollectionType: IUserCollectionType
	{
		#region Implementation of IUserCollectionType

		public IPersistentCollection Instantiate(ISessionImplementor session, ICollectionPersister persister)
		{
			return new PersistentNodeSet(session);
		}

		public IPersistentCollection Wrap(ISessionImplementor session, object collection)
		{
			return new PersistentNodeSet(session, (ISet<Node>)collection);
		}

		public IEnumerable GetElements(object collection)
		{
			return (IEnumerable)collection;
		}

		public bool Contains(object collection, object entity)
		{
			return ((ISet<Node>)collection).Contains((Node)entity);
		}

		public object IndexOf(object collection, object entity)
		{
			throw new NotSupportedException();
		}

		public object ReplaceElements(object original, object target, ICollectionPersister persister, object owner, IDictionary copyCache, ISessionImplementor session)
		{
			var result = (ISet<Node>)target;
			result.Clear();

			foreach (var o in (IEnumerable)original)
				result.Add((Node)o);

			return result;
		}

		public object Instantiate(int anticipatedSize)
		{
			return new HashedSet<Node>();
		}

		#endregion
	}
}