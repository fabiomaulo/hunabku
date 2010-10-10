using System;
using System.Collections.Generic;
using HunabKu.Presentation.PresentationModels;
using HunabKu.Presentation.Presenters;
using HunabKu.Presentation.Utils;
using HunabKu.Presentation.Views;
using HunabKu.Web;

namespace WebFormPoC
{
	public partial class ClasificadoCreate : View, IPublicationView
	{
		private PublicationPresenter presenter;

		protected void Page_Load(object sender, EventArgs e)
		{
			presenter = new PublicationPresenter(this);
			if(Request.IsAjaxRequest())
			{
				this.ResolveAjaxCallBack();
				return;
			}			
			if (!Page.IsPostBack)
			{
				presenter.InitView(Request.QueryString);
			}
		}

		public void LoadModelos(int marca)
		{
			JsonResponse(presenter.GetModelos(marca));
		}

		public void LoadVersiones(int modelo)
		{
			JsonResponse(presenter.GetVersiones(modelo));
		}

		public void LoadEquipamiento(int version)
		{
			JsonResponse(presenter.GetEquipamiento(version));
		}

		public void LoadUsuario(string email)
		{
			JsonResponse(presenter.GetUsuario(email));
		}

		public IEnumerable<NameValueElement> Marcas
		{
			set
			{
				rptMarcas.DataSource = value;
				rptMarcas.DataBind();
			}
		}

		public void Publica(ClasificadoInfo clasificado)
		{
			JsonResponse(presenter.Register(clasificado));
		}
	}
}