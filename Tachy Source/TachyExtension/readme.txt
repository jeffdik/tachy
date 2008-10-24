
Tachy extension was written to debug Tachy code with Visual Studio.
It was not created with the VSIP SDK (which microsoft released for such purposes), 
but simply as a standard Visual Studio Extension.

This software is an alpha version, and will probably remain so forever. I wrote this to experiment 
with Tachy and Scheme, and it suits me fine as it currently is for this purpose. 
I might fix bugs in it when I encounter them, but don't count on it. 

To the extend that I might have any rights, I'm just following Kenrick and am releasing 
anything I wrote on Tachy or this extension under the same BSD License: http://www.opensource.org/licenses/bsd-license.html
(See the readme.txt file in the Tachy project.) 

If anyone else makes an adaption to Tachy or this extension, and releases it, I appreciate
to be notified about that.

June 10, 2005

Peter de Laat
peter_delaat@hotmail.com


To install the Add-in,: run "TachySetup.msi"

Known Issues:
* Tabsize of the text file with Tachy code needs to be 4, otherwise the code will not 
  be properly selected when stepping through it.

* Running through a document in the debugger always returns an empty result in the 
  Tachy Output Window

* Sometimes when running, it goes into stepping mode

* When debugging InitTachy.ss, it starts debugging the macro expansion too, and it crashes on it.

* Besides some examples with small readme files, there is no help or documentation.
