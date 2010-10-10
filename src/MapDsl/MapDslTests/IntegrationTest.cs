using System.Data;
using MapDsl;
using MapDsl.Loquacious;
using NHibernate;
using NHibernate.ByteCode.LinFu;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using SharpTestsEx;

namespace MapDslTests
{
	public class IntegrationTest
	{
		private const string ConnectionString =
			@"Data Source=localhost\SQLEX2008;Initial Catalog=NHTEST;Integrated Security=True;Pooling=False";

		public Configuration ConfigureNHibernate()
		{
			var configure = new Configuration();
			configure.SessionFactoryName("Demo");
			configure.Proxy(p =>
				{
					p.Validation = false;
					p.ProxyFactoryFactory<ProxyFactoryFactory>();
				});
			configure.DataBaseIntegration(db =>
				{
					db.Dialect<MsSql2008Dialect>();
					db.Driver<SqlClientDriver>();
					db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
					db.IsolationLevel = IsolationLevel.ReadCommitted;
					db.ConnectionString = ConnectionString;
					db.Timeout = 10;
					db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
				});
			return configure;
		}

		public static HbmMapping GetMapping()
		{
			IMapper map = new Mapper();
			map.Assembly(typeof (Animal).Assembly);
			map.NameSpace(typeof (Animal).Namespace);

			map.Class<Animal, long>(animal => animal.Id, id => id.Generator = Generators.Native, rc =>
				{
					rc.Property(animal => animal.Description);
					rc.Property(animal => animal.BodyWeight);
					rc.ManyToOne(animal => animal.Mother);
					rc.ManyToOne(animal => animal.Father);
					rc.ManyToOne(animal => animal.Zoo);
					rc.Property(animal => animal.SerialNumber);
					rc.Set(animal => animal.Offspring, cm => cm.OrderBy(an => an.Father), rel => rel.OneToMany());
				});

			map.JoinedSubclass<Reptile>(jsc => { jsc.Property(reptile => reptile.BodyTemperature); });

			map.JoinedSubclass<Lizard>(jsc => { });

			map.JoinedSubclass<Mammal>(jsc =>
				{
					jsc.Property(mammal => mammal.Pregnant);
					jsc.Property(mammal => mammal.Birthdate);
				});

			map.JoinedSubclass<DomesticAnimal>(jsc => { jsc.ManyToOne(domesticAnimal => domesticAnimal.Owner); });

			map.JoinedSubclass<Cat>(jsc => { });

			map.JoinedSubclass<Dog>(jsc => { });

			map.JoinedSubclass<Human>(jsc =>
				{
					jsc.Component(human => human.Name, comp =>
						{
							comp.Property(name => name.First);
							comp.Property(name => name.Initial);
							comp.Property(name => name.Last);
						});
					jsc.Property(human => human.NickName);
					jsc.Property(human => human.Height);
					jsc.Property(human => human.IntValue);
					jsc.Property(human => human.FloatValue);
					jsc.Property(human => human.BigDecimalValue);
					jsc.Property(human => human.BigIntegerValue);
					jsc.Bag(human => human.Friends, cm => { }, rel => rel.ManyToMany());
					jsc.Map(human => human.Family, cm => { }, rel => rel.ManyToMany());
					jsc.Bag(human => human.Pets, cm => { cm.Inverse = true; }, rel => rel.OneToMany());
					jsc.Set(human => human.NickNames, cm =>
						{
							cm.Lazy = CollectionLazy.NoLazy;
							cm.Sort();
						});
					jsc.Map(human => human.Addresses, cm => { }, rel => rel.Component(comp =>
						{
							comp.Property(address => address.Street);
							comp.Property(address => address.City);
							comp.Property(address => address.PostalCode);
							comp.Property(address => address.Country);
							comp.ManyToOne(address => address.StateProvince);
						}));
				});

			map.Class<User, long>(sp => sp.Id, spid => spid.Generator = Generators.Foreign<User, Human>(u => u.Human), rc =>
				{
					rc.Property(user => user.UserName);
					rc.OneToOne(user => user.Human, rm => rm.Constrained());
					rc.List(user => user.Permissions, cm => { });
				});

			map.Class<Zoo, long>(zoo => zoo.Id, id => id.Generator = Generators.Native, rc =>
				{
					rc.Discriminator();
					rc.Property(zoo => zoo.Name);
					rc.Property(zoo => zoo.Classification);
					rc.Map(zoo => zoo.Mammals, cm => { }, rel => rel.OneToMany());
					rc.Map(zoo => zoo.Animals, cm => { cm.Inverse = true; }, rel => rel.OneToMany());
					rc.Component(zoo => zoo.Address, comp =>
						{
							comp.Property(address => address.Street);
							comp.Property(address => address.City);
							comp.Property(address => address.PostalCode);
							comp.Property(address => address.Country);
							comp.ManyToOne(address => address.StateProvince);
						});
				});

			map.Subclass<PettingZoo>(sc => { });

			map.Class<StateProvince, long>(sp => sp.Id, spid => spid.Generator = Generators.Native, rc =>
				{
					rc.Property(sp => sp.Name);
					rc.Property(sp => sp.IsoCode);
				});
			return map.GetCompiledMapping();
		}

		[Test]
		public void SchemaExport()
		{
			Configuration conf = ConfigureNHibernate();
			conf.AddDeserializedMapping(GetMapping(), "Animals_Domain");
			SchemaMetadataUpdater.QuoteTableAndColumns(conf);
			ActionAssert.NotThrow(() => new SchemaExport(conf).Create(false, true));
			new SchemaExport(conf).Drop(false, true);
		}

		[Test]
		public void BuildSessionFactory()
		{
			Configuration conf = ConfigureNHibernate();
			conf.AddDeserializedMapping(GetMapping(), "Animals_Domain");
			SchemaMetadataUpdater.QuoteTableAndColumns(conf);
			new SchemaExport(conf).Create(false, true);
			ActionAssert.NotThrow(() => conf.BuildSessionFactory());
			new SchemaExport(conf).Drop(false, true);
		}

		[Test]
		public void JustForFun()
		{
			Configuration conf = ConfigureNHibernate();
			conf.AddDeserializedMapping(GetMapping(), "Animals_Domain");
			SchemaMetadataUpdater.QuoteTableAndColumns(conf);
			new SchemaExport(conf).Create(false, true);
			ISessionFactory factory = conf.BuildSessionFactory();

			using (ISession s = factory.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var polliwog = new Animal {BodyWeight = 12, Description = "Polliwog"};

					var catepillar = new Animal {BodyWeight = 10, Description = "Catepillar"};

					var frog = new Animal {BodyWeight = 34, Description = "Frog"};

					polliwog.Father = frog;
					frog.AddOffspring(polliwog);

					var butterfly = new Animal {BodyWeight = 9, Description = "Butterfly"};

					catepillar.Mother = butterfly;
					butterfly.AddOffspring(catepillar);

					s.Save(frog);
					s.Save(polliwog);
					s.Save(butterfly);
					s.Save(catepillar);

					var dog = new Dog {BodyWeight = 200, Description = "dog"};
					s.Save(dog);

					var cat = new Cat {BodyWeight = 100, Description = "cat"};
					s.Save(cat);

					var zoo = new Zoo {Name = "Zoo"};
					var add = new Address {City = "MEL", Country = "AU", Street = "Main st", PostalCode = "3000"};
					zoo.Address = add;

					Zoo pettingZoo = new PettingZoo {Name = "Petting Zoo"};
					var addr = new Address {City = "Sydney", Country = "AU", Street = "High st", PostalCode = "2000"};
					pettingZoo.Address = addr;

					s.Save(zoo);
					s.Save(pettingZoo);
					tx.Commit();
				}
			}

			using (ISession s = factory.OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					s.CreateQuery("delete from Animal where mother is not null or father is not null").ExecuteUpdate();
					s.CreateQuery("delete from Animal").ExecuteUpdate();
					s.CreateQuery("delete from Zoo").ExecuteUpdate();
					tx.Commit();
				}
			}

			new SchemaExport(conf).Drop(false, true);
		}
	}
}