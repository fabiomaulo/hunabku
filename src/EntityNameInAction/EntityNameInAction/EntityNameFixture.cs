using Iesi.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Collections.Generic;
using NUnit.Framework.Syntax.CSharp;

namespace EntityNameInAction
{
	[TestFixture]
	public class PrototypeSystemFixture
	{
		protected Configuration cfg;
		protected ISessionFactoryImplementor sessions;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			cfg = new Configuration();
			cfg.AddResource("EntityNameInAction.Domain.hbm.xml", typeof(Animal).Assembly);
			cfg.Configure();
			new SchemaExport(cfg).Create(false, true);
			sessions = (ISessionFactoryImplementor) cfg.BuildSessionFactory();
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
		public void EntityNameDemo()
		{
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var rf = new Reptile {Description = "Crocodile"};
				var rm = new Reptile {Description = "Crocodile"};
				var rc1 = new Reptile {Description = "Crocodile"};
				var rc2 = new Reptile {Description = "Crocodile"};
				var rfamily = new Family<Reptile>
				              	{
				              		Father = rf, 
													Mother = rm, 
													Childs = new HashedSet<Reptile> {rc1, rc2}
				              	};
				s.Save("ReptilesFamily", rfamily);
				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
					var hf = new Human {Description = "Flinstone", Name = "Fred"};
					var hm = new Human {Description = "Flinstone", Name = "Wilma"};
					var hc1 = new Human {Description = "Flinstone", Name = "Pebbles"};
					var hfamily = new Family<Human>
					              	{
					              		Father = hf, 
														Mother = hm, 
														Childs = new HashedSet<Human> {hc1}
					              	};
					s.Save("HumanFamily", hfamily);
					tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				IList<Family<Human>> hf = s.CreateQuery("from HumanFamily").List<Family<Human>>();
				Assert.That(hf.Count, Is.EqualTo(1));
				Assert.That(hf[0].Father.Name, Is.EqualTo("Fred"));
				Assert.That(hf[0].Mother.Name, Is.EqualTo("Wilma"));
				Assert.That(hf[0].Childs.Count, Is.EqualTo(1));

				IList<Family<Reptile>> rf = s.CreateQuery("from ReptilesFamily").List<Family<Reptile>>();
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