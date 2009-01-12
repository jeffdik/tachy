=============
This project was downloaded from Microsoft 
It was in the ToolWindow.exe file on the "Visual Studio .NET: Automation Samples" page 
   at http://www.microsoft.com/downloads/details.aspx?FamilyId=EE1C9710-6DF7-4F3F-A5AE-425A478DDEEB&displaylang=en
It is required by TachyExtension's toolwindows
The text below was in the readme.txt file supplied by Microsoft
=============

VsUserControlHost

Because .NET UserControls are not ActiveX controls, a UserControl cannot be directly hosted on a tool window. This 
project is an ATL ActiveX control that can be hosted on a tool window. It then can create an instance of a UserControl
and parent that control onto the ActiveX control, making it look as if the UserControl is within a tool window frame.

To use this control, first build the project. Then add a reference to the VSUserControlHostLib COM object within the COM tab of the Add References dialog (found by right 
clicking on your Add-in Project). Then, in your Add-in, you would write code such as the following:

object objTemp;
VSUserControlHostLib.IVSUserControlHostCtl objVSUserControlHostCtl;
EnvDTE.Window objWindow = objDTE.Windows.CreateToolWindow(objMyAddin, "VSUserControlHost.VSUserControlHostCtl", "My tool window", "{some random guid}", objVSUserControlHostCtl);


Next, you would call the appropriate code to host your UserControl. The method to do this has two formats. The first looks for an assembly that is in the GAC. 
This is demonstrated with this code, which will use a standard CheckBox as the UserControl:
	objVSUserControlHostCtl.HostUserControl("System.Windows.Forms", "System.Windows.Forms.CheckBox");

The second format is to specify an absolute path to an assembly. This is best when you do not have permissions to write into the GAC or you wish to use a user control within the 
assembly that is calling the code:
	objVSUserControlHostCtl.HostUserControl("C:\path\WindowsControlLibrary1.dll", "WindowsControlLibrary1.UserControl1");
If the user control is within the same assembly that the Add-in is located, you could use code like this to discover the path:
	objVSUserControlHostCtl.HostUserControl(System.Reflection.Assembly.GetExecutingAssembly().CodeBase, "WindowsControlLibrary1.UserControl1");


The second parameter in both method calls is the full name to the class implementing the UserControl.