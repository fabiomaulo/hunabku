using System.Collections.Specialized;
using HunabKu.Presentation.PresentationModels;
using HunabKu.Presentation.Views;

namespace HunabKu.Presentation.Presenters
{
	public class PublicadosPresenter
	{
		public PublicadosPresenter(IPublicadosView view)
		{
			View = view;
		}

		public IPublicadosView View { get; private set; }

		public void InitView(NameValueCollection queryString)
		{
			View.Clasificados = Repository.ClasificadoRepository;
		}
	}
}