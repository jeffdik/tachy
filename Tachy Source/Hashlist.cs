using System;
using System.Collections;

namespace Tachy
{
	/// <summary>
	/// Summary description for Hashlist.
	/// </summary>
	public class Hashlist : IEnumerable, IEnumerator
	{
      //
      //Fields
      //

      private int listCapacity;
      private int listCount;
      private object[] _listValue;
		private object[] _listKey;
		private Hashtable hashtable;
      private int current;

      //
      //Constructors
      //

      public Hashlist(int listCapacity)
      {
         this.listCount = 0;
         this.listCapacity = listCapacity;
      }

		public Hashlist() : this(10){}

      //
      //Properties
      //

      public object this[object key]
      {
         get 
         {
            if (hashtable != null)
               return hashtable[key];
            else
            {
               int index = IndexOf(key);
               if (index < 0)
                  return null;
               else
                  return _listValue[index];
            }
         }
			set
			{
				if (hashtable != null)
					hashtable[key] = value;
				else
				{
               int index = IndexOf(key);
					if (index < 0)
						AddNew(key, value);
					else
						_listValue[index] = value;
				}
			}
      }

      public int Count
      {
         get {return (hashtable == null ? listCount : hashtable.Count);}
      }

      public int ListCapacity
      {
         get {return listCapacity;}
      }

		internal object[] listValue
		{
			get
			{
				if (_listValue == null)
				{
					_listValue = new object[listCapacity];
					_listKey = new object[listCapacity];
				}

				return _listValue;
			}
		}

		internal object[] listKey
		{
			get
			{
				if (_listKey == null)
				{
					_listValue = new object[listCapacity];
					_listKey = new object[listCapacity];
				}

				return _listKey;
			}
		}

		public virtual ICollection Keys 
		{
			get
			{
				if (hashtable != null)
					return hashtable.Keys;
				else
				{
					ArrayList keys = new ArrayList(listCount);
					for (int i = 0; i < listCount; i++)
						keys.Add(_listKey[i]);
					
					return keys;
				}
			}
		}

		public virtual ICollection Values 
		{
			get
			{
				if (hashtable != null)
					return hashtable.Values;
				else
				{
					ArrayList values = new ArrayList(listCount);
					for (int i = 0; i < listCount; i++)
						values.Add(_listValue[i]);
					
					return values;
				}
			}
		}


      //
      //Methods
      //

		public bool Add(object key, object value)
		{
			if (ContainsKey(key))
				return false;

			return AddNew(key, value);
		}

		public bool AddNew(object key, object value)
		{
			if (((listCount > 0) && (listCount < listCapacity)) || ((listCount == 0) && (listCount < listKey.Length)))
         //if (listCount < list.Length)
         {
            _listKey[listCount] = key;
            _listValue[listCount] = value;
				listCount++;
			}
         else
         {
            if (hashtable == null)
            {
               hashtable = new Hashtable();
               for (int i = 0; i < listCapacity; i++)
               {
                  hashtable.Add(_listKey[i], _listValue[i]);
               }
            }

            hashtable.Add(key, value);
         }

         return true;
      }

      private int IndexOf(object key)
      {
         for (int i = 0; i < listCount; i++)
         {
            if (_listKey[i] == key)
               return i;
         }

         return -1;
      }

      public bool ContainsKey(object key)
      {
         if (hashtable != null)
            return hashtable.ContainsKey(key);
         else
         {
            for (int i = 0; i < listCount; i++)
            {
               if (_listKey[i] == key)
                  return true;
            }
         }

         return false;
      }

      public void Clear()
      {
         listCount = 0;
         hashtable = null;
      }

      #region IEnumerable Members

      public IEnumerator GetEnumerator()
      {
         if (hashtable != null)
            return hashtable.GetEnumerator();
         else
            return this;
      }

      #endregion

      #region IEnumerator Members

      public void Reset()
      {
         current = -1;
      }

      public object Current
      {
         get
         {
            if (current < 0)
               return null;
            else
               return new DictionaryEntry(_listKey[current], _listValue[current]);
         }
      }

      public bool MoveNext()
      {
         if (current < listCount - 1)
         {
            current++;
            return true;
         }
         else
            return false;
      }

      #endregion
   }
}
