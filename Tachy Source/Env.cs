using System;
using System.Collections;
//using TopSupport.Core.Common;

namespace Tachy 
{
	public abstract class Env 
	{
		static public Empty_Env The_Empty_Env = new Empty_Env();
		public Extended_Env Extend(Symbol[] syms, Object[] vals)
		{
			return new Extended_Env(syms, vals, this);
		}
		abstract public object Apply(Symbol id);
		abstract public object Bind(Symbol id, Object val);
		//	abstract public object Bind(Symbol[] ids, Object[] vals);
	}

	public class Empty_Env : Env 
	{
		override public object Apply(Symbol id)
		{
			throw new Exception("Unbound variable " + id);
		}
		override public System.String ToString()
		{
			return "<env: empty>";
		}
		override public object Bind(Symbol id, Object val)
		{
			throw new Exception("Unbound variable " + id);
		}

		/*	override public object Bind(Symbol[] ids, Object[] vals)
			{
				throw new Exception("Unbound variable " + ids);
			}
		*/
	}

	public class Extended_Env : Env
	{
		// Pair syms = null;
		// Pair vals = null;
		//internal Hashlist bindings = new Hashlist(); //Hashlist
		internal Hashtable bindings = new Hashtable(); //Hashlist
		internal Env env = null;

		override public System.String ToString()
		{
			return "<env: " + bindings.Keys.ToString() + " " + bindings.Values.ToString() + " " + env.ToString() + " >";
		}
		public Extended_Env(Symbol[] inSyms, Object[] inVals, Env inEnv)
		{
			this.env = inEnv;
		
			for (int pos=0; pos < inSyms.Length; pos++)
			{
				Symbol currSym = (Symbol) inSyms[pos];
			
				if (!currSym.ToString().Equals("."))
					//this.bindings.AddNew(currSym, inVals[pos]); //Hashlist
					this.bindings[currSym] = inVals[pos];
				else 
				{
					// multiple values passed in (R5RS 4.1.4)
					currSym = (Symbol) inSyms[pos+1];
					//this.bindings.AddNew(currSym, Pair.FromArrayAt(inVals,pos)); //Hashlist
					this.bindings[currSym] = Pair.FromArrayAt(inVals,pos);
					break;
				}
			}
		}

		// this and above should be merged somehow (may be obvious actually)
		/*	public override object Bind(Symbol[] inSyms, Object[] inVals)
			{
				for (int pos=0; pos < inSyms.Length; pos++)
				{
					Symbol currSym = (Symbol) inSyms[pos];
			
					if (!currSym.ToString().Equals("."))
						this.bindings[currSym] = inVals[pos];
					else 
					{
						// multiple values passed in (R5RS 4.1.4)
						currSym = (Symbol) inSyms[pos+1];
						this.bindings[currSym] = Pair.FromArrayAt(inVals,pos);
						break;
					}
				}
				return null;
			}
		*/
		override public object Bind(Symbol id, Object val)
		{
			if (this.bindings.ContainsKey(id) || env == The_Empty_Env)
			{
				bindings[id] = val;
				return id;
			} 
			else
			{
				return env.Bind(id,val);
			}
		}

		override public object Apply(Symbol id)
		{
         if (this.bindings.ContainsKey(id))
            return bindings[id];
         else 
            return env.Apply(id);
		}
	}
}