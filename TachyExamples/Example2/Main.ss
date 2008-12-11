
;; To debug this program:
;;	1.	Change the path of the CsharpClass1.dll and CsharpClass2.dll debugging dll's below to match your own directory structure
;;	2.	Make sure that the Tachy Add-in is installed (run TachySetup.msi) 
;;		and activated (through menu "Tools | Add-in Manager")
;;	3.	Use the	Tachy Toolbar debug buttons (the red versions of standard debugging toolbar)


(load-assembly-from "CsharpClass1" "..\Example2DotnetClasses\CsharpClass1\bin\Debug\CsharpClass1.dll")
(load-assembly-from "CsharpClass2" "..\Example2DotnetClasses\CsharpClass2\bin\Debug\CsharpClass2.dll")


(set! class1Object1 (new 'Class1 5 6))
(set! class1Object2 (new 'Class1 15 16))

(set! class2Object (new 'Class2 class1Object1 class1Object2))

(define (myadd x y)
	(+ x y))
	
(myadd class2Object.A.X class2Object.B.Y)

(define (myproc x y)
	(let ((xx 5) (yy 6)) (+ x y xx yy)))

(myproc class1Object1.X class2Object.A.X)


;; To debug the Class1 Class2 dotnet code in combination with this Tachy code:
;;		* Close this project
;;		* Open the dotnetclasses.sln in the TachyExamples\Example2DotnetClasses directory
;;		* Follow the instructions in the readme file