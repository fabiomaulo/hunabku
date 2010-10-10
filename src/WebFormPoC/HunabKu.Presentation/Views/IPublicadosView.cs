using System.Collections.Generic;
using HunabKu.Presentation.PresentationModels;

namespace HunabKu.Presentation.Views
{
	public interface IPublicadosView
	{
		IEnumerable<ClasificadoInfo> Clasificados { set; }
	}
}