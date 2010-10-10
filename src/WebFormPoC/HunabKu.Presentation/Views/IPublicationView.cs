using System.Collections.Generic;
using HunabKu.Presentation.Utils;

namespace HunabKu.Presentation.Views
{
	public interface IPublicationView
	{
		IEnumerable<NameValueElement> Marcas { set; }
	}
}