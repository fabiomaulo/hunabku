using System;
using System.ComponentModel;
using NHibernate.Engine;
using NHibernate.Event;

namespace TurboNH
{
	[Serializable]
	public class PostLoadEventListener : IPostLoadEventListener
	{
		public void OnPostLoad(PostLoadEvent @event)
		{
			var trackableEntity = @event.Entity as INotifyPropertyChanged;
			if(trackableEntity != null)
			{
				EntityEntry entry = @event.Session.PersistenceContext.GetEntry(@event.Entity);
				entry.BackSetStatus(Status.ReadOnly);
				entry.BackSetTracer(new EntityTracer(entry, trackableEntity));
			}
		}
	}

	public class EntityTracer
	{
		public EntityTracer(EntityEntry entry, INotifyPropertyChanged trackableEntity)
		{
			trackableEntity.PropertyChanged += (sender, e) => entry.BackSetStatus(Status.Loaded);
		}
	}
}