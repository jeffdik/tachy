/* Tachy 
 * A Scheme-like (R5RS is the template, but not the goal) language for the .NET framework.
 * (c) 2002-2005 Kenrick Rawlings & Peter de Laat
 *
 * Email: krawling@yahoo.com & peter_delaat@hotmail.com
 * Project Homepage:	http://kenrrawlings.com/pages/tachy
 * 
 * This software is released under the BSD License at: http://www.opensource.org/licenses/bsd-license.html 
 * where owner and organization are "Kenrick Rawlings" and year is "2002-2005"
 */

Note: Tachy requires the release version of the .NET Runtime, available at:
http://msdn.microsoft.com/downloads/default.asp?url=/downloads/sample.asp?url=/msdn-files/027/001/829/msdncompositedoc.xml

Description
----------------
* Tachy is a Scheme-like (R5RS is the template, but not the goal) language that 
is being developed in C# for the .NET framework and is made available as Open 
Source (BSD License). The current implementation is an interpreter, and a 
compiler is planned. The primary focus for Tachy is for programming language 
learning & experimentation, and an attempt has been made to keep the code as 
small(currently under 1000 lines) and straightforward as possible.

* The general philosophy is to stay as close as reasonably possible to Scheme, 
but transperancy and integration with the .NET runtime always trumps R5RS. For 
instance, eq? and eqv? are available, but they call the CLR Object.Equals() and 
Object.Is Equivalent() methods directly rather than follow strict adherence to 
the Scheme specification. 

* Special forms implemented: define if, lambda, set!, quote, and, or, let, letrec, cond

* Uses unwrapped .NET Common Language Runtime (CLR) types whenever possible (a 
string is a System.String, an integer is a System.Int32, etc) . As a result 
there are only three custom data types for expressed values: Symbol, Pair and 
Closure. 

* Since Tachy does not implement its own type system and raw CLR objects are 
valid expressed values, there is no need for a Foreign Function Interface. If a 
CLR class is available in the current Assembly, it can be used. 

* Tachy can create new obects instances of available CLR classes, call object 
methods (and static methods on classes), and get and set the values of object 
fields and properties. Defining new classes is currently unavailable, but is 
planned. 

* There is no language support for continuations 

Documentation
--------------------

* There's not much yet. Browse the code and take a close look at init.ss, which 
uses Tachy's ability to create .NET CLR objects and call their methods to 
implement several core Scheme functions.

* Many scheme functions are missing. These will be added over time, writing them 
in Tachy itself as much as possible. 

Thanks to 
-------------
* Daniel Friedman for igniting my interest in programming languages

* The writers of Essentials Of Programming Languages (Daniel P. Friedman, 
	Mitchell Wand, and Christopher T. Haynes), which provided the initial Scheme
	interprerter(see listing 3.6, EOPL second edition) that served as the launching
	point for Tachy.
	
* Peter Drayton for an extremely clever workaround for C#'s inability to apply the 
	base operators(+,-,*,/) dynamically on object instances at runtime. This turned
	what was quickly becoming many many lines of code into only 4. 
	See: http://www.razorsoft.net/weblog/2002/03/16.html#a67

Changelog
-----------------
08.12.05:
+Integrated Peter de Laat's changes, including his excellent Tachy debugger and Visual Studio.net add-in.
 
04.17.05 
+Separated the Tachy interpreter into a DLL to make it more easy to embed
+Visual studio.net 2003 project now included
+Let, letrec, and, or, cond special forms now supported
+Added preliminary macro system (for example of use see implementation of above special forms in init.ss)
+Define special form now supports creation of functions without explicit lambda (a form used extensively in SICP)
+Many more standard R5RS library functions now available

03.19.02 [Build 1] Initial Release
