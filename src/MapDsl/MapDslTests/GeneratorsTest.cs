using MapDsl.Loquacious;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class GeneratorsTest
	{
		[Test]
		public void CreateNativeGenerator()
		{
			var generator = (Generators.AbstractGenerator) Generators.Native;
			var compiledGenerator = generator.GetCompiledGenerator();
			compiledGenerator.Should().Not.Be.Null();
			compiledGenerator.@class.Should().Be.EqualTo("native");
		}

		private class MyClass
		{
			public int IntProp { get; set; }
		}

		[Test]
		public void CreateForeignGenerator()
		{
			var generator = (Generators.AbstractGenerator)Generators.Foreign<MyClass, int>(mc=>mc.IntProp);
			var compiledGenerator = generator.GetCompiledGenerator();
			compiledGenerator.Should().Not.Be.Null();
			compiledGenerator.@class.Should().Be.EqualTo("foreign");
			compiledGenerator.param.Should().Have.Count.EqualTo(1);
			compiledGenerator.param[0].Satisfy(par => par.name == "property" && par.Text[0] == "IntProp");
		}
	}
}