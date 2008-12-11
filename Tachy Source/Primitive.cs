using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections;
using Microsoft.VisualBasic.CompilerServices;

namespace Tachy
{
	public interface IPrim 
	{
		object Call(Object[] args);
	}
	
	class Primitives
	{
		static Hashtable prims = null;

/*
  		static public void InitializeGraphics()
		{
			// Set our presentation parameters
			PresentParameters presentParams = new PresentParameters();

			presentParams.Windowed = true;
			presentParams.SwapEffect = SwapEffect.Discard;

			// Create our device
			device = new Device(0, DeviceType.Hardware, this, CreateFlags.SoftwareVertexProcessing, presentParams);
		}
*/

		static public IPrim getPrim(string name)
		{
			if (prims == null)
				setupPrimitives();
			return (IPrim) prims[name];
		}

		static public void addPrim(string name, IPrim prim)
		{
			if (prims == null)
				throw new Exception("setupPrims must be called first");
			prims[name] = prim;
		}

		static public void setupPrimitives() 
		{
			prims = new Hashtable(40);
			prims["addobj-prim"] = new AddObjPrim();
			prims["subobj-prim"] = new SubObjPrim();
			prims["mulobj-prim"] = new MulObjPrim();
			prims["divobj-prim"] = new DivObjPrim();
			prims["strcompare-prim"] = new StrcompPrim();
			prims["numcompare-prim"] = new NumcompPrim();
			prims["hash-ref-prim"] = new HashrefPrim();
			prims["hash-set!-prim"] = new HashsetPrim();
			prims["cons-prim"] = new ConsPrim();
			prims["set-car-prim"] = new SetCarPrim();
			prims["set-cdr-prim"] = new SetCdrPrim();
			prims["car-prim"] = new CarPrim();
			prims["cdr-prim"] = new CdrPrim();
			prims["eq?-prim"] = new EqPrim();
			prims["eqv?-prim"] = new EqvPrim();
			prims["is-pair-prim"] = new IsPairPrim();

			prims["typeof-prim"] = new TypeofPrim();

			prims["vector-prim"] = new VectorPrim();
			prims["vector-length-prim"] = new VecLenPrim();
			prims["vector-ref-prim"] = new VecRefPrim();
			prims["vector-set!-prim"] = new VecSetPrim();
			prims["string->symbol-prim"] = new StrSymPrim();
			prims["tostring-prim"] = new TostringPrim();

  			prims["get-type-prim" ] = new GetTypePrim();
			prims["new-prim"] = new NewPrim();
			prims["call-prim"] = new CallPrim();
			prims["call-static-prim"] = new CallStaticPrim();
			prims["get-property-prim"] = new GetPropPrim();
			prims["set-property-prim"] = new SetPropPrim();
			prims["get-field-prim"] = new GetFieldPrim();
			prims["set-field-prim"] = new SetFieldPrim();
			prims["copy-debug-prim"] = new CopyDebugPrim();
		}

		public class StrSymPrim : IPrim	
		{
			public object Call(Object[] args)	
			{
				return Tachy.Symbol.Create((string) args[0]); 
			}
		}
		
		public class TostringPrim : IPrim	
		{ 
			public object Call(Object[] args)	
			{ 
				return args[0].ToString(); 
			}
		}
		
		public class VectorPrim : IPrim	
		{
			public object Call(Object[] args)	
			{
				if (args[1] != null)
					return System.Array.CreateInstance(args[1] as Type, (int) args[0]);
				else
					return new Object[(int) args[0]];
			}
		}

		public class VecLenPrim : IPrim	
		{
			public object Call(Object[] args)	
			{ 
				return (args[0] as System.Array).Length; 
			}
		}
		
		public class VecRefPrim : IPrim	
		{
			public object Call(Object[] args)	
			{
				return (args[0] as System.Array).GetValue((int) args[1]); 
			}
		}
		
		public class VecSetPrim : IPrim	
		{
			public object Call(Object[] args)	
			{
				(args[0] as System.Array).SetValue(args[2], (int) args[1]); return null; 
			} 
		}
		
		public class AddObjPrim : IPrim	
		{ 
			public object Call(Object[] args)	
			{
				return ObjectType.AddObj(args[0], args[1]); 
			}
		}
		
		public class SubObjPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{
				return ObjectType.SubObj(args[0], args[1]); 
			}
		}
		
		public class MulObjPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{
				return ObjectType.MulObj(args[0], args[1]); 
			}
		}
		
		public class DivObjPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{
				return ObjectType.DivObj(args[0], args[1]); 
			}
		}
		
		public class StrcompPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{
				return String.Compare((string) args[0], (string) args[1], (bool) args[2]);
			}
		}

		public class NumcompPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{
				return ObjectType.ObjTst(args[0], args[1], false);
			}
		}
		
		public class HashrefPrim : IPrim	
		{ 
			public object Call(Object[] args)	
			{
				return (args[0] as Hashtable)[args[1]]; 
			} 
		}
		
		public class HashsetPrim : IPrim	
		{ 
			public object Call(Object[] args)	
			{
				return (args[0] as Hashtable)[args[1]] = args[2]; 
			}
		}
		
		public class ConsPrim : IPrim	
		{ 
			public object Call(Object[] args)	{ return Pair.Cons(args[0], args[1] as Pair); }	
		}

		public class SetCarPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{
				(args[0] as Pair).car = args[1];
				return null;
			}
		}
		
		public class SetCdrPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{
				(args[0] as Pair).car = args[1];
				return null;
			}
		}

		public class CarPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{
				return (args[0] as Pair).car; 
			}
		}
		
		public class CdrPrim : IPrim
		{ 
			public object Call(Object[] args)	
			{ 
				return (args[0] as Pair).cdr; 
			} 
		}
		
		public class EqPrim : IPrim 	
		{ 
			public object Call(Object[] args)	
			{ 
				if ((args[0] is Boolean) && (args[1] is Boolean))
					return (bool) args[0] == (bool) args[1];
				else
					return System.Object.ReferenceEquals(args[0],args[1]);
			} 
		}

		public class IsPairPrim : IPrim 	
		{ 
			public object Call(Object[] args)	
			{ 
				if (args[0] is Pair) 
						return true;
				else 
						return false;
			} 
		}
		
		public class EqvPrim : IPrim 
		{ 
			public object Call(Object[] args)	
			{
				return args[0].Equals(args[1]); 
			}
		}
		public class TypeofPrim : IPrim 	
		{ 
			public object Call(Object[] args)	
			{
				return args[0].GetType(); 
			}
		}

		
		public class GetTypePrim : IPrim 
		{
			public object Call(Object[] args) // sym typename, pair usings
			{
				if (args[1] == null)
					return Tachy.Util.GetType(args[0].ToString());
				else
					return Tachy.Util.GetType(args[0].ToString(), args[1] as Pair);
			}
		}

		public class NewPrim : IPrim 
		{
			public object Call(Object[] args)	
			{
				Type type = Util.GetType(args[0].ToString(), args[1] as Pair);
				try 
				{
					if (args[2] == null)
						return Activator.CreateInstance(type);
					else
						return Activator.CreateInstance(type, (args[2] as Pair).ToArray(), null);
				} 
				catch (Exception e) 
				{
					Console.WriteLine("Newprim failed with type " + type);
					throw e;
				}
			}
		}

		public class CallPrim : IPrim
		{
			public object Call(Object[] args)
			{
				return Util.Call_Method(args,false);
			}
		}

		public class  CallStaticPrim : IPrim
		{
			public object Call(Object[] args)
			{
				return Util.Call_Method(args,true);
			}
		}

		public class  GetPropPrim : IPrim
		{
			public object Call(Object[] args)
			{
				Object obj = args[0];
				String field = (args[1] as Symbol).ToString();
				System.Object[] rest = null;
				if (args[2] != null)
					rest = (args[2] as Pair).ToArray();
				if (obj is Symbol)
					return Util.GetType(obj.ToString()).GetProperty(field).GetValue(null,rest);
				else
					return obj.GetType().GetProperty(field).GetValue(obj, rest);
			}
		}
		public class  SetPropPrim : IPrim
		{
			public object Call(Object[] args)
			{
				Object obj = args[0];
				String field = args[1].ToString();
				Object val = args[2];
				System.Object[] rest = null;
				if (args[3] != null)
					rest = (args[3] as Pair).ToArray();
				if (obj is Symbol)
					Util.GetType(obj.ToString()).GetProperty(field).SetValue(null,val,rest);
				else 
				{
					PropertyInfo prop = obj.GetType().GetProperty(field);
					if (prop == null)
						throw new Exception("Could not find property " + field + " in object of type " + obj.GetType().ToString());
					prop.SetValue(obj, val, rest);
				}
				return true;
			}
		}
		public class  GetFieldPrim : IPrim
		{
			public object Call(Object[] args)
			{
				return args[0].GetType().GetField((args[1] as Symbol).ToString()).GetValue(args[0]);
			}
		}
		public class  SetFieldPrim : IPrim
		{
			public object Call(Object[] args)
			{
				args[0].GetType().GetField((args[1] as Symbol).ToString()).SetValue(args[0],args[2]);
				return true;
			}
		}
		public class CopyDebugPrim : IPrim
		{
			public object Call(Object[] args)
			{
				if ((args[0] is Pair) && (args[1] is Pair))
				{
// 					((Pair) args[1]).marker = ((Pair) args[0]).marker;
					((Pair) args[1]).hasMember = ((Pair) args[0]).hasMember;
					((Pair) args[1]).member = ((Pair) args[0]).member;
				}
				else
					throw new Exception("arguments not pairs");

				return args[1];
			}
		}

	}
}