using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Envers;
using NHibernate.Linq;

namespace EnverIntroduction
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var nhinit = new NHibernateInitializer();
			nhinit.Initialize();
			nhinit.InitializeAudit();
			nhinit.CreateSchema();
			ISessionFactory sf = nhinit.SessionFactory;

			CreateModelHistory(sf);
			PrintActualState(sf);
			Console.ReadLine();
			Console.Clear();

			//PrintAuditEntitiesInfo<Customer>(sf);
			ShowHistory(sf);
			Console.WriteLine();
			Console.WriteLine("Work done!");
			Console.ReadLine();
			sf.Close();
			nhinit.DropSchema();
		}

		private static void PrintActualState(ISessionFactory sf)
		{
			Console.WriteLine("Actual state");
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList objects = s.CreateCriteria(typeof (Entity)).List();
					foreach (object o in objects)
					{
						Console.WriteLine("Type->{0} ={1}", o.GetType().Name, o);
					}
					tx.Commit();
				}
			}
			Console.WriteLine("-----END ACTUAL STATE-----");
		}

		private static void CreateModelHistory(ISessionFactory sf)
		{
			CreateMovies(sf);
			CreateBooks(sf);
			CreateCustomers(sf);

			AddPrefferedOfFabioMaulo(sf);
			ChangeMoviesPrice(sf, 30.00m);
			ChangeBooksPrice(sf, 40.00m);
			ChangeMoviesPrice(sf, 35.00m);
			RemoveBook(sf);
		}

		private static void RemoveBook(ISessionFactory sf)
		{
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IFutureValue<Book> book = (from b in s.Query<Book>() where b.ISBN == "184951304X" select b).ToFutureValue();
					IFutureValue<Customer> customer = (from c in s.Query<Customer>() where c.Name.Contains("Fabio") select c).ToFutureValue();
					customer.Value.PreferredProducts.Remove(book.Value);
					s.Delete(book.Value);
					tx.Commit();
				}
			}
		}

		private static void ShowHistory(ISessionFactory sf)
		{
			PrintAuditedEntities<Product>(sf);
			PrintAuditedEntities<Customer>(sf);
			Console.ReadLine();
			Console.Clear();

			PrintCustomerAtSpecificRevision(sf, 5);
			PrintCustomerAtSpecificRevision(sf, 6);
			PrintCustomerAtSpecificRevision(sf, 7);
			PrintCustomerAtSpecificRevision(sf, 8);
			PrintCustomerAtSpecificRevision(sf, 9);
		}

		private static void PrintCustomerAtSpecificRevision(ISessionFactory sf, int revision)
		{
			Console.WriteLine("Customer at revision {0}", revision);
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IAuditReader auditer = AuditReaderFactory.Get(s);
					IList resultList = auditer.CreateQuery().ForEntitiesAtRevision(typeof (Customer), revision).GetResultList();
					foreach (object entity in resultList)
					{
						Console.WriteLine(entity);
					}
					tx.Commit();
				}
			}
			Console.WriteLine("---------------");
		}

		private static void ChangeBooksPrice(ISessionFactory sf, decimal newPrice)
		{
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<Book> products = s.QueryOver<Book>().List();
					Array.ForEach(products.ToArray(), x => x.UnitPrice = newPrice);
					tx.Commit();
				}
			}
		}

		private static void PrintAuditedEntities<T>(ISessionFactory sf) where T : Entity
		{
			Console.WriteLine("Chenges on " + typeof (T).Name);
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IAuditReader auditer = AuditReaderFactory.Get(s);
					IList resultList = auditer.CreateQuery().ForRevisionsOfEntity(typeof (T), true, true).GetResultList();
					foreach (object entity in resultList)
					{
						Console.WriteLine(entity);
					}
					tx.Commit();
				}
			}
			Console.WriteLine("---------------");
		}

		private static void PrintAuditEntitiesInfo<T>(ISessionFactory sf) where T: Entity
		{
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IAuditReader auditer = AuditReaderFactory.Get(s);
					IList resultList = auditer.CreateQuery().ForRevisionsOfEntity(typeof(T), false, true).GetResultList();
					foreach (EntityRevisionInfo<object> revInfo in ToRevisionInfo<object>(resultList))
					{
						Console.WriteLine("{0} {1} Rev={2} Timestamp={3:o}",revInfo.Operation,revInfo.EntityRevision,revInfo.RevisionId,revInfo.Timestamp);
						Console.WriteLine("###########################");
					}
					tx.Commit();
				}
			}
			Console.WriteLine("---------------");
		}

		private static IEnumerable<EntityRevisionInfo<T>> ToRevisionInfo<T>(IList results) where T: class
		{
			foreach (var item in results)
			{
				var result = item as object[];
				var defaultRevisionEntity = (result[1] as DefaultRevisionEntity);
				yield return new EntityRevisionInfo<T>(result[0] as T, (RevisionType)result[2], defaultRevisionEntity.Id, defaultRevisionEntity.RevisionDate);
			}
		}

		private static void ChangeMoviesPrice(ISessionFactory sf, decimal unitPrice)
		{
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IList<Movie> products = s.QueryOver<Movie>().List();
					Array.ForEach(products.ToArray(), x => x.UnitPrice = unitPrice);
					tx.Commit();
				}
			}
		}

		private static void AddPrefferedOfFabioMaulo(ISessionFactory sf)
		{
			string customerName = "Fabio Maulo";
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					IEnumerable<Product> products = s.QueryOver<Product>().Where(c => c.Name.IsLike("300") || c.Name.IsLike("NHibernate%")).Future();
					Customer customer = s.QueryOver<Customer>().Where(c => c.Name == customerName).SingleOrDefault();
					Array.ForEach(products.ToArray(), x => customer.PreferredProducts.Add(x));
					tx.Commit();
				}
			}
		}

		private static void CreateCustomers(ISessionFactory sf)
		{
			var customerFabio = new Customer {Name = "Fabio Maulo"};
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					s.Persist(customerFabio);
					tx.Commit();
				}
			}
		}

		private static void CreateBooks(ISessionFactory sf)
		{
			var bookNh3Cook = new Book {Name = "NHibernate 3.0 Cookbook", Author = "Jason Dentler", ISBN = "184951304X"};
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					s.Persist(bookNh3Cook);
					tx.Commit();
				}
			}
		}

		private static void CreateMovies(ISessionFactory sf)
		{
			var actorHeadey = new Actor {Name = "Lena", Surname = "Headey"};
			var actorButler = new Actor {Name = "Gerad", Surname = "Butler"};
			var movie300 = new Movie {Name = "300", Director = "Zack Snyder", Description = "King Leonidas and a force of 300 men fight the Persians at Thermopylae in 480 B.C"};
			var movieRocknRolla = new Movie {Name = "RocknRolla", Director = "Guy Ritchie", Description = "In London, a real-estate scam puts millions of pounds up for grabs..."};
			var actorsRole300 = new[]
			                    {
			                    	new ActorRole {Actor = actorHeadey, Role = "King Leonidas"},
			                    	new ActorRole {Actor = actorButler, Role = "Queen Gorgo"},
			                    };
			Array.ForEach(actorsRole300, ar => movie300.Actors.Add(ar));
			movieRocknRolla.Actors.Add(new ActorRole {Actor = actorHeadey, Role = "One Two"});

			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					s.Persist(actorButler);
					s.Persist(actorHeadey);
					tx.Commit();
				}
			}
			using (ISession s = sf.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					s.Persist(movie300);
					s.Persist(movieRocknRolla);
					tx.Commit();
				}
			}
		}
	}

	internal class EntityRevisionInfo<T>
	{
		public EntityRevisionInfo(T entityRevision, RevisionType operation, long revisionId, DateTime timestamp)
		{
			EntityRevision = entityRevision;
			Operation = operation;
			RevisionId = revisionId;
			Timestamp = timestamp;
		}

		public T EntityRevision { get; private set; }
		public RevisionType Operation { get; private set; }
		public long RevisionId { get; private set; }
		public DateTime Timestamp { get; private set; }
	}
}