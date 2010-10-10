using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using NUnit.Framework;
using SharpTestsEx;

namespace HunabKu.Web.Tests
{
	[TestFixture]
	public class ParameterBinderTest
	{
		public class MyClass
		{
			public void A() { }
			public void A(int p1)
			{
				P1 = p1;
			}
			public void A(int p1, short p2)
			{
				P1 = p1;
				P2 = p2;
			}
			public int P1 { get; set; }
			public short P2 { get; set; }
		}

		[Test]
		public void Ctor()
		{
			ActionAssert.Throws<ArgumentNullException>(() => new ParameterBinder(null, null, null));
			ActionAssert.Throws<ArgumentNullException>(() => new ParameterBinder(typeof(MyClass), null, null));
			ActionAssert.Throws<ArgumentNullException>(() => new ParameterBinder(typeof(MyClass), "", null));
			ActionAssert.Throws<ArgumentNullException>(() => new ParameterBinder(null, "A", null));
		}

		[Test]
		public void WhenNoParams_ChooseMethodWithNoParams()
		{
			var p = new ParameterBinder(typeof (MyClass), "A", null);
			var expected = typeof (MyClass).GetMethod("A", new Type[0]);

			p.Method.Should().Be.EqualTo(expected);
		}

		[Test]
		public void When1Param_ChooseMethodWith1Param()
		{
			var p = new ParameterBinder(typeof (MyClass), "A", new NameValueCollection {{"p1", "123"}});
			var expected = typeof(MyClass).GetMethod("A", new[] {typeof(int) });

			p.Method.Should().Be.EqualTo(expected);
		}

		[Test]
		public void When2Params_ChooseMethodWith2Params()
		{
			var p = new ParameterBinder(typeof (MyClass), "A", new NameValueCollection {{"p1", "123"}, {"p2", "452"}});
			var expected = typeof(MyClass).GetMethod("A", new[] { typeof(int), typeof(short) });

			p.Method.Should().Be.EqualTo(expected);
		}

		[Test]
		public void When1Param_InvokeMethodWithCorrectParamValue()
		{
			var p = new ParameterBinder(typeof(MyClass), "A", new NameValueCollection { { "p1", "123" } });
			var instance = new MyClass();
			p.Invoke(instance);

			instance.P1.Should().Be.EqualTo(123);
		}

		[Test]
		public void When2Params_InvokeMethodWithCorrectParamsValues()
		{
			var p = new ParameterBinder(typeof(MyClass), "A", new NameValueCollection { { "p1", "123" }, { "p2", "452" } });
			var instance = new MyClass();
			p.Invoke(instance);

			instance.P1.Should().Be.EqualTo(123);
			instance.P2.Should().Be.EqualTo(452);
		}

		public class SimpleOneLevel
		{
			public int P1 { get; set; }
			public short P2 { get; set; }
		}

		public class SimpleOneLevelAndCollection
		{
			public int P1 { get; set; }
			public IEnumerable<int> Coll { get; set; }
		}

		public class Simple2Level
		{
			public SimpleOneLevel Simple1Level { get; set; }
		}

		public class Aclass
		{
			public void MethodOneLevel(SimpleOneLevel oneLevel)
			{
				OneLevel = oneLevel;
			}

			public void MethodOneLevelColl(SimpleOneLevelAndCollection oneLevel)
			{
				OneLevelColl = oneLevel;
			}

			public void MethodTwoLevel(Simple2Level level)
			{
				TwoLevel = level;
			}

			public Simple2Level TwoLevel { get; set; }

			public SimpleOneLevel OneLevel { get; set; }

			public SimpleOneLevelAndCollection OneLevelColl { get; set; }

		}

		[Test]
		public void When1LevelParam_ChooseMethodWith1Param()
		{
			var p = new ParameterBinder(typeof(Aclass), "MethodOneLevel", new NameValueCollection { { "P1", "123" }, { "P2", "452" } });
			var instance = new Aclass();
			p.Invoke(instance);

			instance.OneLevel.P1.Should().Be.EqualTo(123);
			instance.OneLevel.P2.Should().Be.EqualTo(452);
		}

		[Test]
		public void When1LevelParamWithCollection_CanConvertTheCollection()
		{
			var p = new ParameterBinder(typeof(Aclass), "MethodOneLevelColl", new NameValueCollection { { "P1", "123" }, { "P2", "452" }, { "Coll", "8" }, { "Coll", "9" } });
			var instance = new Aclass();
			p.Invoke(instance);

			instance.OneLevelColl.P1.Should().Be.EqualTo(123);
			instance.OneLevelColl.Coll.Should().Have.SameValuesAs(8, 9);
		}

		[Test]
		public void When2LevelParam_ChooseMethodWith2Param()
		{
			var p = new ParameterBinder(typeof(Aclass), "MethodTwoLevel", new NameValueCollection { { "Simple1Level.P1", "123" }, { "Simple1Level.P2", "452" } });
			var instance = new Aclass();
			p.Invoke(instance);

			instance.TwoLevel.Simple1Level.P1.Should().Be.EqualTo(123);
			instance.TwoLevel.Simple1Level.P2.Should().Be.EqualTo(452);
		}

		public class OthersTypes
		{
			public string MStringCalled;
			public DateTime MDateCalled;
			public void MString(string something)
			{
				MStringCalled = something;
			}
			public void MDateTime(DateTime something)
			{
				MDateCalled = something;
			}
		}

		[Test]
		public void CanCallMethodWithString()
		{
			var p = new ParameterBinder(typeof(OthersTypes), "MString", new NameValueCollection { { "something", "aaa" } });
			var instance = new OthersTypes();
			p.Invoke(instance);

			instance.MStringCalled.Should().Be.EqualTo("aaa");
		}

		[Test]
		public void CanCallMethodWithDateTime()
		{
			var p = new ParameterBinder(typeof(OthersTypes), "MDateTime", new NameValueCollection { { "something", DateTime.Today.ToString() } });
			var instance = new OthersTypes();
			p.Invoke(instance);

			instance.MDateCalled.Should().Be.EqualTo(DateTime.Today);
		}
	}
}