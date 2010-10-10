using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using HunabKu.Presentation.PresentationModels;
using HunabKu.Presentation.Presenters;
using HunabKu.Presentation.Views;

namespace WebFormPoC
{
	public partial class Publicado : Page, IPublicadosView
	{
		private PublicadosPresenter presenter;

		#region IPublicadosView Members

		public IEnumerable<ClasificadoInfo> Clasificados
		{
			set
			{
				rptClasificados.DataSource = value.Select(c => c.ToString());
				rptClasificados.DataBind();
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			presenter = new PublicadosPresenter(this);
			if (!Page.IsPostBack)
			{
				presenter.InitView(Request.QueryString);
			}
		}
	}
}