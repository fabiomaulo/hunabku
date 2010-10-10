using MapDsl;
using MapDsl.Loquacious;
using MapDsl.Loquacious.Impl;
using NUnit.Framework;
using NHibernate.Cfg.MappingSchema;
using SharpTestsEx;

namespace MapDslTests
{
	public class SetMapperTest
	{
		[Test]
		public void SetInverse()
		{
			var hbm = new HbmSet();
			var mapper = new SetMapper<Animal, Animal>(hbm);
			mapper.Inverse = true;
			hbm.Inverse.Should().Be.True();
			mapper.Inverse = false;
			hbm.Inverse.Should().Be.False();
		}

		[Test]
		public void SetMutable()
		{
			var hbm = new HbmSet();
			var mapper = new SetMapper<Animal, Animal>(hbm);
			mapper.Mutable = true;
			hbm.Mutable.Should().Be.True();
			mapper.Mutable = false;
			hbm.Mutable.Should().Be.False();
		}

		[Test]
		public void SetWhere()
		{
			var hbm = new HbmSet();
			var mapper = new SetMapper<Animal, Animal>(hbm);
			mapper.Where = "c > 10";
			hbm.Where.Should().Be.EqualTo("c > 10");
		}

		[Test]
		public void SetBatchSize()
		{
			var hbm = new HbmSet();
			var mapper = new SetMapper<Animal, Animal>(hbm);
			mapper.BatchSize = 10;
			hbm.BatchSize.Should().Be.EqualTo(10);
		}

		[Test]
		public void SetLazy()
		{
			var hbm = new HbmSet();
			var mapper = new SetMapper<Animal, Animal>(hbm);
			mapper.Lazy = CollectionLazy.Extra;
			hbm.Lazy.Should().Be.EqualTo(HbmCollectionLazy.Extra);
			mapper.Lazy = CollectionLazy.NoLazy;
			hbm.Lazy.Should().Be.EqualTo(HbmCollectionLazy.False);
			mapper.Lazy = CollectionLazy.Lazy;
			hbm.Lazy.Should().Be.EqualTo(HbmCollectionLazy.True);
		}

		[Test]
		public void SetSort()
		{
			var hbm = new HbmSet();
			var mapper = new SetMapper<Animal, Animal>(hbm);
			mapper.Sort();
			hbm.Sort.Should().Be.EqualTo("natural");
		}

		[Test]
		public void CallKeyMapper()
		{
			var hbm = new HbmSet();
			var mapper = new SetMapper<Animal, Animal>(hbm);
			bool kmCalled = false;
			mapper.Key(km=> kmCalled= true);
			hbm.Key.Should().Not.Be.Null();
			kmCalled.Should().Be.True();
		}
	}
}