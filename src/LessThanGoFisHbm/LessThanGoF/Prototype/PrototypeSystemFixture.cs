using System.Collections;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace LessThanGoF.Prototype
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
			cfg.Configure();
			cfg.AddResource("LessThanGoF.Prototype.ProductLine.hbm.xml", typeof (PrototypeSystemFixture).Assembly);
			new SchemaExport(cfg).Create(false, true);

			cfg.SetProperty("default_entity_mode", EntityModeHelper.ToString(EntityMode.Map));

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
		public void DynamicClasses()
		{
			IDictionary cars;
			IList models;
			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					cars = new Hashtable();
					cars["Description"] = "Cars";

					IDictionary ferrari = new Hashtable();
					ferrari["ProductLine"] = cars;
					ferrari["Name"] = "Dino";
					ferrari["Description"] = "Ferrari Dino.";

					IDictionary lamborghini = new Hashtable();
					lamborghini["ProductLine"] = cars;
					lamborghini["Name"] = "Countach";
					lamborghini["Description"] = "Lamborghini Countach";

					models = new List<IDictionary> {ferrari, lamborghini};

					cars["Models"] = models;

					s.Save("ProductLine", cars);
					t.Commit();
				}
			}

			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					cars = (IDictionary) s.CreateQuery("from ProductLine pl order by pl.Description").UniqueResult();
					models = (IList) cars["Models"];

					Assert.That(models.Count == 2);

					s.Clear();

					IList list = s.CreateQuery("from Model m").List();
					var model = (IDictionary) list[0];

					Assert.That(((IList) ((IDictionary) model["ProductLine"])["Models"]).Contains(model));

					s.Clear();
					t.Commit();
				}
			}

			using (ISession s = sessions.OpenSession())
			{
				using (ITransaction t = s.BeginTransaction())
				{
					cars = (IDictionary) s.CreateQuery("from ProductLine pl order by pl.Description").UniqueResult();
					s.Delete(cars);
					t.Commit();
				}
			}
		}

		[Test]
		public void Anonimous()
		{
			using (ISession s = sessions.OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.SaveDynamic("Model", new
				                       	{
				                       		Name = "Locus", 
																	Description = "Audi Locus", 
																	ProductLine = new
																	              	{
																	              		Description = "concept car"
																	              	}
				                       	});
				t.Commit();
			}
	
			using (ISession s = sessions.OpenSession())
			{
				s.CreateQuery("from ProductLine pl order by pl.Description").List()
					.Count.Should().Be.Equals(1);
			}
		}
	}
}