using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Collection;

namespace DuckTyping
{
	public class PersistentBagGenericWrapper<T> : IList<T>, IDynamicList where T : class
	{
		private readonly PersistentBag persistentBag;

		public PersistentBagGenericWrapper(PersistentBag persistentBag)
		{
			this.persistentBag = persistentBag;
		}

		#region Implementation of IEnumerable

		public IEnumerator<T> GetEnumerator()
		{
			foreach (var element in persistentBag)
			{
				yield return element.AsDynamic<T>();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return persistentBag.GetEnumerator();
		}

		#endregion

		#region Implementation of ICollection<T>

		public void Add(T item)
		{
			var de = item as IDynamicEntity;
			if (de != null)
			{
				persistentBag.Add(de.DynamicHandler.Data);
			}
			else
			{
				persistentBag.Add(item);
			}
		}

		public void Clear()
		{
			persistentBag.Clear();
		}

		public bool Contains(T item)
		{
			return persistentBag.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public bool Remove(T item)
		{
			var orgCount = persistentBag.Count;
			persistentBag.Remove(item);
			return orgCount != Count;
		}

		public int Count
		{
			get { return persistentBag.Count; }
		}

		public bool IsReadOnly
		{
			get { return persistentBag.IsReadOnly; }
		}

		#endregion

		#region Implementation of IList<T>

		public int IndexOf(T item)
		{
			return persistentBag.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			persistentBag.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			persistentBag.RemoveAt(index);
		}

		public T this[int index]
		{
			get { return persistentBag[index].AsDynamic<T>(); }
			set { persistentBag[index] = value; }
		}

		#endregion
		public override bool Equals(object obj)
		{
			return persistentBag.Equals(obj);
		}

		public override int GetHashCode()
		{
			return persistentBag.GetHashCode();
		}
	}
}