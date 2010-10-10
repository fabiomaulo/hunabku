using MapDsl;
using MapDsl.Loquacious;
using MapDsl.Loquacious.Impl;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class ClassMapperTest
	{
		[Test]
		public void AddClassElementToMappingDocument()
		{
			var mapdoc = new HbmMapping();
			new ClassMapper<Animal>(mapdoc, TypeUtils.DecodeMemberAccessExpression<Animal, long>(a => a.Id));
			mapdoc.RootClasses.Should().Have.Count.EqualTo(1);
		}

		[Test]
		public void ClassElementHasName()
		{
			var mapdoc = new HbmMapping();
			new ClassMapper<Animal>(mapdoc, TypeUtils.DecodeMemberAccessExpression<Animal, long>(a => a.Id));
			mapdoc.RootClasses[0].Name.Should().Not.Be.Null();
		}

		[Test]
		public void ClassElementHasIdElement()
		{
			var mapdoc = new HbmMapping();
			new ClassMapper<Animal>(mapdoc, TypeUtils.DecodeMemberAccessExpression<Animal, long>(a => a.Id));
			var hbmId = mapdoc.RootClasses[0].Id;
			hbmId.Should().Not.Be.Null();
			hbmId.name.Should().Be.EqualTo("Id");
		}

		[Test]
		public void CanSetDistriminator()
		{
			var mapdoc = new HbmMapping();
			var rc = new ClassMapper<Zoo>(mapdoc, TypeUtils.DecodeMemberAccessExpression<Zoo, long>(a => a.Id));
			rc.Discriminator();
			mapdoc.RootClasses[0].discriminator.Should().Not.Be.Null();
		}
	}
}