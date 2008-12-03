using System;
using System.IO;
using System.Collections;
using System.Reflection;

namespace Tachy
{
	// Interpreter
	// Programs 
	public abstract class Program 
	{ 
		public Env initEnv;
		public Hashtable macros = new Hashtable();
		public abstract object Eval(Expression exp);
	}

	public class A_Program : Program 
	{
		public A_Program() 
		{
			this.initEnv = new Extended_Env(new Symbol[] { Symbol.Create("_macros") } , new Object[] { macros }, Env.The_Empty_Env);

		} // new Pair(new Symbol("x")), new Pair(3), Env.The_Empty_Env);}
		// this.initEnv = Env.The_Empty_Env; // new Pair(new Symbol("x")), new Pair(3), Env.The_Empty_Env);}
			
		public object LoadEmbededInitTachy()
		{
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			Stream initTachyStream = executingAssembly.GetManifestResourceStream("Tachy.InitTachy.ss");
			StreamReader initTachyStreamReader = new StreamReader(initTachyStream); 
			return Eval(initTachyStreamReader);
		}

		public override object Eval(Expression exp)		{	return exp.Eval(initEnv); }
		public object Eval(string str)
		{
			return Eval(new StringReader(str));
		}

		public object Eval(string str, Env env)
		{
			Env curEnv = initEnv;
			initEnv = env;

			object result = Eval(str);

			initEnv = curEnv;
			return result;
		}

		public object Eval(TextReader str, Env env)
		{
			Env curEnv = initEnv;
			initEnv = env;

			object result = Eval(str);

			initEnv = curEnv;
			return result;
		}

		public object Eval(TextReader str)
		{
			//Console.WriteLine("starting eval");
			Object evaledObj = null;
			Object parsedObj = Util.read(str);

			
			if (parsedObj is Pair)
			{
				if (macros.Count > 0)
				{
// 					bool curRunHidden = DebugInfo.RunHidden;
// 					DebugInfo.RunHidden = true; //don't debug the macro expension code

					Closure macroExpand = initEnv.Apply(Symbol.Create("macro-expand")) as Closure;
					parsedObj = macroExpand.Eval(new Object[] { parsedObj });

// 					DebugInfo.RunHidden = curRunHidden;
				} 
				
				Pair p = parsedObj as Pair;
				switch (p.car.ToString()) 
				{
					case "define":
						Expression expr = null;
						Symbol def = null;
						if (p.cdr.car is Pair) // (define (fn x y) body)
						{
							Pair def_and_args = p.cdr.car as Pair;
							Pair body = p.cdr.cdr;
							Pair syms = def_and_args.cdr;
							def = Symbol.Create(def_and_args.car.ToString());
							expr = Expression.Parse(Pair.Cons(Symbol.Create("lambda"), Pair.Cons(syms,body)));
							// Debug.WriteLine(def + "-->" + expr);
						}
						else // (define fn (lambda (x y) body))
						{
							def = Symbol.Create(p.cdr.car.ToString());
							expr = Expression.Parse(p.cdr.cdr.car);
						}
						//initEnv = new Extended_Env(new Pair(def), new Pair(null), initEnv);
						//Object value = Eval(expr);
						//Console.WriteLine("defining: " + def);
// 						expr.Mark(p);
						initEnv.Bind(def, Eval(expr));					
						break;
					case "macro":
						Symbol name = p.cdr.car as Symbol;
						// Console.WriteLine("macrodef" + name + " " + Expression.Parse(p.cdr.cdr.car).Eval(initEnv).ToString());
						Closure transformer = (Closure) Expression.Parse(p.cdr.cdr.car).Eval(initEnv);
						this.macros[name] = transformer;
						// Console.WriteLine("macro: " + name);
						break;
					default:
						// need to call macro-expand in here. but how?
						evaledObj = Util.Dump(Eval(Expression.Parse(parsedObj)));
						break;
				}
			} 
			else 
				evaledObj = Util.Dump(Eval(Expression.Parse(parsedObj)));

			if (str.Peek() == -1)
				return evaledObj;
			else
				return Eval(str);
		}
	}
}
