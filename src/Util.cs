using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.ComponentModel;

namespace Tachy
{
	public class Util
	{
		static Symbol start_vector_token = Symbol.Create("#(");
		static Symbol left_paren_token = Symbol.Create("(");
		static Symbol right_paren_token = Symbol.Create(")");

		public static string arrayToString(Object[] array)
		{
			string retval = "";
			if (array != null)
			{
				foreach (object obj in array)
					if (obj != null)
						retval += obj.ToString() + " ";
					else 
						retval += " null ";
			} else
				return "{nullarray}";
			retval += " ";
			return retval;
		}

		public static object read_token(TextReader str)
		{
			if (str.Peek() == -1)
				return null;

			char c = (char) str.Read();
			if (Char.IsWhiteSpace(c)) // is_char_whitespace(c)) // .IsWhiteSpace(c))
				return read_token(str);
			else if (c.Equals(';'))
			{
				str.ReadLine();
				/*
				char curr = (char) str.Read();
				Console.WriteLine("skipping chars: ");
				while (!c.Equals('\n') && !c.Equals('\r') && (str.Peek() != -1))
				{
				
					curr = (char) str.Read();
					Console.Write(curr);
				}
				Console.WriteLine("skipping line");
				*/
				return read_token(str);
			}
			else if (c.Equals('('))
				return left_paren_token;
			else if (c.Equals(')'))
				return right_paren_token;
			else if (c.Equals('\''))																			// Quote
			{
				object curr = read(str);
				Pair newpair = new Pair(curr);
				return Pair.Cons(Symbol.Create("quote"), newpair);
			}
			else if (c.Equals('"'))
			{
				char curr = (char) str.Read();
				string strtok = "";
				while(!curr.Equals('"'))
				{
					strtok += curr;
					int curr_as_int = str.Read();
					if (curr_as_int == -1)
						throw new Exception("Error parsing string: \"" + strtok + "\"");
					else
						curr = (char) curr_as_int;
				}
				return strtok;
			} 
			else if (c.Equals('#'))																			// Boolean OR Character OR Vector
			{ 
				char curr = (char) str.Read();
				if (curr.Equals('\\')) // itsa char
				{
					return (char) str.Read();
				} else if (curr.Equals('(')) {
				  return start_vector_token;
			  } else  // itsa bool
			  {
					  if (curr.Equals('t'))
						  return true;
					  else 
						  return false; // should maybe also check for #a, etc.
			  }
			} 
			else if (Char.IsNumber(c) || ( c.Equals('-') && !Char.IsWhiteSpace((char) str.Peek()) ) )
			{
				string numstr = new string(c, 1);
			
				while (Char.IsNumber((char) str.Peek()) || ((char) str.Peek()).Equals('.'))
					numstr += (char) str.Read();
			
				if (numstr.IndexOf('.') != -1)
					return Convert.ToSingle(numstr);
				else
					return Convert.ToInt32(numstr);
			} 
			else //if (Char.IsLetter(c))
			{
				string symstr = new string(c, 1);
			
				while (!Char.IsWhiteSpace((char)str.Peek()) && !((char)str.Peek()).Equals(')') && (str.Peek() != -1))
					symstr += (char) str.Read();

				return Symbol.Create(symstr);
			}
		}

		static public object read(TextReader str)
		{
			object next_token = read_token(str);

			if (left_paren_token.Equals(next_token))
			{
// 				if (str is DocumentReader)
// 				{
// 					DocumentReader documentReader = (DocumentReader) str;
// 					//EditPoint startEditPoint = documentReader.editPoint.CreateEditPoint();
// 					int startEditPointLine = documentReader.Line;
// 					int startEditPointCol = documentReader.Col;// - 1;
// 					//startEditPoint.CharLeft(1);
					
// 					Pair result = read_list(null, str);
					
// // 					if (result != null)
// // 						result.marker = new Marker(documentReader.document, startEditPointLine, startEditPointCol, documentReader.Line, documentReader.Col + 1, "");//startEditPoint.GetText(documentReader.editPoint));

// 					return result;
// 				}
// 				else
					return read_list(null, str);
			}
			if (start_vector_token.Equals(next_token))
				return read_vector(str);
			else
				return next_token;
		}

		static public Object[] read_vector(TextReader str)
		{
			ArrayList vec = new ArrayList();

			object token = null;
			while (!right_paren_token.Equals(token))
			{
				token = read_token(str);
				if (token == null) 
					throw new Exception("Error parsing vector: #" + Util.Dump(vec).TrimEnd(new char[] {')'}).Substring(10)); 
				// a continuation would be handy here // also, the magic # 10 is to take off the ArrayList: at the beginning of the dump
				if (right_paren_token.Equals(token))
					return vec.ToArray();
				else if (left_paren_token.Equals(token))
					vec.Add(read_list(null, str));
				else if (start_vector_token.Equals(token))
					vec.Add(read_vector(str));
				else
					vec.Add(token);
			}
			return null; // thisshouldneverhappen
		}

		static public Pair read_list(Pair list_so_far, TextReader str) 
		{
			object token = read_token(str);
			
			if (token == null) // at the end of the stream, but no list. this is bad.
				throw new Exception("Error parsing list: " + Util.Dump(Pair.reverse(list_so_far)).TrimEnd(new char[] {')'})); // a continuation would be handy here
			
			if (right_paren_token.Equals(token))
				return Pair.reverse(list_so_far); // if this far we're cool
			else if (start_vector_token.Equals(token))
				return read_list(Pair.Cons(read_vector(str), list_so_far), str);
			else if (left_paren_token.Equals(token)) 
			{
// 				if (str is DocumentReader)
// 				{
// 					DocumentReader documentReader = (DocumentReader) str;
// 					//EditPoint startEditPoint = documentReader.editPoint.CreateEditPoint();
// 					//startEditPoint.CharLeft(1);
// 					int startEditPointLine = documentReader.Line;
// 					int startEditPointCol = documentReader.Col;
					
// 					Pair curList = read_list(null,str);
// // 					if (curList != null)
// // 						curList.marker = new Marker(documentReader.document, startEditPointLine, startEditPointCol, documentReader.Line, documentReader.Col + 1, "");//startEditPoint.GetText(documentReader.editPoint));
					
// 					return read_list(Pair.Cons(curList, list_so_far), str);
// 				}
// 				else
					return read_list(Pair.Cons(read_list(null,str), list_so_far),str);
			}
			else if (IsMember(list_so_far, token))
			{
				AddMemberToCar(list_so_far, token);
				return read_list(list_so_far,str);
			}
			else
				return read_list(Pair.Cons(token, list_so_far), str);
		}

		static private bool IsMember(Pair list_so_far, object token)
		{
			return ((list_so_far != null) && (list_so_far.car is Pair) && 
				(token is Symbol) && (((Symbol)token).IsMember()));
		}

		static private void AddMemberToCar(Pair pair, object token)
		{
			((Pair)pair.car).member = ((Symbol) token).val.Substring(1);
			((Pair)pair.car).hasMember = true;
		}

		static public Object[] SubArray(Object[] array, int start)
		{
			object[] retval = new object[array.Length-start];

			for (int pos=0;pos<retval.Length; pos++)
				retval[pos] = array[pos+start];
			return retval;
		}		

		static public Type[] GetTypes(object[] objs)
		{
			int i = 0;
			Type[] retval = new Type[objs.Length];
			foreach (Object obj in objs)
			{
				if (obj == null)
					retval[i] =Type.GetType("System.Object");
				else
					retval[i] = obj.GetType();
				//Console.WriteLine("gtypes [" + retval[i] + "]");
				i++;
			}
			return retval;
		}

		// could move to scheme, but likely just take out of prims & make part of Expressions.App
		public static object Call_Method(Object[] args, bool static_call)
		{
			// Console.WriteLine("call: " + Util.arrayToString(args));
			// Assembly a = System.Reflection.Assembly.Load("System");
			Object[] objs = null;
			Type[] types = new Type[0];
			if (args[2] != null) // see def of call & call-static in init.ss -- method args passed in as rest, if none then it's ()
			{
				objs = (args[2] as Pair).ToArray();
				types = GetTypes(objs);
			}
			Type type;
			if (static_call == true) // args.car is Symbol) // Static Method Invokation
				type = Util.GetType(args[0].ToString());
			else if (args[0] == null)
				type = Type.GetType("System.Object");
			else
				type = args[0].GetType();
			//Console.WriteLine("type = " + type.ToString());
			// Console.WriteLine(" = " + args[1]);
			
			object retval = null;
			try 
			{
				// Console.WriteLine("Method " + args[0].ToString() + "." + args[1].ToString());
				MethodInfo method = type.GetMethod(args[1].ToString(), types);
				if (method != null)
					retval = method.Invoke(args[0], objs);
				else 
				{
					throw new Exception("call: Could not find method " + args[1]);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				Console.WriteLine("Method " + args[0].ToString() + "." + args[1].ToString() + " failed with arguments ");
				for (int i=0; i<objs.Length; i++)
				{
					if (objs[i] == null)
						Console.WriteLine("null " + "[type: " + types[i] + "]");
					else
						Console.WriteLine(objs[i] + "[type: " + types[i] + "]");
				}
			}
			return retval;
		}

		public static Type GetType(String tname, Pair prefixes)
		{
			Type type = GetType(tname);
			if (type != null)
				return type;
			foreach (string prefix in prefixes) 
			{
				type = GetType(prefix + "." + tname);
				if (type != null)
					return type;
			}
			return null;
		}

		public static Type GetType(String tname)
		{
			Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly assembly in assemblies) 
			{
				Type type = assembly.GetType(tname);
				// type = System.Reflection.Assembly.Load(aname).GetType(tname);
				if (type != null)
					return type;
			}
			return null;
		}

		static public String Dump(object exp)
		{
			if (exp is string)
			{
				return '"' + (string) exp + '"';
			} 
			else if (exp is bool) 
			{
				if ((bool) exp == true)
					return "#t";
				else
					return "#f";
			} 
			else if (exp is char) 
			{
				return "#\\" + (char) exp;
			} 
			else if (exp is Pair) 
			{
				String retVal = "( ";

				Pair curr = exp as Pair;
				while (curr != null)
				{
					retVal += Dump(curr.car) + " ";
					curr = curr.cdr ;
				}

				return retVal + ")";
			} 
			else if (exp == null) 
			{
				return " () ";
			} 
			else if (exp is ArrayList) 
			{
				String retVal = "ArrayList:( ";
				foreach (Object obj in (exp as ArrayList)) 
					retVal += Dump(obj) + " ";
				return retVal + ")";
			} 
			else 
			{ 
				return exp.ToString();
			}
		}
	}
	public class Symbol
	{ 
		static int lastGen = 1000;
		static Hashtable syms = new Hashtable();
		internal String val;

		internal Symbol(String val) { this.val = val; }

		static public Symbol GenSym()
		{
			String symName = "g" + lastGen.ToString();
			lastGen++;
			return Create(symName);
		}

		static public Symbol Create(String symName)
		{
			Symbol retVal;
			if (syms[symName] == null)
			{
				retVal = new Symbol(symName);
				syms.Add(symName, retVal);
			}
			else
				retVal = (Symbol) syms[symName];

			return retVal;
		}

		public override Boolean Equals(object obj) 
		{
			if (obj is Symbol)
				if (this.val == (obj as Symbol).val)
					return true;
				else
					return false;
			else 
				return false;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		internal bool IsMember()
		{
			if (val.Length < 2)
				return false;

			if ((val[0] != '.') || (val[1] == ' '))
				return false;

			return true;
		}

		private bool HasMember()
		{
			return ((!IsMember()) && (val.IndexOf('.') != -1));
		}

		override public System.String ToString() { return val;  } 
	}

	// Closure
	public class Closure
	{
		public Symbol[] ids;
		public Expression body;
		public Env env;
		public bool all_in_one;
		public Closure(Symbol[] ids, Expression body, bool all_in_one, Env env)
		{
			this.ids = ids; this.body = body; this.all_in_one = all_in_one; this.env = env; 
		}

		public override string ToString()
		{
			return "<Closure: ids=[" + Util.arrayToString(ids) + " ] all_in_one=" + all_in_one + " Env=" + env.ToString() + " Body=" + body.ToString() + ">";
		}

		public object Eval(Object[] args)
		{	
			// Debug.WriteLine("Closure.Eval: ids:" + ids + " args: " + args);
			Env eval_Env;
			if (ids != null)
			{
				if (all_in_one) // this might bear optimization at some point (esp since +,-,*, etc. use this)
				{
					Pair pairargs = Pair.FromArrayAt(args,0);
					Object[] newargs = new Object[1];
					newargs[0] = pairargs;
					eval_Env = env.Extend(ids, newargs);
				} 
				else
					eval_Env = env.Extend(ids, args); //tailcall

			}
			else 
				eval_Env = env; // tailcall

// 			DebugInfo.SetEnvironment(eval_Env);
			return body.Eval(eval_Env); 
		}
	}

	// Pair
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class Pair : ICollection
	{  
		private object car_; private Pair cdr_; 
// 		public Marker marker;
		public bool hasMember = false;
		public string member = "";
		
		public Pair (object car) { this.car= car; this.cdr = null; }
		public Pair (object car, Pair cdr) { this.car= car; this.cdr = cdr; }


		[TypeConverter(typeof(ExpandableObjectConverter))]
		public object car
		{
			get {return car_;}
			set {car_ = value;}
		}

		public Pair cdr
		{
			get {return cdr_;}
			set {cdr_ = value;}
		}
		
		public static Pair Cons(object obj, Pair p)
		{
			Pair newPair = new Pair(obj);
			newPair.cdr = p;
			return newPair;
		}

		public static Pair Cons(object obj, object p)
		{
			if (p != null)
				throw new Exception("dotted pairs not supported");
			Pair newPair = new Pair(obj);
			newPair.cdr = null;
			return newPair;
		}

		public void Append(object obj)
		{
			Pair curr = this;
			while (curr.cdr != null)  { curr = curr.cdr; }
			curr.cdr = new Pair(obj);
		}

		public int Count 
		{
			get 
			{
				int len = 0;
				Object sigh; // only to get rid of compiler warning
				foreach (object obj in this)
				{
					sigh = obj;
					len++;
				}
				return len;
			}
		}

		public Pair Clone()
		{
			return (Pair) base.MemberwiseClone();
		}

		// TODO: new cons for each?
		static public Pair append(Pair ls1, Pair ls2)
		{
			if (ls1 == null)
				return ls2;
			else
			{
				Pair p = ls1.Clone();
				p.cdr = append(ls1.cdr, ls2);
				return p;

/*				Pair p = Pair.Cons(ls1.car, append(ls1.cdr, ls2));
				p.Info = ls1.Info;
				return p;*/
			}
		}

		// TODO: new cons for each?
		static public Pair reverse(Pair pair)
		{
			if (pair == null)
				return null;
			else
			{
				//TODO: change recursive copy to single copy
				Pair p = pair.Clone();
				p.cdr = null;
				return append(reverse(pair.cdr), p);

/*
				Pair p = new Pair(pair.car);
				p.Info = pair.Info;
				return append(reverse(pair.cdr), p);*/
			}
		}

		public void CopyTo(Array array, int index)
		{
			if (array.Length < (this.Count + index))
				throw new ArgumentException();

			foreach (object obj in this)
				array.SetValue(obj, index++);
		}

		public IEnumerator GetEnumerator()
		{
			return new PairEnumerator(this);
		}

		class PairEnumerator : IEnumerator
		{
			Pair pair, current;

			public PairEnumerator(Pair pair) { this.pair = pair; this.current = null;}

			public object Current 
			{ 
				get { return current.car; } 
			}
			public bool MoveNext() 
			{
				if (current == null)
				{
					current = pair;
					return true;
				} 
				else if (current.cdr != null)
				{	
					current = current.cdr;
					return true;
				} 
				else 
				{
					return false;
				}
			}
			public  void Reset() { current = pair; }
		}

		[Browsable(false)]
		public bool IsSynchronized {get { return false; } }

		[Browsable(false)]
		public object SyncRoot {get { return this; } }

		public static Pair FromArrayAt(Object[] array, int pos)
		{
			Pair retval = null;

			for (int i=array.Length-1;i>=pos;i--)
				retval = Pair.Cons(array[i], retval);
			return retval;		
		}

		public object[] ToArray()
		{
			object[] retval = 	new Object[Count];
			CopyTo(retval, 0);
			return retval;
		}
		
		override public System.String ToString() 
		{ 
			System.String retVal = "( ";

			foreach (object obj in this)
			{
				retVal += obj + " ";
			}
			retVal += " )";
			// broke dotted pairs
			return retVal;
		} 

	}
}