
In this readme:

	A.	Prerequisites to use this program
	
	B.	How to use this example solution to debug dotnet code in combination with debugging Tachy code.

	C.	How to setup your own dotnet projects to debug them in combination with Tachy code
	
	D.	** EXTREMELY IMPORTANT **


A. Prerequisites to use this program
====================================

1. You have installed the Tachy Add-in by running TachySetup.msi

2. You have activated the Tachy Add-in. (In Visual Studio 2003 this can be done by
selecting "Tools | Add-in Manager" from the main menu and selecting Tachy)

3. Set the debugging working directory of the CsharpClass1 project to the directory 
containing "TachyTestSolution.sln". This is required because the working directory will 
depend on where you decided to put the examples.

	In Visual Studion 2003, this can be done with the following steps:
		1. Right click on the CsharpClass1 project.
		2. Select Properties from the dropdown menu.
		3. Open the Configuration Properties folder, and in it the Debugging page.
		4. Change the working directory to the directory containing "TachyTestSolution.sln" 


B. How to use this example solution to debug dotnet code in combination with debugging Tachy code
=================================================================================================
	
After setting the working directory, and installing and activating the Tachy Add-in, 
press F5 to run the program:
	
	Another version of Visual Studio should open now with the example Tachy solution.
	If the Tachy toolbar is not visible, make it visible by right clicking a ToolBar and 
	selecting Tachy	in the Popup menu. 
	
You can set breakpoints in both Class1 and Class2 and when the Tachy code makes a call to 
the either class, you can debug your dotnet class.


C.	How to setup your own dotnet projects to debug them in combination with Tachy code
======================================================================================

1. Make sure you have two solutions. One with your dotnet code, your dotnet solution, and 
one with your Tachy code, your Tachy solution.

2. Open your dotnet solution and pick any of your projects with classes you want to debug 
and make this you startup project.

3.	Open the Properties window of this startup project and in the debugging page of the 
	Configuration Properties folder, make the following changes:
		a. Set the debug mode to Program
		b. Press the Apply button (or else you cannot do step c, d, e)
		c. Set the Start Application to devenv.exe
		d. Set the working directory to the directory containing your Tachy Solution
		e. Set the Command line arguments to your Tachy Solution

4. In your Tachy code, while debugging, make sure you load dll's you want to debug 
from the bin/debug directory, and not the release directory. You might consider to dynamically
build the string for the assembly, depending on the release or debug version you want to load.


=======================
D.	EXTREMELY IMPORTANT 
=======================

**Never** stop a debug session by pressing "Stop Debugging" in the dotnet solution, because 
this will kill the second version of Visual Studio, and you will loose the changes you made 
to your Tachy Code. Instead, always close the Tachy Solution yourself, so you are allerted 
to save your changes. You might even consider (temporarily) removing your "Stop Debugging" 
button from the Toolbar alltogether while developing Tachy code in combination with dotnet 
code, to avoid losing code.

