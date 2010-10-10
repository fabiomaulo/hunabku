using System.Collections.Generic;

namespace HunabKu.Presentation.PresentationModels
{
	public class Repository
	{
		public static readonly List<ClasificadoInfo> ClasificadoRepository;
		public static readonly List<Equipamiento> EquipamientoRepository;
		private static readonly Marca fiatMarca = new Marca {Id = 2, Nombre = "Fiat"};
		private static readonly Marca fordMarca = new Marca {Id = 1, Nombre = "Ford"};
		public static readonly List<Marca> MarcaRepository;
		public static readonly List<Modelo> ModeloRepository;
		public static readonly List<Usuario> UsuarioRepository;

		static Repository()
		{
			MarcaRepository = new List<Marca> {fordMarca, fiatMarca};
			ModeloRepository = new List<Modelo>
			                   	{
			                   		new Modelo
			                   			{
			                   				Id = 1,
			                   				Nombre = "Focus",
			                   				Marca = fordMarca,
			                   				Versiones = new[] {new Version {Id = 1, Nombre = "4p Abiente 1.6"}}
			                   			},
			                   		new Modelo
			                   			{
			                   				Id = 2,
			                   				Nombre = "Mondeo",
			                   				Marca = fordMarca,
			                   				Versiones =
			                   					new[] {new Version {Id = 2, Nombre = "CLX 5P"}, new Version {Id = 3, Nombre = "Ghia 2.0"}}
			                   			},
			                   		new Modelo
			                   			{
			                   				Id = 3,
			                   				Nombre = "Cinquecento",
			                   				Marca = fiatMarca,
			                   				Versiones = new[] {new Version {Id = 4, Nombre = "1.1 3P"}}
			                   			},
			                   		new Modelo
			                   			{
			                   				Id = 4,
			                   				Nombre = "Punto",
			                   				Marca = fiatMarca,
			                   				Versiones =
			                   					new[]
			                   						{
			                   							new Version {Id = 5, Nombre = "5P ELX 1.3 16V"},
			                   							new Version {Id = 6, Nombre = "5P ELX 1.4 Top"}
			                   						}
			                   			}
			                   	};
			EquipamientoRepository = new List<Equipamiento>
			                         	{
			                         		new Equipamiento
			                         			{
			                         				Id = 1,
			                         				Categoria = "Comunicación y entretenimiento",
			                         				Nombre = "4 Parlantes",
			                         				Tiene = true,
			                         				VersionId = 6
			                         			},
			                         		new Equipamiento
			                         			{
			                         				Id = 2,
			                         				Categoria = "Comunicación y entretenimiento",
			                         				Nombre = "Radio AM - FM y CD con lector de Mp3 y tarjetas SD",
			                         				Tiene = false,
			                         				VersionId = 6
			                         			},
			                         		new Equipamiento
			                         			{
			                         				Id = 3,
			                         				Categoria = "Comunicación y entretenimiento",
			                         				Nombre = "Radio AM - FM",
			                         				Tiene = true,
			                         				VersionId = 6
			                         			},
			                         		new Equipamiento
			                         			{
			                         				Id = 4,
			                         				Categoria = "Confort",
			                         				Nombre = "Aire acondicionado",
			                         				Tiene = true,
			                         				VersionId = 6
			                         			},
			                         		new Equipamiento
			                         			{
			                         				Id = 5,
			                         				Categoria = "Confort",
			                         				Nombre = "Tapizados de cuero",
			                         				Tiene = false,
			                         				VersionId = 6
			                         			},
			                         		new Equipamiento
			                         			{Id = 6, Categoria = "Motor", Nombre = "GNC", Tiene = false, VersionId = 6}
			                         	};
			UsuarioRepository = new List<Usuario>
			                    	{
			                    		new Usuario {Email = "franco@gmail.com", Calle = "Av. Cabildo, 1234"},
			                    		new Usuario {Email = "merlo@yahoo.com", Calle = "Av. Santa Fe, 456"}
			                    	};
			ClasificadoRepository = new List<ClasificadoInfo>();
		}
	}
}