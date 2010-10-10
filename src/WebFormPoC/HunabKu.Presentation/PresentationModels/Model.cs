using System.Collections.Generic;
using System.Text;

namespace HunabKu.Presentation.PresentationModels
{
	public class Marca
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
	}

	public class Modelo
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public Marca Marca { get; set; }
		public IEnumerable<Version> Versiones { get; set; }
	}

	public class Version
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
	}

	public class CategoriaEquipamiento
	{
		public string Categoria { get; set; }
		public IEnumerable<EquipamientoInfo> Equipamiento { get; set; }
	}

	public class Equipamiento
	{
		public int Id { get; set; }
		public int VersionId { get; set; }
		public string Nombre { get; set; }
		public string Categoria { get; set; }
		public bool Tiene { get; set; }
	}

	public class EquipamientoInfo
	{
		public int Id { get; set; }
		public string Nombre { get; set; }
		public bool Tiene { get; set; }
	}

	public class ClasificadoInfo
	{
		public int MarcaId { get; set; }
		public int ModeloId { get; set; }
		public int VersionId { get; set; }
		public IEnumerable<int> Equipamiento { get; set; }
		public Usuario Usuario { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder(300);
			sb.AppendLine(string.Format("Marca={0},Modelo={1},Version={2}", MarcaId, ModeloId, VersionId));
			sb.AppendLine("Equipamiento:");
			if (Equipamiento != null)
			{
				foreach (int e in Equipamiento)
				{
					sb.Append(e).Append(',');
				}
			}
			sb.AppendLine(string.Format("Usuario={0}", Usuario));
			return sb.ToString();
		}
	}

	public class Usuario
	{
		public string Email { get; set; }
		public string Calle { get; set; }
		public override string ToString()
		{
			return string.Format("Email={0},Calle={1}", Email, Calle);
		}
	}
}