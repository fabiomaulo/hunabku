using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using HunabKu.Presentation.PresentationModels;
using HunabKu.Presentation.Utils;
using HunabKu.Presentation.Views;

namespace HunabKu.Presentation.Presenters
{
	public class PublicationPresenter
	{
		public PublicationPresenter(IPublicationView view)
		{
			View = view;
		}

		public IPublicationView View { get; private set; }

		public void InitView(NameValueCollection queryString)
		{
			View.Marcas = Repository.MarcaRepository.ToNameValue(m => m.Id, m => m.Nombre);
		}

		public IEnumerable<NameValueElement> GetModelos(int marcaId)
		{
			return Repository.ModeloRepository.Where(m => m.Marca.Id == marcaId).ToNameValue(m => m.Id, m => m.Nombre);
		}

		public IEnumerable<NameValueElement> GetVersiones(int modeloId)
		{
			return
				Repository.ModeloRepository.Where(m => m.Id == modeloId).SelectMany(m => m.Versiones).ToNameValue(m => m.Id,
				                                                                                                  m => m.Nombre);
		}

		public IEnumerable<CategoriaEquipamiento> GetEquipamiento(int versionId)
		{
			var result = Repository.EquipamientoRepository.Where(e => e.VersionId == versionId).GroupBy(e => e.Categoria,
																																		(c, ee) =>
																																		new CategoriaEquipamiento { Categoria = c, Equipamiento = ee.Select(x => new EquipamientoInfo { Id = x.Id, Nombre = x.Nombre, Tiene = x.Tiene }) }).ToArray();
			return result.Any() ? result : EquipamientoDefault();
		}

		private static CategoriaEquipamiento[] EquipamientoDefault()
		{
			return new[] { new CategoriaEquipamiento { Categoria = "Minimo", Equipamiento = new[] { new EquipamientoInfo{Id=9999,Nombre="4 ruedas", Tiene = true} }}};
		}

		public Usuario GetUsuario(string email)
		{
			var usuario = Repository.UsuarioRepository.Where(u => u.Email == email).FirstOrDefault();
			return usuario ?? new Usuario {Email = string.Empty, Calle = string.Empty};
		}

		public IEnumerable<InvalidValue> Register(ClasificadoInfo clasificado)
		{
			List<InvalidValue> result = Validate(clasificado);
			if (result.Count == 0)
			{
				Repository.ClasificadoRepository.Add(clasificado);				
			}
			return result;
		}

		private List<InvalidValue> Validate(ClasificadoInfo clasificado)
		{
			var result = new List<InvalidValue>(10);
			if (string.IsNullOrEmpty(clasificado.Usuario.Email))
			{
				result.Add(new InvalidValue {Message = "Sin e-mail no se quien sos."});
			}
			if (clasificado.MarcaId <= 0)
			{
				result.Add(new InvalidValue {Message = "Marca ? lo ensemblaste vos?"});
			}
			if (clasificado.ModeloId <= 0)
			{
				result.Add(new InvalidValue {Message = "Modelo ?"});
			}
			if (clasificado.VersionId <= 0)
			{
				result.Add(new InvalidValue {Message = "Version ?"});
			}
			if (clasificado.Equipamiento == null || clasificado.Equipamiento.Count() <= 0)
			{
				result.Add(new InvalidValue {Message = "Por lo menos debería tener las cuatro ruedas."});
			}
			return result;
		}
	}
}