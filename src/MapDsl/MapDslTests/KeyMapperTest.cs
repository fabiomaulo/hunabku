using System.Linq;
using MapDsl.Loquacious.Impl;
using NHibernate.Cfg.MappingSchema;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class KeyMapperTest
	{
		[Test]
		public void SetNotNull()
		{
			var hbm = new HbmKey();
			var mapper = new KeyMapper<string>(hbm);
			mapper.NotNull = true;
			hbm.IsNullable.Should().Have.Value();
			hbm.IsNullable.Value.Should().Be.False();
		}

		[Test]
		public void SetUpdate()
		{
			var hbm = new HbmKey();
			var mapper = new KeyMapper<string>(hbm);
			mapper.Update = false;
			hbm.update.Should().Be.False();
			hbm.updateSpecified.Should().Be.True();
		}

		[Test]
		public void SetUnique()
		{
			var hbm = new HbmKey();
			var mapper = new KeyMapper<string>(hbm);
			mapper.Unique = true;
			hbm.unique.Should().Be.True();
			hbm.uniqueSpecified.Should().Be.True();
		}

		[Test]
		public void SetColumn()
		{
			var hbm = new HbmKey();
			var mapper = new KeyMapper<string>(hbm);
			mapper.Column = "peppe";
			hbm.Columns.Should().Have.Count.EqualTo(1);
			hbm.Columns.First().name.Should().Be.EqualTo("peppe");
		}

		[Test]
		public void SetOndelete()
		{
			var hbm = new HbmKey();
			var mapper = new KeyMapper<string>(hbm);
			mapper.OnDeleteCascade();
			hbm.ondelete.Should().Be.EqualTo(HbmOndelete.Cascade);
		}
	}
}