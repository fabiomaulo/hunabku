using MapDsl.Loquacious;
using MapDsl.Loquacious.Impl;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class IdMappingTest
	{
		[Test]
		public void CanSetGenerator()
		{
			var hbmId = new HbmId();
			new IdMapper(hbmId) {Generator = Generators.Native};
			hbmId.generator.Should().Not.Be.Null();
		}
	}
}