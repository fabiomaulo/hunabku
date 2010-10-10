using MapDsl;
using MapDsl.Loquacious.Impl;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class OneToOneMapperTest
	{
		[Test]
		public void SetConstrained()
		{
			var hbm = new HbmOneToOne();
			var mapper = new OneToOneMapper<Human>(hbm);
			mapper.Constrained();
			hbm.Satisfy(a => hbm.constrained);
		}
	}
}