using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Reparenting
{
	public class Node : IEquatable<Node>
	{
		private int? requestedHashCode;

		private ISet<Node> children;
		private Node parent;

		public Node()
		{
			children = new HashedSet<Node>();
		}
		public virtual int Id { get; protected set; }
		public virtual string Description { get; set; }
		public virtual Node Parent
		{
			get { return parent; }
			set
			{
				if (ReferenceEquals(null, parent) && !ReferenceEquals(null, value))
				{
					value.AddChild(this);
				}
				if (!ReferenceEquals(null, parent) && ReferenceEquals(null, value))
				{
					parent.RemoveChild(this);
				}
				if (!ReferenceEquals(null, parent) && !ReferenceEquals(null, value))
				{
					parent.RemoveChild(this);
					value.AddChild(this);
				}
			}
		}

		public virtual IEnumerable<Node> Children
		{
			get { return children; }
		}

		public virtual void AddChild(Node child)
		{
			if(child.parent != null)
			{
				child.parent.RemoveChild(child);
			}
			child.parent = this;
			children.Add(child);
		}

		public virtual void RemoveChild(Node child)
		{
			if(children.Remove(child))
			{
				child.parent = null;
			}
		}

		#region Implementation of IEquatable<Node>

		public virtual bool Equals(Node other)
		{
			if (null == other || !GetType().IsInstanceOfType(other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			bool otherIsTransient = Equals(other.Id, 0);
			bool thisIsTransient = Equals(Id, 0);
			if (otherIsTransient && thisIsTransient)
			{
				return ReferenceEquals(other, this);
			}

			return other.Id.Equals(Id);
		}

		#endregion

		public override bool Equals(object obj)
		{
			var that = obj as Node;
			return Equals(that);
		}

		public override int GetHashCode()
		{
			if (!requestedHashCode.HasValue)
			{
				requestedHashCode = Equals(Id, 0) ? base.GetHashCode() : Id.GetHashCode();
			}
			return requestedHashCode.Value;
		}

		public override string ToString()
		{
			return Description;
		}
	}
}