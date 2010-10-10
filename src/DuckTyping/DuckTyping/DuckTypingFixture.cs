using System.Collections.Generic;
using log4net.Config;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NUnit.Framework;
using System.Collections;

namespace DuckTyping
{
	[TestFixture]
	public class DuckTypingFixture
	{
		static DuckTypingFixture()
		{
			XmlConfigurator.Configure();
		}
		protected ISessionFactoryImplementor sessions;
		public interface IProductLine
		{
			int Id { get; set; }
			string Description { get; set; }
			IList<IModel> Models { get; set; }
		}
		public interface IModel
		{
			int Id { get; set; }
			string Name { get; set; }
			string Description { get; set; }
			IProductLine ProductLine { get; set; }
		}

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			var cfg = new Configuration().Configure();
			sessions = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
		}

		[TestFixtureTearDown]
		public void TestFixtureTearDown()
		{
			sessions.Close();
			sessions = null;
		}

		[Test]
		public void CrudWithAnonimous()
		{
			object savedId;
			IProductLine productLine;
			using (ISession s = sessions.OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				var line = new
				           	{
				           		Description = "High quality cars", 
											Models = new ArrayList(), 
											Pizza = "calda"
				           	};
				var ferrari = new
				              	{
				              		ProductLine = line, 
													Name = "Dino", 
													Description = "Ferrari Dino", 
													Fuel = "Gasoline"
				              	};
				var lamborghini = new
				                  	{
				                  		ProductLine = line, 
															Name = "Countach", 
															Description = "Lamborghini Countach",
															Origin = "Italy"
				                  	};
				line.Models.Add(ferrari);
				line.Models.Add(lamborghini);

				savedId = s.SaveDynamic("ProductLine", line);
				t.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				// Reload the saved dynamic entity
				var entity = s.Get("ProductLine", savedId);
				// Transform it to a know type
				productLine = entity.AsDynamic<IProductLine>();

				// Modify through know type
				productLine.Description = "Quality cars";
				productLine.Models.Add(
					(new
					 	{
							ProductLine = entity,
							Name = "Locus",
							Description = "Audi Locus"
					 	}).AsDynamic<IModel>());

				t.Commit(); // Persist modification
			}

			// Test over modifications
			using (ISession s = sessions.OpenSession())
			{
				var entity = s.Get("ProductLine", savedId);
				productLine = entity.AsDynamic<IProductLine>();
				productLine.Description.Should().Be.EqualTo("Quality cars");
				productLine.Models.Count.Should().Be.EqualTo(3);
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				// Delete the detached dynamic entity
				s.DeleteDynamic("ProductLine", productLine);
				t.Commit();
			}

			using (ISession s = sessions.OpenSession())
			{
				// check entity deletation with cascade
				s.Get("ProductLine", savedId).Should().Be.Null();
				s.CreateQuery("from Model").List().Count.Should().Be.EqualTo(0);
			}
		}
	}
}