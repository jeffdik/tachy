using System;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using EnvDTE;
using System.Threading;
using System.IO;
using Microsoft.Office.Core;


namespace Tachy
{
	/// <summary>
	/// Summary description for Marker.
	/// </summary>
	public class Marker
	{
		internal Document document;
		internal int startLine;
		internal int startCol;
		internal int endLine;
		internal int endCol;
		internal string Text;
		
		public Marker(Document document, int startLine, int startCol, int endLine, int endCol, string Text)
		{
			this.document = document;
			this.startLine = startLine;
			this.startCol = startCol;
			this.endLine = endLine;
			this.endCol = endCol;
			this.Text = Text;
		}
	}

	public class DebugInfo
	{
		public static A_Program tachyProg;
		public static _DTE applicationObject;

		public enum Command {Run, Step, StepOver, Abort, EvalImmediate, ShowNextExpression}

		public static bool tachyMainCommandEventIsWaiting = false;
		public static ManualResetEvent debugWaitEvent;
		public static bool debugWaitEventIsWaiting;
		public static Command command;
		private static bool InitCalled = false;

		public static bool RunHidden = true;
		public static int stepOverProcsCount = -1;
		public static object Immediate = null;
		public static bool BreakPointsSet = false;

		public static ArrayList calls;
		public static ArrayList procs;
		public static ArrayList envs;
		public static ArrayList vals;
		public static ArrayList procIndexOfItemInCallStack;
		public static Hashtable hiddenBindings ;

		public static CommandBar tachyBar;
		public static OutputWindowPane tachyOutputWindowPane;
		public static PropertyGrid propertyGrid = null;
		public static ListView callstackListView = null;
		public static ListView localsListView = null;


		public static void Init()
		{
			InitCalled = true;

			debugWaitEvent = new ManualResetEvent(false);
			debugWaitEventIsWaiting = false;

			RunHidden = true;
			stepOverProcsCount = -1;
			Immediate = null;

			calls = new ArrayList();
			procs = new ArrayList();
			envs = new ArrayList();
			vals = new ArrayList();
			procIndexOfItemInCallStack = new ArrayList();
			hiddenBindings = new Hashtable();

			propertyGrid.SelectedObject = null;
			callstackListView.Items.Clear();
			localsListView.Items.Clear();

			Push(tachyProg);
			SetEnvironment(tachyProg.initEnv);
		}

		internal static void EvalExpression(Expression expression)
		{
			if (!InitCalled)
				return;

			if ((!RunHidden) && (expression.marker != null))
			{
				MarkExpression(expression.marker);
				Wait(expression);
			}
			else if ((expression.marker != null) && (command != Command.EvalImmediate) && LineHasBreakPoint(expression.marker))
			{
				Pause();
				MarkExpression(expression.marker);
				Wait(expression);
			}
		}

		private static bool LineHasBreakPoint(Marker marker)
		{
			if ((!InitCalled) || (!BreakPointsSet))
				return false;

			string myName = marker.document.FullName.ToUpper();
			Breakpoints breakPoints = applicationObject.Debugger.Breakpoints;
			foreach (EnvDTE.Breakpoint breakPoint in breakPoints)
			{
				if ((breakPoint.File.ToUpper() == myName) &&
					 (breakPoint.FileLine == marker.startLine))
					return true;
			}
			return false;
		}

		internal static void MarkExpression(Marker marker)
		{
			if (!InitCalled)
				return;

			TextDocument textDocument = (TextDocument)marker.document.Object("TextDocument");
			TextSelection textSelection = textDocument.Selection;
			textSelection.MoveTo(marker.startLine, marker.startCol, false);
			textSelection.MoveTo(marker.endLine, marker.endCol, true);
		}


		public static void Go(Command command)
		{
			if (!InitCalled)
				return;

			DebugInfo.command = command;
			debugWaitEvent.Set();
		}

		public static void Pause()
		{
			if (!InitCalled)
				return;

			RunHidden = false;
			debugWaitEvent.Reset();
		}

		public static Command Wait(Expression currentExpression)
		{
			if (!InitCalled)
				return Command.Run;
			
			ShowCalls();

			do
			{
				debugWaitEventIsWaiting = true;
				DebugInfo.SetEnableButtons();
				debugWaitEvent.WaitOne();
				debugWaitEventIsWaiting = false;
				DebugInfo.SetEnableButtons();

				if (command != Command.Run)
					debugWaitEvent.Reset();

				switch (command)
				{
					case Command.Abort :
						Init();
						throw new Exception("Abort");
			
					case Command.EvalImmediate :
						EvalImmediate();
						break;

					case Command.ShowNextExpression :
						MarkExpression(currentExpression.marker);
						currentExpression.marker.document.Activate();

						break;
				}

			} while ((command != Command.Run) && (command != Command.Step) && (command != Command.StepOver));

			return command;
		}

		public static void SetEnableButtons()
		{
			try 
			{
				//if ((tachyBar != null) && (tachyBar.Controls != null))
				{
					foreach (CommandBarControl commandBarControl in tachyBar.Controls)
						commandBarControl.Enabled = GetEnabledStatus(commandBarControl.Tag);

					Application.DoEvents();
				}
			}
			catch(System.Exception e)
			{
				MessageBox.Show("e.Message" + "\r\n\r\n" + e.InnerException + "\r\n\r\n" + e.StackTrace);
			}
			
		}

		public static bool GetEnabledStatus(string commandName)
		{
			return
				(commandName == "TachyExtension.Connect.Start")||
				((commandName == "TachyExtension.Connect.BreakAll") && (tachyProg != null) && (!debugWaitEventIsWaiting) && (!tachyMainCommandEventIsWaiting))||
				((commandName == "TachyExtension.Connect.Stop") && (tachyProg != null))||
				((commandName == "TachyExtension.Connect.Restart") && (tachyProg != null))||
				(commandName == "TachyExtension.Connect.StepInto")||
				(commandName == "TachyExtension.Connect.StepOver")||
				((commandName == "TachyExtension.Connect.StepSelected") && tachyMainCommandEventIsWaiting)||
				((commandName == "TachyExtension.Connect.RunSelected") && (tachyMainCommandEventIsWaiting || debugWaitEventIsWaiting ) )||
				((commandName == "TachyExtension.Connect.StepCurrentDocument") && (tachyMainCommandEventIsWaiting))||
				((commandName == "TachyExtension.Connect.RunCurrentDocument") && (tachyMainCommandEventIsWaiting || debugWaitEventIsWaiting ) )||
				((commandName == "TachyExtension.Connect.ShowNextStatement") && (debugWaitEventIsWaiting))||
				(commandName == "TachyExtension.Connect.Clear")||
				(commandName == "TachyExtension.Connect.ShowAllTachyWindows")||
				(commandName == "TachyExtension.Connect.HideAllTachyWindows")||
				false;//the only function of false is to ease adding commands at the bottom with copy/paste

		}

		private static Env GetCurrentEnvFromCallStackListView()
		{
			if (!InitCalled)
				return null;

			Env EvalEnv = null;
			if (procs.Count != 0)
			{
				int callStackIndex;
				if (callstackListView.SelectedIndices.Count == 0) 
					callStackIndex = 0;
				else
					callStackIndex = callstackListView.SelectedIndices[0];

				if (procIndexOfItemInCallStack.Count > callStackIndex) 
				{
					int procIndex = callstackListView.Items.Count - 1 - (int) procIndexOfItemInCallStack[callStackIndex];
					if (envs.Count > procIndex)
						EvalEnv = (Env) envs[procIndex];
				}
			}

			return EvalEnv;
		}

		private static void EvalImmediate()
		{
			if (!InitCalled)
				return;

			Env EvalEnv = GetCurrentEnvFromCallStackListView();

			if (EvalEnv == null)
				MessageBox.Show("The environment to execute the selected text could not be determined. This Error should not occur.");
			else
			{
				bool curRunHidden = RunHidden;
				RunHidden = true;
				try
				{
					object result = null;
					if (Immediate is string)
						result = tachyProg.Eval((string) Immediate, EvalEnv);
					else if (Immediate is TextReader)
						result = tachyProg.Eval((TextReader) Immediate, EvalEnv);
					else
						MessageBox.Show("The environment to execute the selected text could not be determined. This Error should not occur.");

					if (result != null)
						tachyOutputWindowPane.OutputString(result.ToString() + "\n");
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message + "      " + e.InnerException);
				}
				finally
				{
					RunHidden = curRunHidden;
				}
			}
		}

		public static void Push(A_Program prog)
		{
			if (!InitCalled)
				return;

			procs.Add(prog);
			procIndexOfItemInCallStack.Add(procs.Count - 1);

			//ListViewItem lvi = callstackListView.Items.Add("");
			//lvi.SubItems.Add("(Tachy)");
			calls.Add("(Tachy)");
		}

		public static void Push(Closure proc, Expression rator, Object[] args, Marker marker)
		{
			if (!InitCalled)
				return;

			if (command == Command.EvalImmediate)
				return;

			procs.Add(proc);

			if (marker == null)
				return;

			if ((command == Command.StepOver) && (stepOverProcsCount == -1))
			{
				RunHidden = true;
				stepOverProcsCount = procs.Count;
			}

			procIndexOfItemInCallStack.Add(procs.Count - 1);
			string ratorname;
			if (rator is Var)
				ratorname = ((Tachy.Var) rator).id.ToString();
			else
				ratorname = "...";

			string argsstr = "";
			foreach (object arg in args)
				argsstr = argsstr + " " + arg.ToString();

			//ListViewItem lvi = callstackListView.Items.Add("");
			//lvi.SubItems.Add("(" + ratorname + argsstr + ")");
			calls.Add("(" + ratorname + argsstr + ")");
		}

		public static void Pop(Marker marker)
		{
			if (!InitCalled)
				return;

			if (command == Command.EvalImmediate)
				return;

			SetEnvironment(null);
			procs.RemoveAt(procs.Count - 1);

			if (marker != null)
			{
				if ((command == Command.StepOver) && (stepOverProcsCount == procs.Count + 1))
				{
					RunHidden = false;
					stepOverProcsCount = -1;
				}

				//callstackListView.Items.RemoveAt(0);
				calls.RemoveAt(calls.Count - 1);
				procIndexOfItemInCallStack.RemoveAt(procIndexOfItemInCallStack.Count - 1);
			}
		}

		public static void SetEnvironment(Env env)
		{
			if (!InitCalled)
				return;

			if (command == Command.EvalImmediate)
				return;

			while (procs.Count > envs.Count)
				envs.Add(null);
			
			envs[procs.Count - 1] = env; 
		}

		public static void ShowCalls()
		{
			if (!InitCalled)
				return;

			try
			{
				callstackListView.Items.Clear();

				foreach (object call in calls)
				{
					ListViewItem lvi = callstackListView.Items.Add("");
					lvi.SubItems.Add((string) call);
				}
				if (callstackListView.Items.Count > 0)
					callstackListView.Items[0].Selected = true;
			}
			catch {}
		}

		public static void ShowLocals(Env env)
		{
			if (!InitCalled)
				return;

			try
			{
				localsListView.Items.Clear();
				vals.Clear();

				Env curEnv = env;
				string padStr = "    ";
				string totalpadStr = "";
				while (curEnv is Extended_Env)
				{
					foreach (DictionaryEntry binding in ((Extended_Env) curEnv).bindings)
					{
						if ((!(binding.Value is Closure)) && (hiddenBindings[binding.Key] == null))
						{
							vals.Add(binding.Value);

							ListViewItem lvi = localsListView.Items.Add("");
							lvi.SubItems.Add(totalpadStr + binding.Key.ToString());
							lvi.SubItems.Add(binding.Value.ToString());
						}
					}

					curEnv = ((Extended_Env) curEnv).env;
					totalpadStr = totalpadStr + padStr;
				}
				if (localsListView.Items.Count > 0)
					localsListView.Items[0].Selected = true;
			}
			catch {}
		}

		public static void AddHiddenBindings(Env env)
		{
			if (!InitCalled)
				return;

			Env curEnv = env;
			while (curEnv is Extended_Env)
			{
				foreach (DictionaryEntry binding in ((Extended_Env) curEnv).bindings)
					hiddenBindings.Add(binding.Key, binding.Value);

				curEnv = ((Extended_Env) curEnv).env;
			}
		}

		public static void LocalsListView_ItemActivate(object sender, System.EventArgs e)
		{
			if (!InitCalled)
				return;

			if ((localsListView.SelectedIndices.Count > 0) && (vals.Count > localsListView.SelectedIndices[0]))
				propertyGrid.SelectedObject = vals[localsListView.SelectedIndices[0]];
		}

		public static void CallstackListView_ItemActivate(object sender, System.EventArgs e)
		{
			if (!InitCalled)
				return;

			try
			{
				ShowLocals(GetCurrentEnvFromCallStackListView());
			}
			catch {}
		}

	}
}
