using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Threading;
using Microsoft.Office.Core;
using Extensibility;
using System.Runtime.InteropServices;
using EnvDTE;
using Tachy; 
using VSUserControlHostLib;


namespace TachyExtension
{
	#region Read me for Add-in installation and setup information.
	// When run, the Add-in wizard prepared the registry for the Add-in.
	// At a later time, if the Add-in becomes unavailable for reasons such as:
	//   1) You moved this project to a computer other than which is was originally created on.
	//   2) You chose 'Yes' when presented with a message asking if you wish to remove the Add-in.
	//   3) Registry corruption.
	// you will need to re-register the Add-in by building the MyAddin21Setup project 
	// by right clicking the project in the Solution Explorer, then choosing install.
	#endregion
	
	/// <summary>
	///   The object for implementing an Add-in.
	/// </summary>
	/// <seealso class='IDTExtensibility2' />
	[GuidAttribute("277C9372-4C29-4BF7-B42E-B14F81A2BDE4"), ProgId("TachyExtension.Connect")]
	public class Connect : Object, Extensibility.IDTExtensibility2, IDTCommandTarget
	{
		private AddIn addInInstance;
		private bool initialized = false;

		private System.Threading.Thread tachyThread;
		private bool stopTachyThreadProc;
		private ManualResetEvent tachyMainCommandEvent;
		private string tachyThreadCommandName;

		private EnvDTE.Window propertyToolWindow;
		private EnvDTE.Window callstackToolWindow;
		private EnvDTE.Window localsToolWindow;

		//private int start;
		
		public Connect()
		{
		}

		public void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref System.Array custom)
		{
			DebugInfo.applicationObject = (_DTE)application;

			addInInstance = (AddIn)addInInst;
			try
			{
				if (! initialized)
				{
					InitTachyToolbar();
					InitTachyToolWindows();

					initialized = true;
				}

			}
			catch(System.Exception e)
			{
				MessageBox.Show(e.Message + "\r\n\r\n" + e.InnerException + "\r\n\r\n" + e.StackTrace);
			}
		}

		private void InitTachyToolbar()
		{
			foreach (CommandBar commandBar in DebugInfo.applicationObject.CommandBars)
			{
				if (commandBar.Name == "Tachy")
				{
					DebugInfo.tachyBar = commandBar;
					return;
				}
			}

			DebugInfo.tachyBar = DebugInfo.applicationObject.CommandBars.Add("Tachy", MsoBarPosition.msoBarTop, false, false);

			AddCommand(DebugInfo.tachyBar, "Start", "Run", "Run", true, 1009, "Global::alt+f5", false);
			AddCommand(DebugInfo.tachyBar, "BreakAll", "Break All", "Break All", true, 1010, "Global::ctrl+alt+shift+break", false);
			AddCommand(DebugInfo.tachyBar, "Stop", "Stop Debugging", "Stop Debugging", true, 1011, "Global::alt+shift+f5", false);
			AddCommand(DebugInfo.tachyBar, "Restart", "Restart", "Restart", true, 1012, "Global::alt+ctrl+shift+f5", false);

			AddCommand(DebugInfo.tachyBar, "ShowNextStatement", "Show Next Statement", "Show Next Statement", true, 1013, "Global::alt+n", true);
			AddCommand(DebugInfo.tachyBar, "StepInto", "Step Into", "Step Into", true, 1014, "Global::alt+f11", false);
			AddCommand(DebugInfo.tachyBar, "StepOver", "Step Over", "Step Over", true, 1015, "Global::alt+f10", false);
			AddCommand(DebugInfo.tachyBar, "StepSelected", "Step Through Selected Text", "Step Through Selected Text", true, 1016, "Global::alt+f7", false);
			AddCommand(DebugInfo.tachyBar, "RunSelected", "Run Through Selected Text", "Run Through Selected Text", true, 1017, "Global::alt+shift+f7", false);
			AddCommand(DebugInfo.tachyBar, "StepCurrentDocument", "Step Through Current Document", "Step Through Current Document", true, 1018, "Global::alt+f8", false);
			AddCommand(DebugInfo.tachyBar, "RunCurrentDocument", "Run Through Current Document", "Run Through Current Document", true, 1019, "Global::alt+shift+f8", false);

			AddCommand(DebugInfo.tachyBar, "Clear", "Clear Output Window", "Clear Output Window", true, 1022, "", true);
			AddCommand(DebugInfo.tachyBar, "ShowAllTachyWindows", "Show All Tachy Windows", "Show All Tachy Windows", true, 1020, "", false);
			AddCommand(DebugInfo.tachyBar, "HideAllTachyWindows", "Hide All Tachy Windows", "Hide All Tachy Windows", true, 1021, "", false);

			DebugInfo.tachyBar.Visible = true;
		}

		private void AddCommand(CommandBar commandBar, string name, string buttonText, string toolTip, bool addToCommandBar, int bitMap, string bindings, bool beginGroup)
		{
			object []contextGUIDS = new object[] { };
			Commands commands = DebugInfo.applicationObject.Commands;

			Command command = null;
			try //try to retrieve the command with the name
			{
				command = commands.Item(addInInstance.ProgID + "." + name, -1);
			}
			catch {}

			if (command == null) //if not retrieved, add the command with the name
			{
				if (bitMap == 0)
					command = commands.AddNamedCommand(addInInstance, name, buttonText, toolTip, true, 59, ref contextGUIDS, (int) vsCommandStatus.vsCommandStatusEnabled);
				else
					command = commands.AddNamedCommand(addInInstance, name, buttonText, toolTip, false, bitMap, ref contextGUIDS, (int) vsCommandStatus.vsCommandStatusEnabled);
			}

			try
			{
				if (bindings != "")
					command.Bindings = bindings;
			}
			catch {}

			if (addToCommandBar)
			{
				CommandBarButton commandBarButton = (CommandBarButton) command.AddControl(commandBar, commandBar.Controls.Count + 1);
				commandBarButton.Tag = "TachyExtension.Connect." + name;
				commandBarButton.Style = MsoButtonStyle.msoButtonIcon;
				commandBarButton.BeginGroup = beginGroup;
			}
			
		}

		public void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref System.Array custom)
		{
			try
			{
				if (tachyThread != null)
				{
					stopTachyThreadProc = true;
					tachyThread.Interrupt();
					if (!tachyThread.Join(5000))
						tachyThread.Abort();
				}
			}
			catch(System.Exception e)
			{
				MessageBox.Show(e.Message + "\r\n\r\n" + e.InnerException + "\r\n\r\n" + e.StackTrace);
			}

		}

		public void OnAddInsUpdate(ref System.Array custom)
		{
		}

		public void OnStartupComplete(ref System.Array custom)
		{
			try
			{
				ShowAllWindows(false);
			}
			catch(System.Exception e)
			{
				MessageBox.Show(e.Message + "\r\n\r\n" + e.InnerException + "\r\n\r\n" + e.StackTrace);
			}

		}

		public void OnBeginShutdown(ref System.Array custom)
		{
		}
		
		public void QueryStatus(string commandName, EnvDTE.vsCommandStatusTextWanted neededText, ref EnvDTE.vsCommandStatus status, ref object commandText)
		{
			try
			{
				if (neededText == EnvDTE.vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
				{
					if (DebugInfo.GetEnabledStatus(commandName))
					{
						status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
					}
				}
			}
			catch(System.Exception e)
			{
				MessageBox.Show(e.Message + "\r\n\r\n" + e.InnerException + "\r\n\r\n" + e.StackTrace);
			}

		}

		public void Exec(string commandName, EnvDTE.vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == EnvDTE.vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				try
				{
					//start = Environment.TickCount;

					tachyThreadCommandName = commandName;
					switch (tachyThreadCommandName)
					{
						case "TachyExtension.Connect.Clear":
							ClearTachyOutputWindowPane();
							break;

						case "TachyExtension.Connect.ShowAllTachyWindows":
							ShowAllWindows(true);
							break;
						
						case "TachyExtension.Connect.HideAllTachyWindows":
							ShowAllWindows(false);
							break;
						
						default:
						{
							if (DebugInfo.tachyProg == null)
							{
								bool canceled; 
								InitProgram(out canceled);
								if (canceled) //saving open documents was canceled
									return;

								ShowAllWindows(true);
							}

						
							switch (tachyThreadCommandName)
							{
								case "TachyExtension.Connect.Start":
									DebugInfo.RunHidden = true;
									if (!DebugInfo.tachyMainCommandEventIsWaiting)
										DebugInfo.Go(DebugInfo.Command.Run);
									else
										SetTachyThread();
									break;

								case "TachyExtension.Connect.BreakAll":
									if (!DebugInfo.debugWaitEventIsWaiting)
										DebugInfo.Pause();
									break;

								case "TachyExtension.Connect.Stop":
								case "TachyExtension.Connect.Restart":
									if (!DebugInfo.tachyMainCommandEventIsWaiting)
										DebugInfo.Go(DebugInfo.Command.Abort);
									else
									{
										DebugInfo.tachyProg = null;
										if (tachyThreadCommandName == "TachyExtension.Connect.Stop")
											ShowAllWindows(false);
										else
											SetTachyThread();
									}
									break;

								case "TachyExtension.Connect.StepInto":
								case "TachyExtension.Connect.StepSelected":
								case "TachyExtension.Connect.StepCurrentDocument":
									DebugInfo.RunHidden = false;
									if (DebugInfo.debugWaitEventIsWaiting)
										DebugInfo.Go(DebugInfo.Command.Step);
									else
									{
										DebugInfo.Pause();
										SetTachyThread();
									}
									break;

								case "TachyExtension.Connect.StepOver":
									DebugInfo.RunHidden = false;
									if (DebugInfo.debugWaitEventIsWaiting)
										DebugInfo.Go(DebugInfo.Command.StepOver);
									else
									{
										DebugInfo.Pause();
										SetTachyThread();
									}
									break;

								case "TachyExtension.Connect.RunSelected":
									if (DebugInfo.debugWaitEventIsWaiting)
										EvalTextSelectionWithOpenEval();
									else
									{
										DebugInfo.RunHidden = true;
										SetTachyThread();
									}
									break;

								case "TachyExtension.Connect.RunCurrentDocument":
									if (DebugInfo.debugWaitEventIsWaiting)
										EvalDocumentWithOpenEval();
									else
									{
										DebugInfo.RunHidden = true;
										SetTachyThread();
									}
									break;

								case "TachyExtension.Connect.ShowNextStatement":
									if (DebugInfo.debugWaitEventIsWaiting)
										DebugInfo.Go(DebugInfo.Command.ShowNextExpression);
									else
										MessageBox.Show("There is no expression to be evaluated.");
									break;
						
							}
							break;
						}
					}
					handled = true;
				}
				catch(System.Exception e)
				{
					MessageBox.Show(e.Message + "\r\n\r\n" + e.InnerException + "\r\n\r\n" + e.StackTrace);
				}
			}
		}

		private void SetTachyThread()
		{
			if (tachyMainCommandEvent == null)
				CreateTachyThread();
			else
				tachyMainCommandEvent.Set();
		}



		private void CreateTachyThread()
		{
			tachyMainCommandEvent = new ManualResetEvent(true);
			DebugInfo.tachyMainCommandEventIsWaiting = false;
			tachyThread = new System.Threading.Thread(new ThreadStart(TachyThreadProc));
			tachyThread.Name = "tachyThread";
			tachyThread.Start();
		}


		private void TachyThreadProc()
		{
			stopTachyThreadProc = false;
			while (!stopTachyThreadProc)
			{
				try
				{
					if (DebugInfo.tachyProg != null)
						DebugInfo.ShowCalls();

					DebugInfo.tachyMainCommandEventIsWaiting = true;
					DebugInfo.SetEnableButtons();
					tachyMainCommandEvent.WaitOne();
					DebugInfo.tachyMainCommandEventIsWaiting = false;
					DebugInfo.SetEnableButtons();
					if (DebugInfo.RunHidden)
						DebugInfo.BreakPointsSet = (DebugInfo.applicationObject.Debugger.Breakpoints.Count > 0);

					if (!stopTachyThreadProc)
					{
						switch (tachyThreadCommandName)
						{
							case "TachyExtension.Connect.Start":
							case "TachyExtension.Connect.StepInto":
							case "TachyExtension.Connect.StepOver":
								EvalDocumentWithoutOpenEval(GetStartDocument());
								break;

							case "TachyExtension.Connect.Restart":
								bool canceled;
								InitProgram(out canceled);
								if (!canceled)
									EvalDocumentWithoutOpenEval(GetStartDocument());
								break;

							case "TachyExtension.Connect.StepSelected":
							case "TachyExtension.Connect.RunSelected":
								EvalTextSelectionWithoutOpenEval();
								break;
							
							case "TachyExtension.Connect.StepCurrentDocument":
							case "TachyExtension.Connect.RunCurrentDocument":
								EvalDocumentWithoutOpenEval(DebugInfo.applicationObject.ActiveDocument);
								break;

						}

					}

					tachyMainCommandEvent.Reset();
				}
				catch (Exception e) 
				{
					if (e.Message == "Abort")
					{
						DebugInfo.tachyProg = null;
						if (tachyThreadCommandName == "TachyExtension.Connect.Stop")
						{
							ShowAllWindows(false);
							tachyMainCommandEvent.Reset();
						}
					}
					else 
					{
						if (!(e is ThreadInterruptedException))
							MessageBox.Show(e.Message + "      " + e.InnerException + "\r\n\r\n" + e.StackTrace);
						
						tachyMainCommandEvent.Reset();
					}
				}
			}
		}

		private void InitProgram(out bool canceled)
		{
			SaveDocuments(out canceled);
			
			if (canceled)
				return;

			//start = Environment.TickCount;
			DebugInfo.tachyProg = new A_Program();
			//int newprog = Environment.TickCount - start;

			DebugInfo.Init();
		
			if (!FileIsInSolution("InitTachy.ss"))
			{
				//start = Environment.TickCount;
				DebugInfo.tachyProg.LoadEmbededInitTachy();
			}

			//DebugInfo.tachyOutputWindowPane.OutputString("(" + (newprog + Environment.TickCount - start) + " ms)\n");

			DebugInfo.AddHiddenBindings(DebugInfo.tachyProg.initEnv);
		}

		private Document GetStartDocument()
		{
			Document documentWithFile;
			if (FileIsInSolution("InitTachy.ss", out documentWithFile))
				return documentWithFile;

			if (FileIsInSolution("main.ss", out documentWithFile))
				return documentWithFile;

			return DebugInfo.applicationObject.ActiveDocument;
		}


		private void InitTachyToolWindows()
		{
			//Output Window
			OutputWindow outputWindow = (OutputWindow) DebugInfo.applicationObject.Windows.Item(Constants.vsWindowKindOutput).Object;
			bool found = false;
			foreach (OutputWindowPane outputWindowPane in outputWindow.OutputWindowPanes)
			{
				if (outputWindowPane.Name == "Tachy Output")
				{
					DebugInfo.tachyOutputWindowPane = outputWindowPane;
					found = true; 
					break;
				}
			}
			if (!found)
				DebugInfo.tachyOutputWindowPane = outputWindow.OutputWindowPanes.Add("Tachy Output");


			//Property Window
			object obj = null; 
			propertyToolWindow = DebugInfo.applicationObject.Windows.CreateToolWindow(addInInstance, "VSUserControlHost.VSUserControlHostCtl", "Tachy Properties", "{E8FC9DD0-00CC-414c-B289-A1C9CA81D27A}", ref obj );
			IVSUserControlHostCtl shimControl = (IVSUserControlHostCtl) propertyToolWindow.Object;

			PropertyWindow propWindow = (PropertyWindow) shimControl.HostUserControl(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, "TachyExtension.PropertyWindow");

			DebugInfo.propertyGrid = (PropertyGrid) GetControlFromControls(propWindow.Controls, "PropertyGrid1");
			if (DebugInfo.propertyGrid == null)
				throw new Exception("PropertyGrid1 control not found in PropertyWindow usercontrol");


			// Call Stack Window
			obj = null; 
			callstackToolWindow = DebugInfo.applicationObject.Windows.CreateToolWindow(addInInstance, "VSUserControlHost.VSUserControlHostCtl", "Tachy Call Stack", "{A68488C4-E8D7-4c32-8D00-DCA8E3FC6A5F}", ref obj );
			shimControl = (IVSUserControlHostCtl) callstackToolWindow.Object;
			ListViewWindow callStackWindow = (ListViewWindow) shimControl.HostUserControl(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, "TachyExtension.ListViewWindow");

			DebugInfo.callstackListView = (ListView) GetControlFromControls(callStackWindow.Controls, "ListView1");
			if (DebugInfo.callstackListView == null)
				throw new Exception("ListView1 control not found in ListViewWindow usercontrol");

			DebugInfo.callstackListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			DebugInfo.callstackListView.View = View.Details;
			DebugInfo.callstackListView.Sorting = SortOrder.Ascending;
			DebugInfo.callstackListView.Items.Clear();
			DebugInfo.callstackListView.FullRowSelect = true;
			DebugInfo.callstackListView.Tag = "call stack";


			// Locals Window

			obj = null; 
			localsToolWindow = DebugInfo.applicationObject.Windows.CreateToolWindow(addInInstance, "VSUserControlHost.VSUserControlHostCtl", "Tachy Locals", "{DC5CF04C-C32D-4060-A47D-984EC1BDC10B}", ref obj );
			shimControl = (IVSUserControlHostCtl) localsToolWindow.Object;
			ListViewWindow localsWindow = (ListViewWindow) shimControl.HostUserControl(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, "TachyExtension.ListViewWindow");

			DebugInfo.localsListView = (ListView) GetControlFromControls(localsWindow.Controls, "ListView1");
			if (DebugInfo.localsListView == null)
				throw new Exception("ListView1 control not found in ListViewWindow usercontrol");

			DebugInfo.localsListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			DebugInfo.localsListView.Columns.Add("Value", 500, HorizontalAlignment.Left);
			DebugInfo.localsListView.View = View.Details;
			DebugInfo.localsListView.Sorting = SortOrder.None;
			DebugInfo.localsListView.Items.Clear();
			DebugInfo.localsListView.FullRowSelect = true;
			DebugInfo.localsListView.Tag = "locals";

			ShowAllWindows(false);
		}

		private void ShowAllWindows(bool visible)
		{
			propertyToolWindow.Visible = visible;
			callstackToolWindow.Visible = visible;
			localsToolWindow.Visible = visible;
		}

		private Control GetControlFromControls(Control.ControlCollection controls, string name)
		{
			foreach (Control control in controls)
			{
				if (control.Name.ToUpper() == name.ToUpper())
					return control;
			}
			return null;
		}

		private void InitTachyListViewWindowPane()
		{

		}


		private void ClearTachyOutputWindowPane()
		{
			try
			{
				DebugInfo.tachyOutputWindowPane.Clear();
			}
			catch {};
		}

		private object InterpretDocument(Document document)
		{
 			if (document == null)
				return null;

			DocumentReader documentReader = new DocumentReader(document);
			return DebugInfo.tachyProg.Eval(documentReader);
		}

		private void EvalTextSelectionWithoutOpenEval()
		{
			TextDocument activeTextDocument = (TextDocument) DebugInfo.applicationObject.ActiveDocument.Object("TextDocument");
			object result = DebugInfo.tachyProg.Eval(activeTextDocument.Selection.Text);
			if (result != null)
				DebugInfo.tachyOutputWindowPane.OutputString(result.ToString() + "\n");

			activeTextDocument.Selection.MoveTo(activeTextDocument.Selection.BottomPoint.Line, activeTextDocument.Selection.BottomPoint.DisplayColumn, false);
		}

		private void EvalTextSelectionWithOpenEval()
		{
			TextDocument activeTextDocument = (TextDocument) DebugInfo.applicationObject.ActiveDocument.Object("TextDocument");
			DebugInfo.Immediate = activeTextDocument.Selection.Text;
			DebugInfo.Go(DebugInfo.Command.EvalImmediate);
		}

		private void EvalDocumentWithoutOpenEval(Document document)
		{
			//start = Environment.TickCount;
			object result = InterpretDocument(document);
			//DebugInfo.tachyOutputWindowPane.OutputString("(" + (Environment.TickCount - start) + " ms)\n");
			if (result != null)
				DebugInfo.tachyOutputWindowPane.OutputString(result.ToString() + "\n");

			TextDocument activeTextDocument = (TextDocument) document.Object("TextDocument");
			activeTextDocument.Selection.MoveTo(activeTextDocument.EndPoint.Line, activeTextDocument.EndPoint.DisplayColumn, false);
		}

		private void EvalDocumentWithOpenEval()
		{
			DebugInfo.Immediate = new DocumentReader(DebugInfo.applicationObject.ActiveDocument);
			DebugInfo.Go(DebugInfo.Command.EvalImmediate);
		}

		private bool FileIsInSolution(string fileName)
		{
			Document dumDoc;
			return FileIsInSolution(fileName, out dumDoc);
		}


		private bool FileIsInSolution(string fileName, out Document documentWithFile)
		{
			documentWithFile = null;
			if ((DebugInfo.applicationObject == null) || (DebugInfo.applicationObject.Solution == null) || (DebugInfo.applicationObject.Solution.Projects == null))
				return false;

			foreach (Project project in DebugInfo.applicationObject.Solution.Projects)
			{
				if (FileIsInProject(project, fileName, out documentWithFile))
					return true;
			}
			return false;
		}

		private bool FileIsInProject(Project project, string fileName, out Document documentWithFile)
		{
			documentWithFile = null;
			
			if ((project == null) || (project.ProjectItems == null))
				return false;

			foreach (ProjectItem item in project.ProjectItems)
			{
				if (item.SubProject != null)
				{
					if (FileIsInProject(item.SubProject, fileName, out documentWithFile))
					{
						return true;
					}
				}
				else if (item.Name.ToUpper() == fileName.ToUpper())
				{
					documentWithFile = item.Document;
					return true;
				}
			}
			return false;
		}

		enum OnBuildRunEnum {SaveAll, Prompt, NoSave, SaveOpen}; 

		private void SaveDocuments(out bool canceled)
		{
			canceled = false;
			Properties properties = DebugInfo.applicationObject.DTE.get_Properties("Environment", "ProjectsAndSolution");
			OnBuildRunEnum saveOption = (OnBuildRunEnum) properties.Item("OnRunOrPreview").Value;
			if (saveOption == OnBuildRunEnum.NoSave)
			{}
			else if (saveOption == OnBuildRunEnum.Prompt)
				canceled = (DebugInfo.applicationObject.ItemOperations.PromptToSave == EnvDTE.vsPromptResult.vsPromptResultCancelled);
			else if ((saveOption == OnBuildRunEnum.SaveAll) || (saveOption == OnBuildRunEnum.SaveOpen))
				DebugInfo.applicationObject.Documents.SaveAll();
		}
	}
}