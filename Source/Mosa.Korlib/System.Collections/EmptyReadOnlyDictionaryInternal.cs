﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.Contracts;

namespace System.Collections
{
	///    This is a simple implementation of IDictionary that is empty and readonly.
	[Serializable]
	internal sealed class EmptyReadOnlyDictionaryInternal : IDictionary
	{
		// Note that this class must be agile with respect to AppDomains.  See its usage in
		// System.Exception to understand why this is the case.

		public EmptyReadOnlyDictionaryInternal()
		{
		}

		// IEnumerable members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new NodeEnumerator();
		}

		// ICollection members

		public void CopyTo(Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));

			if (array.Rank != 1)
				throw new ArgumentException("Arg_RankMultiDimNotSupported");

			if (index < 0)
				throw new ArgumentOutOfRangeException(nameof(index), "ArgumentOutOfRange_NeedNonNegNum");

			if (array.Length - index < this.Count)
				throw new ArgumentException("ArgumentOutOfRange_Index", nameof(index));
			Contract.EndContractBlock();

			// the actual copy is a NOP
		}

		public int Count
		{
			get
			{
				return 0;
			}
		}

		public Object SyncRoot
		{
			get
			{
				return this;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// IDictionary members

		public Object this[Object key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException(nameof(key), "ArgumentNull_Key");
				}
				Contract.EndContractBlock();
				return null;
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException(nameof(key), "ArgumentNull_Key");
				}

				if (!key.GetType().IsSerializable)
					throw new ArgumentException("Argument_NotSerializable", nameof(key));

				if ((value != null) && (!value.GetType().IsSerializable))
					throw new ArgumentException("Argument_NotSerializable", nameof(value));
				Contract.EndContractBlock();

				throw new InvalidOperationException("InvalidOperation_ReadOnly");
			}
		}

		public ICollection Keys
		{
			get
			{
				return EmptyArray<Object>.Value;
			}
		}

		public ICollection Values
		{
			get
			{
				return EmptyArray<Object>.Value;
			}
		}

		public bool Contains(Object key)
		{
			return false;
		}

		public void Add(Object key, Object value)
		{
			if (key == null)
			{
				throw new ArgumentNullException(nameof(key), "ArgumentNull_Key");
			}

			if (!key.GetType().IsSerializable)
				throw new ArgumentException("Argument_NotSerializable", nameof(key));

			if ((value != null) && (!value.GetType().IsSerializable))
				throw new ArgumentException("Argument_NotSerializable", nameof(value));
			Contract.EndContractBlock();

			throw new InvalidOperationException("InvalidOperation_ReadOnly");
		}

		public void Clear()
		{
			throw new InvalidOperationException("InvalidOperation_ReadOnly");
		}

		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		public IDictionaryEnumerator GetEnumerator()
		{
			return new NodeEnumerator();
		}

		public void Remove(Object key)
		{
			throw new InvalidOperationException("InvalidOperation_ReadOnly");
		}

		private sealed class NodeEnumerator : IDictionaryEnumerator
		{
			public NodeEnumerator()
			{
			}

			// IEnumerator members

			public bool MoveNext()
			{
				return false;
			}

			public Object Current
			{
				get
				{
					throw new InvalidOperationException("InvalidOperation_EnumOpCantHappen");
				}
			}

			public void Reset()
			{
			}

			// IDictionaryEnumerator members

			public Object Key
			{
				get
				{
					throw new InvalidOperationException("InvalidOperation_EnumOpCantHappen");
				}
			}

			public Object Value
			{
				get
				{
					throw new InvalidOperationException("InvalidOperation_EnumOpCantHappen");
				}
			}

			public DictionaryEntry Entry
			{
				get
				{
					throw new InvalidOperationException("InvalidOperation_EnumOpCantHappen");
				}
			}
		}
	}
}
