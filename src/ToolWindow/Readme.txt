This folder contains Add-ins using VB, C#, and C++ to create Tool Windows. When loaded the Add-in 
will create a tool window, then host a UserControl (for C# & VB) or an ActiveX control (C++) inside of that tool window. 

Instructions for VB & C#:
------------------------------------------------------------------------------------------------------------------------
To run the Add-ins, follow these steps:
1) Load the project in the VSUserControlHost directory, then build it. 
2) Load either the C# or VB version of the project you would like to use.
3) If the reference to the VSUserControlHostLib is broken, you will need to remove it and add it again.  In the 
references under the ToolWindowAddin, remove the reference to VSUserControlHostLib_1_0 by right-clicking on it and selecting 'Remove'.  Then add a reference to the vsusercontrolhostlib_1_0.dll located in the ToolWindow directory.
4) From a command prompt, find the UserControl (either CSharpToolWindow\ToolWinControl\bin\Debug for C# or VToolWindow\ToolWinControl\bin\Debug for VB),
	and type the command 'gacutil -i ToolWinControl.dll'. This step must be performed every time you build a new version of the ToolWinControl.
4) In either the CSharpToolWindow directory or VBToolWindow directory (depending on which language you are using), run the reg file 'AddinReg.reg'.
5) Start a new instance of Visual Studio.NET, and in the Add-in manager, load the Add-in you just built.

The VSUserControlHost project is a 'Shim' control which is used to host a .NET UserControl. It creates an instance of 
the .NET runtime, instantiates the UserControl, then parents that control onto the VSUserControlHost. VSUserControlHost is
a general-purpose ATL control, which you may ship as part of your product to 



Instructions for Visual C++:
------------------------------------------------------------------------------------------------------------------------
To run this Add-in, follow these steps:
1) Load the solution in the CPPToolWindow directory, and build it.
2) double click the file Addinreg.reg in the CPPToolWindow directory.
3) Start a new instance of Visual Studio.NET, and in the Add-in manager, load the Add-in you just built.
