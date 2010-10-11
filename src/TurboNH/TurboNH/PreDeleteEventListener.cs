using System;
using Iesi.Collections;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Util;

namespace TurboNH
{
	[Serializable]
	public class PreDeleteEventListener : IDeleteEventListener
	{
		public void OnDelete(DeleteEvent @event)
		{
			OnDelete(@event, new IdentitySet());
		}

		public void OnDelete(DeleteEvent @event, ISet transientEntities)
		{
			var session = @event.Session;
			EntityEntry entry = session.PersistenceContext.GetEntry(@event.Entity);
			if (entry != null && entry.RowId is EntityTracer)
			{
				entry.BackSetStatus(Status.Loaded);
			}
		}
	}
}