
;; To debug this program:
;;	1.	Make sure that the Tachy Add-in is installed (run TachySetup.msi) 
;;		and activated (through menu "Tools | Add-in Manager")
;;	2.	Use the	Tachy Toolbar debug buttons (the red versions of standard debugging toolbar)


(define (myadd x y)
	(+ x y))
	
(define (myaddadd a b c)
		  (myadd a (myadd b c)))

(myadd 3 4)

(myaddadd 
  		(myaddadd 6 7 8) 
	8 9)


(define (myproc x y)
	(let ((xx 5) (yy 6)) (+ x y xx yy)))

(myproc 15 3)


;;
;; While debugging through myadd or myproc, try clicking on different rows in the callstack
;; In the Locals window, you will see the variables of the environment of the procedure
;;
;; When using "run through selected text" the environment is used of the selected procedure
;; in the call stack
;; 
;; After all statements are executed, Tachy will remain active, and you can keep selecting
;; statements or documents and step or run through them. Click the red 'Stop Debugging'
;; button to Stop Tachy
;;