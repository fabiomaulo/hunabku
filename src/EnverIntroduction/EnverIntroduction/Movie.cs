using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace EnverIntroduction
{
	public class Movie : Product
	{
		private Iesi.Collections.Generic.ISet<ActorRole> actors;
		public Movie()
		{
			actors = new HashedSet<ActorRole>();
		}
		public virtual string Director { get; set; }
		public virtual ICollection<ActorRole> Actors
		{
			get { return actors; }
		}
	}
}