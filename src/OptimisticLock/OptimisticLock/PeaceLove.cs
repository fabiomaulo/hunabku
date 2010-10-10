using NHibernate;
using NUnit.Framework;

namespace OptimisticLock
{
	[TestFixture]
	public class PeaceLove : AbstractExample
	{
		protected override string GetMappingResourcePath()
		{
			return "OptimisticLock.PeaceLove.hmb.xml";
		}

		[Test]
		public void SaveUpdate()
		{
			using (ISession session = sessionFactory.OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					var sample = new Sample { Description = "sample" };
					session.Save(sample);
					sample.Description = "modified sample";
					transaction.Commit();
				}
			}
		}
	}
}