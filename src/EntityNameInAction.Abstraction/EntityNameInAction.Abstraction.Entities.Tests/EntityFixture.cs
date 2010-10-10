using System;
using EntityNameInAction.Abstraction.Entities.Impl;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace EntityNameInAction.Abstraction.Entities.Tests
{
	[TestFixture]
	public class EntityFixture
	{
		[Test]
		public void Equality()
		{
			Guid id1 = Guid.NewGuid();
			Guid id2 = Guid.NewGuid();
			var e1 = new EntityMock(id1);
			var e2 = new EntityMock(id2);
			var e1Detached = new EntityMock(id1);

			Assert.That(null, Is.Not.EqualTo(e1));
			Assert.That(e2, Is.Not.EqualTo(e1));
			Assert.That(e2, Is.EqualTo(e2));
			Assert.That(e1Detached, Is.EqualTo(e1));
			Assert.That(1, Is.Not.EqualTo(e1));

			var eA = new EntityMockA(id1);
			Assert.That(eA, Is.Not.EqualTo(e1));

			var ei = new EntityMockInherit(id1);
			Assert.That(ei, Is.EqualTo(e1));
		}

		[Test]
		public void HashCode()
		{
			Guid id1 = Guid.NewGuid();
			Assert.That(new EntityMock(Guid.Empty), Is.Not.EqualTo(new EntityMock(Guid.Empty)),
			            "two transient should not have the same hash.");
			Assert.That((new EntityMock(id1)).GetHashCode(), Is.EqualTo((new EntityMock(id1)).GetHashCode()),
			            "two transient should have the same hash.");
		}
	}

	public class EntityMock : AbstractEntity<Guid>
	{
		public EntityMock(Guid id)
		{
			Id = id;
		}

		public override Guid Id { get; set; }
	}

	public class EntityMockA : AbstractEntity<Guid>
	{
		public EntityMockA(Guid id)
		{
			Id = id;
		}

		public override Guid Id { get; set; }
	}

	public class EntityMockInherit : EntityMock
	{
		public EntityMockInherit(Guid id) : base(id) {}
	}
}