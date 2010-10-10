using System.Linq;
using MapDsl;
using MapDsl.Loquacious;
using MapDsl.Loquacious.Impl;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class ComponentMappingTest
	{
		[Test]
		public void WhenNoDefaultAssignComponentClassAssemblyQualifiedName()
		{
			var mapping = new HbmMapping();
			var hbm = new HbmComponent();
			new ComponentMapper<Address>(mapping, hbm);
			hbm.Class.Should().Be.EqualTo(typeof (Address).AssemblyQualifiedName);
		}

		[Test]
		public void WhenDefaultAssemblyAssignComponentClassFullName()
		{
			var mapping = new HbmMapping {assembly = typeof (Address).Assembly.GetName().Name};
			var hbm = new HbmComponent();
			new ComponentMapper<Address>(mapping, hbm);
			hbm.Class.Should().Be.EqualTo(typeof(Address).FullName);
		}

		[Test]
		public void WhenDefaultNamespaceAssignComponentClassNameAndAssembly()
		{
			var mapping = new HbmMapping { @namespace = typeof(Address).Namespace};
			var hbm = new HbmComponent();
			new ComponentMapper<Address>(mapping, hbm);
			hbm.Class.Should().Be.EqualTo("Address, " + typeof(Address).Assembly.GetName().Name);
		}

		[Test]
		public void WhenDefaultNamespaceAndDefaultAssemblyAssignComponentClassName()
		{
			var mapping = new HbmMapping { assembly = typeof(Address).Assembly.GetName().Name, @namespace = typeof(Address).Namespace };
			var hbm = new HbmComponent();
			new ComponentMapper<Address>(mapping, hbm);
			hbm.Class.Should().Be.EqualTo("Address");
		}

		[Test]
		public void CanAddSimpleProperty()
		{
			var mapping = new HbmMapping();
			var hbm = new HbmComponent();
			var map = new ComponentMapper<Address>(mapping, hbm);
			map.Property(address => address.City);
			hbm.Items.Should().Have.Count.EqualTo(1);
			hbm.Items.First().Should().Be.OfType<HbmProperty>().And.ValueOf.Name.Should().Be.EqualTo("City");
		}
	}
}