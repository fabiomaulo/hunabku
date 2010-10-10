using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace DuckTyping
{
	[TestFixture]
	public class ToDynamicFixture
	{
		public interface ISomething
		{
			string Description { get; set; }
			decimal Amount { get; set; }
		}

		public interface ISomethingWithList
		{
			string Description { get; set; }
			IEnumerable<decimal> Amounts { get; set; }
		}

		public interface ISomethingWithAnonList
		{
			string Description { get; set; }
			IEnumerable Elements { get; set; }
		}

		public interface ISomethingWithEntityList
		{
			string Description { get; set; }
			IEnumerable<ISomething> Elements { get; set; }
		}

		[Test]
		public void CanConvert()
		{
			var obj = new {Description = "something", Amount = 15.5m};
			var something = obj.AsDynamic<ISomething>();
			something.Description.Should().Be.EqualTo("something");
			something.Amount.Should().Be.EqualTo(15.5m);
		}

		[Test]
		public void CanConvertNull()
		{
			object obj = null;
			var something = obj.AsDynamic<ISomething>();
			something.Should().Be.Null();
		}

		[Test]
		public void CanConvertNullProperty()
		{
			var obj = new { Description = (string)null, Amount = 15.5m };
			var something = obj.AsDynamic<ISomething>();
			something.Description.Should().Be.Null();
			something.Amount.Should().Be.EqualTo(15.5m);
		}

		[Test]
		public void CanConvertListOfTypes()
		{
			var obj =
				new
					{
						Description = "something",
						Elements = new[] {new {Description = "s1", Amount = 1m}, new {Description = "s2", Amount = 2m}}
					};
			var something = obj.AsDynamic<ISomethingWithAnonList>();
			something.Description.Should().Be.EqualTo("something");
			int i = 0;
			foreach (var element in something.Elements)
			{
				i++;
				var s = element.AsDynamic<ISomething>();
				s.Description.Should().Be.EqualTo("s" + i);
				s.Amount.Should().Be.EqualTo((decimal) i);
			}
		}

		[Test]
		public void CanConvertListOfEntities()
		{
			var obj =
				new
					{
						Description = "something",
						Elements = new[] {new {Description = "s1", Amount = 1m}, new {Description = "s2", Amount = 2m}}
					};
			var something = obj.AsDynamic<ISomethingWithEntityList>();
			something.Description.Should().Be.EqualTo("something");
			int i = 0;
			foreach (var element in something.Elements)
			{
				i++;
				element.Description.Should().Be.EqualTo("s" + i);
				element.Amount.Should().Be.EqualTo((decimal) i);
			}
		}

		[Test]
		public void CanConvertListOfValues()
		{
			var obj = new {Description = "something", Amounts = new[] {20.5m, 15.5m}};
			var something = obj.AsDynamic<ISomethingWithList>();
			something.Description.Should().Be.EqualTo("something");
			something.Amounts.Should().Have.SameSequenceAs(new[] {20.5m, 15.5m});
		}

		[Test]
		public void ToDictionary()
		{
			var obj =
				new
					{
						Description = "something",
						Elements = new[] {new {Description = "s1", Amount = 1m}, new {Description = "s2", Amount = 2m}}
					};
			IDictionary<string, object> dic = obj.ToDictionary();
			dic.Keys.Should().Contain("Description").And.Contain("Elements");
			(dic["Elements"] as IList)[0].Should().Be.OfType<Dictionary<string, object>>()
				.And
				.ValueOf.Keys.Should().Contain("Amount");
		}

		[Test]
		public void ToDictionaryNoGenericList()
		{
			var obj = new {Description = "something", Elements = new ArrayList()};
			obj.Elements.AddRange(new[]{ new {Description = "s1", Amount = 1m}, new {Description = "s2", Amount = 2m}});
			IDictionary<string, object> dic = obj.ToDictionary();
			dic.Keys.Should().Contain("Description").And.Contain("Elements");
			(dic["Elements"] as IList)[0].Should().Be.OfType<Dictionary<string, object>>()
				.And
				.ValueOf.Keys.Should().Contain("Amount");
		}

		[Test]
		public void ToDictionaryCicularReference()
		{
			var obj = new { Description = "something", Elements = new ArrayList() };
			obj.Elements.AddRange(new[] { new { Parent = obj, Name = "s1" }, new { Parent = obj, Name = "s2" } });

			IDictionary<string, object> dic = obj.ToDictionary();
			dic.Keys.Should().Contain("Description").And.Contain("Elements");
			(dic["Elements"] as IList)[0].Should().Be.OfType<Dictionary<string, object>>()
				.And.ValueOf["Parent"]
					.Should().Be.OfType<Dictionary<string, object>>()
						.And.ValueOf["Description"].Should().Be.EqualTo("something");
			(dic["Elements"] as IList)[1].Should().Be.OfType<Dictionary<string, object>>()
				.And.ValueOf["Parent"]
					.Should().Be.OfType<Dictionary<string, object>>()
						.And.ValueOf["Description"].Should().Be.EqualTo("something");
			(dic["Elements"] as IList)[1].Should().Be.OfType<Dictionary<string, object>>()
				.And.ValueOf["Parent"]
					.Should().Be.OfType<Dictionary<string, object>>()
						.And.ValueOf["Elements"].Should().Not.Be.Null();
		}
	}
}