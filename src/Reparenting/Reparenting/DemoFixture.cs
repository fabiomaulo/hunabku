using System.Collections.Generic;
using System.Linq;
using log4net.Config;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Engine;
using NUnit.Framework;

namespace Reparenting
{
	[TestFixture]
	public class DemoFixture
	{
		static DemoFixture()
		{
			XmlConfigurator.Configure();
		}

		private ISessionFactoryImplementor sessions;

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

		private void FillDb()
		{
			var rep = new List<Node>();
			for (int i = 0; i < 2; i++)
			{
				var gp = new Node {Description = "N" + i.ToString("00")};
				rep.Add(gp);
				for (int j = 0; j < 5; j++)
				{
					var c = new Node { Description = gp.Description + "-" + j.ToString("00") };
					gp.AddChild(c);
					for (int k = 0; k < 4; k++)
					{
						var cc = new Node { Description = c.Description + "-" + k.ToString("00") };
						c.AddChild(cc);
					}
				}
			}
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				foreach (var node in rep)
				{
					s.Save(node);
				}
				tx.Commit();
			}
		}

		private void ClearDb()
		{
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.CreateQuery("delete from Node").ExecuteUpdate();
				tx.Commit();
			}			
		}

		private void ReparentingAllChildren(Node original, Node newParent)
		{
			var toReparent = new List<Node>(original.Children);
			for (int i = 0; i < toReparent.Count; i++)
			{
				var node = toReparent[i];
				newParent.AddChild(node);
				node.Description = newParent + "(" + node.Description + ")";
			}
		}

		[Test]
		public void ReparentingAllChildrenAttached()
		{
			FillDb();
			var tree = new List<Node>();

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.CreateQuery("from Node n where n.Parent is null").List(tree);
				tree.Count.Should().Be.EqualTo(2);

				ReparentingAllChildren(tree[1], tree[0]);
				tx.Commit();
			}
			tree.Clear();
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.CreateQuery("from Node n where n.Parent is null").List(tree);

				tree.Should().Satisfy(a => a.Any(x => x.Children.Count() == 10));

				tx.Commit();
			}

			ClearDb();
		}

		[Test]
		public void ReparentingSomeChildrenDeletingOthersAttached()
		{
			FillDb();
			var tree = new List<Node>();

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.CreateQuery("from Node n where n.Parent is null").List(tree);

				var children = new List<Node>(tree[0].Children);

				// Move first 2 children to the new parent
				tree[1].AddChild(children[0]);
				tree[1].AddChild(children[1]);

				// remove others children
				children = new List<Node>(tree[0].Children);
				foreach (var child in children)
				{
				  tree[0].RemoveChild(child);
				}

				tx.Commit();
			}

			tree.Clear();
			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.CreateQuery("from Node n where n.Parent is null").List(tree);
				
				tree.Count.Should("there are only two roots").Be.EqualTo(2);
				tree.Should("one should have no children")
					.Satisfy(a => a.Any(x => x.Children.Count() == 0));
				tree.Should("the other should have seven children")
					.Satisfy(a => a.Any(x => x.Children.Count() == 7));

				tx.Commit();
			}

			ClearDb();
		}

		[Test]
		public void TryingDisaster()
		{
			FillDb();

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var tree = s.CreateQuery("from Node n where n.Parent is null").List<Node>();


				// Move first 2 children of root-0 to root-1
				var children = new List<Node>(tree[0].Children);

				tree[1].AddChild(children[0]);
				tree[1].AddChild(children[1]);

				// remove others children from root-1
				children = new List<Node>(tree[0].Children);
				foreach (var child in children)
				{
					tree[0].RemoveChild(child);
				}

				// move all nodes from actual root-1-child-0 to root-0
				ReparentingAllChildren(tree[1].Children.First(), tree[0]);

				tx.Commit();
			}

			using (ISession s = sessions.OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var tree = s.CreateQuery("from Node n where n.Parent is null").List<Node>();

				tree.Count.Should("have only two roots").Be.EqualTo(2);

				tree.Should("one should have four children")
					.Satisfy(a => a.Any(x => x.Children.Count() == 4));

				// get the root-1
				var root1 = tree.FirstOrDefault(x => x.Children.Count() == 7);

				root1.Should("have 7 children").Not.Be.Null();

				// one of children should be empty (children was moved to root-0)
				root1.Children.Should()
					.Satisfy(a => a.Any(x => x.Children.Count() == 0));

				// others children, should be intact
				root1.Children.Count(c=> c.Children.Count() == 4).Should().Be.EqualTo(6);

				tx.Commit();
			}
			ClearDb();
		}
	}
}