
This project is required to get button images in the toolbar.

To add another image:

* Right click the Resource Files folder in the TachyAddInImages project
* Choose "Add | Add Resource" from the popup menu
* In the window that opens:
	* Choose Bitmap, and import or new
	* Set the size on 16*16 in the properties
* Set the transparent color:
	* select the light green color with the left mouse button in the color palette
	* select "Image | Adjust colors" from the menu
	* change the green value from 255 to 254, and leave red and blue 0
	* use the new light green color as the transparent color
* Edit the image
* Save the image
* The EnvDTE.commands.AddNamedCommand method requires a bitmap int to select the image you created. 
  This bitmap int can be obtained by:
	* clicking "View | Resource View" in the menu
	* right clicking on a resource in the resource View window
	* select "Resource symbols" from the popup menu
	* the bitmap int value is in the value column for each bitmap
	
	