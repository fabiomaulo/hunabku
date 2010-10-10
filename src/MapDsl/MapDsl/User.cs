using System.Collections.Generic;

namespace MapDsl
{
	public class User
	{
		public virtual long Id { get; set; }

		public virtual string UserName { get; set; }

		public virtual Human Human { get; set; }

		public virtual IList<string> Permissions { get; set; }
	}
}