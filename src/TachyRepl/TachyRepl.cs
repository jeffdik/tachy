using System;
using System.IO;

namespace Tachy
{
	// Environment
	public class Interpreter
	{
		public static void Repl(string loadFile)
		{
			int start = Environment.TickCount;

			A_Program prog = null;
			for (int i = 0; i < 10; i++)
			{
				prog = new A_Program();
				prog.LoadEmbededInitTachy();
			}

			int end = Environment.TickCount;

			if (loadFile != null)
			{
				String init = File.OpenText(loadFile).ReadToEnd();
				prog.Eval(new StringReader(init));
			}
			else
			{
				while (true)
				{
					// Application.DoEvents();
					StreamWriter str = new StreamWriter("transcript.ss", true);
					try 
					{
						Console.WriteLine("(" + (end - start) + " ms)");
						Console.Write("> ");
					
						String val = ReadAvailable();

						str.WriteLine(val);

						start = Environment.TickCount;
						object result = prog.Eval(new StringReader(val));
						end = Environment.TickCount;
						
						Console.WriteLine(result);
					} 
					catch (Exception e) 
					{
						Console.WriteLine("Tachy Error: " + e.Message); // + e.Message); // .Message);
						Console.WriteLine("Stacktrace: " + e.StackTrace);
					}
					str.Close();
				}
			}
		}

                static string ReadAvailable()
                {
                    StringWriter writer = new StringWriter();
                    char[] buffer = new char[1024];
                    int chars_read = buffer.Length;

                    writer.Write((char) Console.In.Read());

                    while ((chars_read == buffer.Length) ||
                           (Console.In.Peek() != -1))
                    {
                        chars_read = Console.In.Read(buffer, 0, buffer.Length);
                        writer.Write(buffer, 0, chars_read);
                        if (chars_read != buffer.Length)
                            break;
                    }

                    return writer.ToString().TrimEnd();
                }

		static void Main(string[] args)
		{
			if (args.Length > 0)
				Repl(args[0]);
			else	
				Repl(null);
			
			//ReplForm replForm = new ReplForm();
			//Application.Run(replForm);
		}
	}

}