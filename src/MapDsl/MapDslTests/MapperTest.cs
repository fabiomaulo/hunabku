using System;
using System.Linq;
using MapDsl;
using MapDsl.Loquacious;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class MapperTest
	{
		[Test]
		public void CanCreateHbmMapping()
		{
			var map = new Mapper();
			map.GetCompiledMapping().Should().Not.Be.Null();
		}

		[Test]
		public void MappingHasDefaultAssembly()
		{
			var map = new Mapper();
			map.Assembly(typeof (Animal).Assembly);
			ActionAssert.Throws<ArgumentNullException>(() => map.Assembly(null));
			var hbm = map.GetCompiledMapping();
			hbm.assembly.Should().Be.EqualTo("MapDsl");
		}

		[Test]
		public void MappingHasDefaultNamespace()
		{
			var map = new Mapper();
			map.NameSpace(typeof(Animal).Namespace);
			ActionAssert.Throws<ArgumentNullException>(() => map.NameSpace(null));
			ActionAssert.Throws<ArgumentNullException>(() => map.NameSpace(string.Empty));
			var hbm = map.GetCompiledMapping();
			hbm.@namespace.Should().Be.EqualTo("MapDsl");
		}

		[Test]
		public void CanAddClassMappingWithGenerator()
		{
			var map = new Mapper();
			map.Class<Animal, long>(animal => animal.Id, idm => idm.Generator = Generators.Native, animal => { });
			var rcs = map.GetCompiledMapping().RootClasses;
			rcs.Should().Have.Count.EqualTo(1);
			rcs[0].Id.generator.Should().Not.Be.Null();
		}

		[Test]
		public void CanAddSubClassMapping()
		{
			var map = new Mapper();
			map.Subclass<PettingZoo>(pettingZoo => { });
			var subclasses = map.GetCompiledMapping().SubClasses;
			subclasses.Should().Have.Count.EqualTo(1);
			subclasses[0].extends.Satisfy(ex => !string.IsNullOrEmpty(ex) && ex.Contains(typeof(Zoo).FullName));
		}

		[Test]
		public void CanAddJoinedSubClassMapping()
		{
			var map = new Mapper();
			map.JoinedSubclass<Reptile>(reptile => { });
			map.JoinedSubclass<Lizard>(reptile => { });
			var joinedSubclasses = map.GetCompiledMapping().JoinedSubclasses;
			joinedSubclasses.Should().Have.Count.EqualTo(2);
			joinedSubclasses[0].extends.Satisfy(ex => !string.IsNullOrEmpty(ex) && ex.Contains(typeof(Animal).FullName));
			joinedSubclasses[1].extends.Satisfy(ex => !string.IsNullOrEmpty(ex) && ex.Contains(typeof(Reptile).FullName));
			joinedSubclasses.All(js => js.Satisfy(j => js.key != null && js.key.Columns.Any()));
			joinedSubclasses.Select(js => js.key.Columns.Single().name)
				.Should("The name of each key-column should be the BaseType name").Have.SameValuesAs(typeof(Animal).Name.ToLowerInvariant() + "_key", typeof(Reptile).Name.ToLowerInvariant() + "_key");
		}
	}
}