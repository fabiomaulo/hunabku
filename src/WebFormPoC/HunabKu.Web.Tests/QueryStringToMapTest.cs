using System;
using System.Linq;
using NUnit.Framework;
using System.Collections.Specialized;
using SharpTestsEx;

namespace HunabKu.Web.Tests
{
	[TestFixture]
	public class QueryStringToMapTest
	{
		[Test]
		public void PlainProperties()
		{
			var nvc = new NameValueCollection { { "p1", "v1" }, { "p2", "v2" } };
			var qsm = new QueryStringToGraph();
			var pg = qsm.Parse(nvc);
			pg.Properties.Should().Have.Count.EqualTo(2);
			pg.Properties.Select(p => p.PropertyName).Should().Have.SameValuesAs("p1", "p2");
			pg.Properties.Select(p => p.Value).Should().Have.SameValuesAs("v1", "v2");
		}

		[Test]
		public void PlainPropertiesWithCollection()
		{
			var nvc = new NameValueCollection { { "p1", "v1" }, { "p1", "v2" } };
			var qsm = new QueryStringToGraph();
			var pg = qsm.Parse(nvc);
			pg.Properties.Should().Have.Count.EqualTo(1);
			pg.Properties.Select(p => p.PropertyName).First().Should().Be.EqualTo("p1");
			pg.Properties.Select(p => p.Value).First().Should().Be.OfType<string[]>().And.Value.Should().Have.SameValuesAs("v1", "v2");
		}

		[Test]
		public void TwoLevelsProperties()
		{
			var nvc = new NameValueCollection { { "p1", "v1" }, { "p2.p22", "v2" } };
			var qsm = new QueryStringToGraph();
			var pg = qsm.Parse(nvc);
			pg.Properties.Should().Have.Count.EqualTo(2);
			var complex = pg.Properties.First(p => p.PropertyName == "p2");
			complex.PropertyType.Should().Be.EqualTo(PropertyType.ParamObject);
			var complexLevel2 = complex.Value.Should().Be.OfType<ParamObject>().And.Value;
			complexLevel2.Properties.Count.Should().Be.EqualTo(1);
			var prop = complexLevel2.Properties.First();
			prop.PropertyName.Should().Be.EqualTo("p22");
			prop.Value.Should().Be.EqualTo("v2");
		}

		[Test]
		public void TwoLevelsTwoProperties()
		{
			var nvc = new NameValueCollection { { "p2.p21", "v1" }, { "p2.p22", "v2" } };
			var qsm = new QueryStringToGraph();
			var pg = qsm.Parse(nvc);
			pg.Properties.Should().Have.Count.EqualTo(1);
			var complex = pg.Properties.First(p => p.PropertyName == "p2");
			complex.PropertyType.Should().Be.EqualTo(PropertyType.ParamObject);
			var complexLevel2 = complex.Value.Should().Be.OfType<ParamObject>().And.Value;
			complexLevel2.Properties.Count.Should().Be.EqualTo(2);
			complexLevel2.Properties.First(p => p.PropertyName == "p22").Value.Should().Be.EqualTo("v2");
			complexLevel2.Properties.First(p => p.PropertyName == "p21").Value.Should().Be.EqualTo("v1");
		}
	}
}