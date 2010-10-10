using Iesi.Collections.Generic;
using LessThanFew.Domain;
using NHibernate;
using NUnit.Framework;
using NUnit.Framework.Syntax.CSharp;

namespace LessThanFew.Tests
{
	[TestFixture]
	public class PersistenceFixture : FunctionalTest
	{
		[Test]
		public void PersonCrud()
		{
			object savedId;
			var person = entityFactory.NewEntity<IPerson>();
			person.Name = "Katsumoto";

			using (var session = sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				savedId = session.Save(person);
				tx.Commit();
			}

			using (var session = sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				person = session.Get<IPerson>(savedId);
				Assert.That(person, Is.Not.Null);
				Assert.That(person.Name, Is.EqualTo("Katsumoto"));
				session.Delete(person);
				tx.Commit();
			}

			using (var session = sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				person = session.Get<IPerson>(savedId);
				Assert.That(person, Is.Null);
				tx.Commit();
			}
		}

		[Test]
		public void FullCream()
		{
			object savedCustomerId;

			// Test saving these proxies
			using (var session = sessions.OpenSession())
			{
				session.BeginTransaction();
				
				var company = entityFactory.NewEntity<ICompany>();
				company.Name = "acme";
				session.Save(company);
				var customer = entityFactory.NewEntity<ICustomer>();
				customer.Name = "Katsumoto";
				customer.Company = company;
				var address = entityFactory.NewEntity<IAddress>();
				address.Street = "somewhere over the rainbow";
				address.City = "lawerence, kansas";
				address.PostalCode = "toto";
				customer.Address = address;
				customer.Family = new HashedSet<IPerson>();
				var son = entityFactory.NewEntity<IPerson>();
				son.Name = "son";
				customer.Family.Add(son);
				var wife = entityFactory.NewEntity<IPerson>();
				wife.Name = "wife";
				customer.Family.Add(wife);
				savedCustomerId = session.Save(customer);

				session.Transaction.Commit();
				Assert.IsNotNull(company.Id, "company id not assigned");
				Assert.IsNotNull(customer.Id, "customer id not assigned");
				Assert.IsNotNull(address.Id, "address id not assigned");
				Assert.IsNotNull(son.Id, "son:Person id not assigned");
				Assert.IsNotNull(wife.Id, "wife:Person id not assigned");
			}

			// Test loading these proxies, along with flush processing
			ICustomer willDetached;
			using (var session = sessions.OpenSession())
			{
				session.BeginTransaction();

				willDetached = session.Load<ICustomer>(savedCustomerId);
				Assert.IsFalse(NHibernateUtil.IsInitialized(willDetached), "should-be-proxy was initialized");

				willDetached.Name = "other";
				session.Flush();
				Assert.IsFalse(NHibernateUtil.IsInitialized(willDetached.Company), "should-be-proxy was initialized");

				session.Refresh(willDetached);
				Assert.AreEqual("other", willDetached.Name, "name not updated");
				Assert.AreEqual("acme", willDetached.Company.Name, "company association not correct");

				session.Transaction.Commit();
			}

			// Test detached entity re-attachment with these dyna-proxies
			willDetached.Name = "Katsumoto"; //<= is detached
			using (var session = sessions.OpenSession())
			{
				session.BeginTransaction();

				session.Update(willDetached);
				session.Flush();
				session.Refresh(willDetached);
				Assert.AreEqual("Katsumoto", willDetached.Name, "name not updated");

				session.Transaction.Commit();
			}


			// Test querying
			using (var session = sessions.OpenSession())
			{
				session.BeginTransaction();

				int count = session.CreateQuery("from ICustomer").List().Count;
				Assert.AreEqual(1, count, "querying dynamic entity");
				session.Clear();
				count = session.CreateQuery("from IPerson").List().Count;
				Assert.AreEqual(3, count, "querying dynamic entity");

				session.Transaction.Commit();
			} 

			// test deleteing
			using (var session = sessions.OpenSession())
			using (var tx = session.BeginTransaction())
			{
				session.Delete("from ICompany");
				session.Delete("from ICustomer");
				tx.Commit();
			}
		}
	}
}