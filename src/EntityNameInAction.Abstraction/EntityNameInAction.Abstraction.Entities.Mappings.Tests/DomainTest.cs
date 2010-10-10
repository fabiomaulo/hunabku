using EntityNameInAction.Abstraction.Adapters.DependencyInjection;
using EntityNameInAction.Abstraction.Entities.Naturalness;
using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace EntityNameInAction.Abstraction.Entities.Mappings.Tests
{
	[TestFixture]
	public class DomainTest
	{
		protected Configuration cfg;
		protected ISessionFactoryImplementor sessions;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			cfg = new Configuration();
			cfg.AddAssembly("EntityNameInAction.Abstraction.Entities.Mappings");
			cfg.Configure();
			new SchemaExport(cfg).Create(false, true);
			sessions = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
			DI.StackResolver(new NhAdapters.NhEntityClassResolver(sessions));
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			new SchemaExport(cfg).Drop(false, true);
			sessions.Close();
			sessions = null;
			cfg = null;
		}

		[Test]
		public void DomainAbstraction()
		{
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var rf = DI.Resolver.Resolve<IReptile>();
				rf.Description = "Crocodile";

				var rm = DI.Resolver.Resolve<IReptile>();
				rm.Description = "Crocodile";

				var rc1 = DI.Resolver.Resolve<IReptile>();
				rc1.Description = "Crocodile";

				var rc2 = DI.Resolver.Resolve<IReptile>();
				rc2.Description = "Crocodile";

				var rfamily = DI.Resolver.Resolve<IFamily<IReptile>>();
				rfamily.Father = rf;
				rfamily.Mother = rm;
				rfamily.Childs = new HashedSet<IReptile> { rc1, rc2 };

				s.Save(rfamily);
				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var hf = DI.Resolver.Resolve<IHuman>();
				hf.Description = "Flinstone";
				hf.Name = "Fred";

				var hm = DI.Resolver.Resolve<IHuman>();
				hm.Description = "Flinstone";
				hm.Name = "Wilma";

				var hc1 = DI.Resolver.Resolve<IHuman>();
				hc1.Description = "Flinstone";
				hc1.Name = "Pebbles";

				var hfamily = DI.Resolver.Resolve<IFamily<IHuman>>();
				hfamily.Father = hf;
				hfamily.Mother = hm;
				hfamily.Childs = new HashedSet<IHuman> { hc1 };

				s.Save(hfamily);
				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var hf = s.CreateQuery("from HumanFamily").List<IFamily<IHuman>>();
				
				Assert.That(hf.Count, Is.EqualTo(1));
				Assert.That(hf[0].Father.Name, Is.EqualTo("Fred"));
				Assert.That(hf[0].Mother.Name, Is.EqualTo("Wilma"));
				Assert.That(hf[0].Childs.Count, Is.EqualTo(1));

				var rf = s.CreateQuery("from ReptilesFamily").List<IFamily<IReptile>>();

				Assert.That(rf.Count, Is.EqualTo(1));
				Assert.That(rf[0].Childs.Count, Is.EqualTo(2));

				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from HumanFamily");
				s.Delete("from ReptilesFamily");
				tx.Commit();
			}
		}
	}

}