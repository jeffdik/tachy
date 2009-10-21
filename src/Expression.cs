using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;

namespace Tachy
{
	// Expressions 
	public abstract class Expression 
	{
// 		internal Marker marker;

		abstract public object Eval(Env env);
		
// 		internal void Mark(object obj)
// 		{
// 			if (obj is Pair)
// 				marker = ((Pair) obj).marker;
// 		}

		static public Object[] Eval_Rands(Expression[] rands, Env env)
		{
			if (rands == null)
				return null;

			Object[] dest = new Object[rands.Length];

			for (int i=0; i<rands.Length; i++)
				dest[i] = rands[i].Eval(env);
		
			return dest;
		}

		static public Expression Parse(object a)
		{
			if (a is Symbol) 
			{
				if (((Symbol) a).val.IndexOf(".") == -1)
					return new Var(a as Symbol);
				else
				{
					string aString = ((Symbol) a).val;
					int posLastDot = aString.LastIndexOf(".");
					
					Expression[] rands = new Expression[2];
					rands[0] = Expression.Parse(Symbol.Create(aString.Substring(0, posLastDot)));
					rands[1] = new Lit(Symbol.Create(aString.Substring(posLastDot + 1, aString.Length - posLastDot - 1)));

					return new App(Expression.Parse(Symbol.Create("get-property")), rands);
					
				}
			}
			if (a is Pair) 
			{
				Pair pair = a as Pair;
				object car = pair.car;
				switch (car.ToString())
				{
					case "begin":
						Expression[] exps = new Expression[pair.cdr.Count];
						int pos = 0;
						foreach (object obj in pair.cdr)
						{
							exps[pos] = Parse(obj);
// 							exps[pos].Mark(obj);
							pos++;
						}
						Begin beginExps = new Begin(exps);
// 						beginExps.Mark(pair);
						return beginExps;
					case "if": 
						Pair curr = pair.cdr;
						Expression test_exp = Parse(curr.car);
// 						test_exp.Mark(curr.car);
						curr = curr.cdr;
						Expression true_exp = Parse(curr.car);
// 						true_exp.Mark(curr.car);
						curr = curr.cdr;
						Expression false_exp = Parse(curr.car);
// 						false_exp.Mark(curr.car);
						return new If(test_exp, true_exp, false_exp);
					case "quote":
						return new Lit(pair.cdr.car);
					case "set!": 
					{
						Symbol var = pair.cdr.car as Symbol;
						if (var == null)
							throw new Exception("set! error -> variable must be a symbol: " + Util.Dump(pair));

						Expression exp = Parse(pair.cdr.cdr.car) as Expression;
// 						exp.Mark(pair.cdr.cdr.car);
						if (var.val.IndexOf('.') == -1)
						{
							Assignment assignment = new Assignment(var, exp);
// 							assignment.Mark(pair);
							return assignment;
						}
						else
						{
							string aString = var.val;
							int posLastDot = aString.LastIndexOf(".");
							Expression[] rands = new Expression[3];
							rands[0] = Expression.Parse(Symbol.Create(aString.Substring(0, posLastDot)));
							rands[1] = new Lit(Symbol.Create(aString.Substring(posLastDot + 1, aString.Length - posLastDot - 1)));
							rands[2] = exp;
							
							App app = new App(Expression.Parse(Symbol.Create("set-property")), rands);
// 							app.Mark(pair);
							return app;
						}
					}
					case "lambda":
						// Debug.WriteLine("sparsing lambda " + pair.ToString());
						curr = pair.cdr  as Pair;
						Symbol[] ids = null;
						bool all_in_one = false;
						if (curr.car != null)
						{
							if (curr.car is Pair)
							{
								Object[] ids_as_obj = (curr.car as Pair).ToArray();
								ids = new Symbol[ids_as_obj.Length];
								for (int i=0; i<ids_as_obj.Length; i++)
								{
									ids[i] = ids_as_obj[i] as Symbol;
									if (ids[i] == null)
										throw new Exception("lambda error -> params must be symbols: " + Util.Dump(pair));
								}
							} 
							else 
							{
								all_in_one = true;
								ids = new Symbol[1];
								ids[0] = curr.car as Symbol;
								if (ids[0] == null)
									throw new Exception("lambda error -> params must be symbols: " + Util.Dump(pair));
							}
						}
						curr = curr.cdr  as Pair;
						// insert implied begin if neccessary
						Expression body;
						if (curr.cdr == null)
						{
							body = Parse(curr.car);
// 							if (curr.car is Pair)
// 								body.Mark(curr.car);
// 							else
// 								body.Mark(curr);
						}
						else
						{
							Expression[] begin = new Expression[curr.Count];
							pos = 0;
							foreach (object obj in curr)
							{
								begin[pos] = Parse(obj);
// 								begin[pos].Mark(obj);
								pos++;
							}
							body = new Begin(begin);
						}
						return new Proc(ids, body, all_in_one);
					default:  // app
						if (pair.hasMember)
						{
							Expression[] rands = new Expression[2];
							if (pair.member.IndexOf('.') != -1)
							{
								string currentMember = pair.member;
								int posLastDot = currentMember.LastIndexOf(".");
					
								pair.member = currentMember.Substring(0, posLastDot);
								rands[0] = Expression.Parse(pair);
								pair.member = currentMember;

								rands[1] = new Lit(Symbol.Create(currentMember.Substring(posLastDot + 1, currentMember.Length - posLastDot - 1)));
							}
							else
							{
								pair.hasMember = false;
								rands[0] = Expression.Parse(pair);
								pair.hasMember = true;

								rands[1] = new Lit(Symbol.Create(pair.member));
							}
							return new App(Expression.Parse(Symbol.Create("get-property")), rands);
						}
						else
						{
							Expression[] rands = null;
							if (pair.cdr != null)
							{
								rands = new Expression[pair.cdr.Count];
								pos = 0;
								foreach (object obj in pair.cdr)
								{
									rands[pos] = Expression.Parse(obj);
// 									rands[pos].Mark(obj);
									pos++;
								}
							}
						
                                                        Console.WriteLine(pair.car.ToString());
							IPrim prim = Primitives.getPrim(pair.car.ToString());
							if (prim != null)
							{
								Primapp primapp = new Primapp(prim, rands);
// 								primapp.Mark(pair);
								return primapp;
							}
                                                        else if (Regex.IsMatch(pair.car.ToString(), @"^([\w.]+)\.$"))
                                                        {
                                                            Match m = Regex.Match(pair.car.ToString(), @"^([\w.]+)\.$");
                                                            string class_name = m.Result("$1");
                                                            Console.WriteLine("ClassName: {0}", class_name);
                                                            if (rands == null)
                                                                rands = new Expression[0];
                                                            Expression[] new_rands = new Expression[rands.Length+1];
                                                            new_rands[0] = Expression.Parse(Pair.Cons(Symbol.Create("quote"), new Pair(class_name)));
                                                            Console.WriteLine(Util.Dump(new_rands[0]));
                                                            rands.CopyTo(new_rands, 1);
                                                            App app = new App(Expression.Parse(Symbol.Create("new")), new_rands);
                                                            Console.WriteLine(Util.Dump(app));
                                                            return app;
                                                                                            
                                                        }
							else if (Regex.IsMatch(pair.car.ToString(), @"^\.(\w+)\$$"))
							{
							    Match m = Regex.Match(pair.car.ToString(), @"^\.(\w+)\$$");
							    string property_name = m.Result("$1");
							    Console.WriteLine(Util.Dump(pair));
							    if (rands.Length == 2) {
							      Expression[] new_rands = new Expression[rands.Length+1];
							      new_rands[0] = rands[0];
							      new_rands[1] = Expression.Parse(Pair.Cons(Symbol.Create("quote"), new Pair(property_name)));
							      new_rands[2] = rands[1];
							      App app = new App(Expression.Parse(Symbol.Create("set-property")), new_rands);
							      Console.WriteLine(Util.Dump(app));
							      return app;
							    } else {
							      return null;
							    }
							}
							else if (Regex.IsMatch(pair.car.ToString(), @"^(.*)\.([^.]+)$"))
							{
							    Match m = Regex.Match(pair.car.ToString(), @"^(.*)\.([^.]+)$");
							    string class_name = m.Result("$1");
							    string static_method = m.Result("$2");
							    Expression[] new_rands = new Expression[rands.Length+2];
							    new_rands[0] = Expression.Parse(Pair.Cons(Symbol.Create("quote"), new Pair(class_name)));
							    new_rands[1] = Expression.Parse(Pair.Cons(Symbol.Create("quote"), new Pair(static_method)));
							    rands.CopyTo(new_rands, 2);
							    App app = new App(Expression.Parse(Symbol.Create("call-static")), new_rands);
							    Console.WriteLine(Util.Dump(app));
							    return app;
							}
							else
							{
								App app = new App(Expression.Parse(pair.car), rands);
// 								app.Mark(pair);
								return app;
							}
						}
				}
			} 
			else 
				return new Lit(a);
		}
	}

	public class Lit : Expression 
	{
		public object datum;
		public Lit(object datum) { this.datum = datum; }
		public override object Eval(Env env)
		{
			// Debug.WriteLine("Eval Lit: " + datum);
			return datum;
		}

		public override String ToString() { return "<Lit: " + datum + ">"; }
	}

	public class Var : Expression 
	{
		public Symbol id;
		public Var(Symbol id) { this.id = id; }
		public override object Eval(Env env)
		{
			// Debug.WriteLine("Eval->Var: " + id);
			return env.Apply(id);
		}

		override public System.String ToString() { return "<var: " + id + "> "; } 
	}

	public class Proc : Expression 
	{
		public Symbol[] ids; // if null, gets all args as a list
		public Expression body;
		public bool all_in_one;
		public Proc(Symbol[] ids, Expression body, bool all_in_one) { this.ids = ids; this.body = body; this.all_in_one = all_in_one; }
		override public System.String ToString() { return "<proc: ids=[" + Util.arrayToString(ids) + "]  body=" + body + "> "; } 
		public override object Eval(Env env)
		{
// 			DebugInfo.EvalExpression(this);
			// Debug.WriteLine("Eval->Proc");
			return new Closure(ids, body, all_in_one, env);
		}

	}

	public class Primapp : Expression 
	{
		public IPrim prim;
		public Expression[] rands;
		public Primapp(IPrim prim, Expression[] rands) { this.prim = prim; this.rands = rands; }
		override public System.String ToString() { return "<primapp: prim=" + prim + " rands=[" + Util.arrayToString(rands) + "]> "; } 
		public override object Eval(Env env)
		{
			// Debug.WriteLine("Eval->Prim: " + prim.ToString());
			Object[] eval_Rands = Eval_Rands(rands, env);
// 			DebugInfo.EvalExpression(this);
			return prim.Call(eval_Rands);
		}
	}

	public class Begin : Expression 
	{
		public Expression[] expressions;
		public Begin(Expression[] expressions) { this.expressions = expressions; }
		public override string ToString()	{	return "<begin: exps=[" + Util.arrayToString(expressions) + "]> ";	}
		public override object Eval(Env env) 
		{
			Expression[] exps = expressions;
			// Debug.WriteLine("Eval->Begin");
			for (int i=0; i<exps.Length-1; i++)
				exps[i].Eval(env);
			return exps[exps.Length-1].Eval(env);
		}
	}

	public class If : Expression 
	{
		public Expression test_exp, true_exp, false_exp;
		public If(Expression test_exp, Expression true_exp, Expression false_exp) 
		{
			this.test_exp = test_exp; this.true_exp = true_exp; this.false_exp = false_exp;
		}
		override public System.String ToString() { return "<if: " + test_exp + true_exp + false_exp + "> "; } 
		public override object Eval(Env env)
		{
			object testVal = test_exp.Eval(env);
			// Debug.WriteLine("Eval->If: " + testVal);
			if (!(testVal is bool)) 
				throw new Exception("invalid test expression type in if " + testVal.ToString());

			if ((testVal is bool) && (((bool) testVal) == false)) // return false only if a bool false
				return false_exp.Eval(env);
			else
				return true_exp.Eval(env);
		}
	}

	public class Assignment : Expression
	{
		public Symbol id; 
		public Expression val;
		public Assignment(Symbol id, Expression val) { this.id = id; this.val = val; }
		public override string ToString()
		{
			return "<assignment id=" + id + " val=" + val + ">";
		}

		public override object Eval(Env env)
		{
			// Debug.WriteLine("Eval->Assign: " + id);
			object valEval = val.Eval(env);
// 			DebugInfo.EvalExpression(this);
			return env.Bind(id, valEval);
		}
	}

	public class App : Expression
	{
		public Expression rator; 
		public Expression[] rands;
		public App(Expression rator, Expression[] rands) { this.rator = rator; this.rands = rands; }
		override public System.String ToString() { return "<app: rator=" + rator + ", rands=[" + Util.arrayToString(rands) + "]> "; } 
		public override object Eval(Env env)
		{
			Object proc = rator.Eval(env);
			Object[] args = Eval_Rands(rands, env);
// 			DebugInfo.EvalExpression(this);
			// Debug.WriteLine("Eval->App: " + proc + " " + args);
			if (proc is Closure)
			{
// 				DebugInfo.Push(proc as Closure, rator, args, marker);

				object result = (proc as Closure).Eval(args);
				
// 				DebugInfo.Pop(marker);
				
				return result;
			}
			else
				throw new Exception("invalid operator" + proc.ToString());
		}
	}
}
