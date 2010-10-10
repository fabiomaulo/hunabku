using System.Collections.Generic;
using System.Linq;
using MapDsl;
using MapDsl.Loquacious.Impl;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class AbstractPropertyContainerMapperTest
	{
		[Test]
		public void CanAddSimpleProperty()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<StateProvince>(properties);
			map.Property(sp => sp.Name);

			properties.Should().Have.Count.EqualTo(1);
			properties.First().Should().Be.OfType<HbmProperty>().And.ValueOf.Name.Should().Be.EqualTo("Name");
		}

		[Test]
		public void CanAddComponentProperty()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Zoo>(properties);
			map.Component(zoo => zoo.Address, comp => { });

			properties.Should().Have.Count.EqualTo(1);
			properties.First().Should().Be.OfType<HbmComponent>().And.ValueOf.Name.Should().Be.EqualTo("Address");
		}

		[Test]
		public void CanAddComponentPropertyAndItsProperties()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Zoo>(properties);
			map.Component(zoo => zoo.Address, comp =>
				{
					comp.Property(address => address.City);
					comp.Property(address => address.PostalCode);
				});

			properties.Should().Have.Count.EqualTo(1);
			var component = (HbmComponent) properties.First();
			component.Items.Should().Have.Count.EqualTo(2);
		}

		[Test]
		public void CanAddManyToOneProperty()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Animal>(properties);
			map.ManyToOne(animal => animal.Father);

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmManyToOne>().And.ValueOf.Name.Should().Be.EqualTo("Father");
		}

		[Test]
		public void CanAddOneToOneProperty()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<User>(properties);
			map.OneToOne(user => user.Human, rm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmOneToOne>().And.ValueOf.Name.Should().Be.EqualTo("Human");
		}

		[Test]
		public void OneToOneExecuteRelationMapping()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<User>(properties);
			bool executed = false;
			map.OneToOne(user => user.Human, rm => { executed = true; });
			executed.Should().Be.True();
		}

		[Test]
		public void CanAddSetProperty()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Animal>(properties);
			map.Set(animal => animal.Offspring, cm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmSet>().And.ValueOf.Name.Should().Be.EqualTo("Offspring");
		}

		[Test]
		public void CanAddBagProperty()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Human>(properties);
			map.Bag(human => human.Friends, cm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmBag>().And.ValueOf.Name.Should().Be.EqualTo("Friends");
		}

		[Test]
		public void CanAddListProperty()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<User>(properties);
			map.List(user => user.Permissions, cm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmList>().And.ValueOf.Name.Should().Be.EqualTo("Permissions");
		}

		[Test]
		public void CanAddMapProperty()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Zoo>(properties);
			map.Map(zoo => zoo.Mammals, cm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmMap>().And.ValueOf.Name.Should().Be.EqualTo("Mammals");
		}

		[Test]
		public void WhenNoRelationThenAddSetPropertyWithElement()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Human>(properties);
			map.Set(human => human.NickNames, cm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmSet>().And.ValueOf.ElementRelationship.Should().Be.OfType<HbmElement>().And.ValueOf.Type.
				name.Should().Be.EqualTo("String");
		}

		[Test]
		public void WhenNoRelationThenAddBagPropertyWithElement()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Human>(properties);
			map.Bag(human => human.NickNames, cm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmBag>().And.ValueOf.ElementRelationship.Should().Be.OfType<HbmElement>().And.ValueOf.Type.
				name.Should().Be.EqualTo("String");
		}

		[Test]
		public void WhenNoRelationThenAddListPropertyWithElement()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<User>(properties);
			map.List(user => user.Permissions, cm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmList>().And.ValueOf.ElementRelationship.Should().Be.OfType<HbmElement>().And.ValueOf.Type
				.name.Should().Be.EqualTo("String");
		}

		[Test]
		public void WhenNoRelationThenAddMapPropertyWithElement()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<MyEntity>(properties);
			map.Map(myEntity => myEntity.Dictionary, cm => { });

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmMap>().And.ValueOf.ElementRelationship.Should().Be.OfType<HbmElement>().And.ValueOf.Type.
				name.Should().Be.EqualTo("String");
		}

		[Test]
		public void WhenRelationThenAddSetPropertyWithSpecificRelation()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Animal>(properties);
			map.Set(animal => animal.Offspring, cm => { }, rel => rel.OneToMany());

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmSet>().And.ValueOf.ElementRelationship.Should().Be.OfType<HbmOneToMany>();
		}

		[Test]
		public void WhenRelationThenAddBagPropertyWithSpecificRelation()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Human>(properties);
			map.Bag(human => human.Pets, cm => { }, rel => rel.OneToMany());

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmBag>().And.ValueOf.ElementRelationship.Should().Be.OfType<HbmOneToMany>();
		}

		[Test]
		public void WhenRelationThenAddListPropertyWithSpecificRelation()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Human>(properties);
			map.List(human => human.Pets, cm => { }, rel => rel.OneToMany());

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmList>().And.ValueOf.ElementRelationship.Should().Be.OfType<HbmOneToMany>();
		}

		[Test]
		public void WhenRelationThenAddMapPropertyWithSpecificRelation()
		{
			var properties = new List<object>();
			var map = new StubPropertyContainerMapper<Human>(properties);
			map.Map(human => human.Family, cm => { }, rel => rel.ManyToMany());

			properties.Should().Have.Count.EqualTo(1);
			object first = properties.First();
			first.Should().Be.OfType<HbmMap>().And.ValueOf.ElementRelationship.Should().Be.OfType<HbmManyToMany>();
		}

		#region Nested type: MyEntity

		private class MyEntity
		{
			public IDictionary<string, string> Dictionary { get; set; }
		}

		#endregion

		#region Nested type: StubPropertyContainerMapper

		private class StubPropertyContainerMapper<T> : AbstractPropertyContainerMapper<T> where T : class
		{
			private readonly ICollection<object> elements;

			public StubPropertyContainerMapper(ICollection<object> elements) : base(new HbmMapping())
			{
				this.elements = elements;
			}

			#region Overrides of AbstractClassMapping<StateProvince>

			protected override void AddProperty(object property)
			{
				elements.Add(property);
			}

			#endregion
		}

		#endregion
	}
}