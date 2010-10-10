using System;
using NHibernate.Engine;
using NHibernate.Event;

namespace DisableAutoDirtyCheck
{
	[Serializable]
	public class PostLoadEventListener : IPostLoadEventListener
	{
		public void OnPostLoad(PostLoadEvent @event)
		{
			EntityEntry entry = @event.Session.PersistenceContext.GetEntry(@event.Entity);
			entry.BackSetStatus(Status.ReadOnly);
		}
	}
}