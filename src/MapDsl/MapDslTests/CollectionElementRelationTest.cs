using MapDsl;
using MapDsl.Loquacious;
using MapDsl.Loquacious.Impl;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class CollectionElementRelationTest
	{
		[Test]
		public void AssignOneToManyRelationship()
		{
			object relationField = null;
			var hbm = new CollectionElementRelation<Address>(new HbmMapping(), element => relationField = element);
			hbm.OneToMany();
			relationField.Should().Not.Be.Null().And.Be.OfType<HbmOneToMany>().And.ValueOf.@class.Satisfy(
				a => !string.IsNullOrEmpty(a));
		}

		[Test]
		public void AssignManyToManyRelationship()
		{
			object relationField = null;
			var hbm = new CollectionElementRelation<Address>(new HbmMapping(), element => relationField = element);
			hbm.ManyToMany();
			relationField.Should().Not.Be.Null().And.Be.OfType<HbmManyToMany>().And.ValueOf.@class.Satisfy(
				a => !string.IsNullOrEmpty(a));
		}

		[Test]
		public void AssignComponentRelationship()
		{
			object relationField = null;
			var hbm = new CollectionElementRelation<Address>(new HbmMapping(), element => relationField = element);
			hbm.Component(comp => { });
			relationField.Should().Not.Be.Null().And.Be.OfType<HbmCompositeElement>().And.ValueOf.@class.Satisfy(
				a => !string.IsNullOrEmpty(a));
		}
	}
}