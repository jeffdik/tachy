;; Tachy Utility Library

;; CLR Utility functions

(define _using '())
(define (using typestr)
	(set! _using (cons-prim typestr _using)))
(using "System")
(using "System.Collections")	

(define (get-type typename)
	(get-type-prim typename _using))

(define (is-type? obj typename)
	(if (eq? (typeof obj) (get-type typename)) 
		#t 
		#f))

(define (new typename . args )
	(new-prim typename _using args))

;; obj args	
(define (call obj method . args)
	(call-prim obj method args))
	
;; sym args
(define (call-static classname method . args)
	(call-static-prim classname method args))
	
(define (get-property obj propname . rest )
	(get-property-prim obj propname rest))

(define (set-property obj propname val . rest)
	(set-property-prim obj propname val rest))

(define (get-field obj propname)
	(get-field-prim obj propname))
	
(define (set-field obj fieldname val)
	(set-field-prim obj fieldname val))
	
(define (copy-debug-info from-pair to-pair)
	(copy-debug-prim from-pair to-pair))

(define (get-enum enum_name enum_member)
	(call-static 'System.Enum 'Parse (get-type enum_name) enum_member))
				
(define (load-assembly assembly-name)
	(using assembly-name)
	(call-static 'System.Reflection.Assembly 'Load assembly-name))

(define (load-assembly-from assembly-name assembly-file)
	(using assembly-name)
	(call-static 'System.Reflection.Assembly 'LoadFrom assembly-file))

(define (list->array ls)
	(call ls 'ToArray))
	
(define (tostring obj)
	(tostring-prim obj))

(define (typeof obj)
	(string->symbol (tostring (typeof-prim obj))))

;; R5RS Functions

;;  Symbols
(define (string->symbol str) 
	(string->symbol-prim str))

(define (symbol->string sym)	
	(tostring sym))

;; Scheme Standard Library

(define (eq? obj1 obj2)
	(eq?-prim obj1 obj2))

(define (eqv? obj1 obj2)
	(eqv?-prim obj1 obj2))

;; Needs work (see TSPL)
;(define (equal? obj1 obj2) 
;		(if (pair? obj1) 
;			(if (equal? (car obj1) (car obj2)) 
;				(equal? (cdr obj1) (cdr obj2)) #f) 
;				(eqv? obj1 obj2)))

(define (boolean? x) 
	(if (eq? x #t)
		#t
		(if (eq? x #f)
			#t
			#f)))

(define (null? x)
	(eq? x '()))

(define (pair? obj) 
	(if (eq? (typeof obj) 'Tachy.Pair) #t #f))
	
(define (integer? obj)
	(if (eq? (typeof obj) 'System.Int32) #t #f))

(define (real? obj)
	(if (eq? (typeof obj) 'System.Single) #t #f))

(define (number? x)
	(if (real? x) 
		#t
		(if (integer? x)
			#t
			#f)))

;;(rational? obj)

(define (char? obj)
	(if (eq? (typeof obj) 'System.Char) #t #f))
	
(define (string? obj)
	(if (eq? (typeof obj) 'System.String) #t #f))

(define (vector? obj)
	(if (eq? (typeof obj) 'System.Object[]) #t #f))

(define (symbol? obj)
	(if (eq? (typeof obj) 'Tachy.Symbol) #t #f))

(define (procedure? obj)
	(if (eq? (typeof obj) 'Tachy.Closure) #t #f))
	
;; Lists & Pairs

(define ls '(1 2 3 4))

(define (set-car! a ls) 
	(set-car-prim ls a))

(define (set-cdr! a ls) 
	(set-cdr-prim ls a))

(define (cons obj ls)
	(cons-prim obj ls))
	
(define (debug-cons from-debug-ls obj ls)
	(copy-debug-info from-debug-ls (cons obj ls)))

(define (car ls)
	(car-prim ls))

(define (cdr ls)
	(cdr-prim ls))

(define (caar pair) 
	(car (car pair)))

(define (cadr pair)
	(car (cdr pair)))

;; this is just too cool
(define list 
	(lambda x x))
	
(define (list? obj)
	(pair? obj))
	
(define (length lst) 
	(if (null? lst)
		0
		(+ 1 (length (cdr lst)))))

;; (define (list-ref lst n)
;; (list-tail lst n)
(define (append ls1 ls2)
	(if (null? ls1)
		(if (null? ls2)
			'()
			(cons (car ls2) (append ls1 (cdr ls2))))
		(cons (car ls1) (append (cdr ls1) ls2))))
;; (define (reverse lst)
;; (memq obj lst)
;; (memv obj lst)
;; (member obj lst)
;; (assq obj alst)
;; (assv obj alst)
;; (assoc obj alst)

;; additional pair funcs
(define (first ls)
	(car ls))
	
(define (second ls)
	(car (cdr ls)))

(define (third ls)
	(car (cdr (cdr ls))))

(define (fourth ls)
	(car (cdr (cdr (cdr ls)))))

(define (not obj)	
	(if obj		
		#f 		
		#t))

;; Hashtable
(define (hash)	
	(new 'System.Collections.Hashtable))

(define (hash? obj) 
	(is-type? obj 'System.Collections.Hashtable))

(define (hash-length hsh) 
	(get-property hsh 'Count))

(define (hash-ref hsh key)
	(hash-ref-prim hsh key))

(define (hash-set! hsh key obj)
	(hash-set!_prim hsh key obj))

(define (hash-contains-key? hsh key) 
	(call hsh 'ContainsKey key))

(define (hash-contains-value? hsh value) 
	(call hsh 'ContainsValue value))

(define (map proc ls)
	(if (null? ls)
		'()
		(cons (proc (car ls)) (map proc (cdr ls)))))

;;changed cons to change-list to preserve debug info in parsed code lists 
(define (map-preserve proc ls)
	(if (null? ls)
		'()
		(debug-cons ls (proc (car ls)) (map proc (cdr ls)))))

;; Macros 
(define (macro-expand obj)
	(if (pair? obj)
			(if (eq? (car obj) 'quote)
					obj
					(if (hash-contains-key? _macros (car obj))
							(macro-expand (copy-debug-info obj ( (hash-ref _macros (car obj)) obj )))
							(map-preserve macro-expand obj)))
			obj))

(define (make-list-of-parameters code)
	(map first (second code)))

(define (make-list-of-operands code)
	(map second (second code)))
	
(define (make-list-of-body-items code)
	(cdr (cdr code)))

(define (make-lambda-expression params body-exprs)
	(cons 'lambda (cons params body-exprs)))
	
(define (let-trans code)
	(cons (make-lambda-expression 
				(make-list-of-parameters code) 
				(make-list-of-body-items code)) 
		 	 (make-list-of-operands code)))

(macro let let-trans)

(define (letrec-trans code)
	(append (list (append (append (append (list 'lambda)
						(list (make-list-of-parameters code)))
							(map make-set (second code)))
								(make-list-of-body-items code)))
				(map make-undefined (make-list-of-parameters code))))
			
(define (make-undefined ls) (list 'quote 'undefined))
(define (make-set ls) (append (list 'set!) ls))
(macro letrec letrec-trans)

(define (and-trans code)
	(list 'if (second code) (list 'if (third code) #t #f) #f))
(macro and and-trans)

(define (or-trans code)
	(list 'if (second code) #t (list 'if (third code) #t #f)))
(macro or or-trans)

(define (cond-trans code)
	(cond-helper (cdr code)))
(define (cond-helper code)
	(let ((curr (car code)))
		(if (= (length curr) 1)
			(list 'if (car curr) #t (cond-helper (cdr code)))
			(if (eq? (car curr) 'else)
				(cons 'begin (cdr curr))
				 (list 'if (car curr) (cons 'begin (cdr curr)) (cond-helper (cdr code)))))))
(macro cond cond-trans)

;; need to move to macros

;;(define (or obj1 obj2) 
;;	(if obj1 #t (if obj2 #t #f)))
;;
;;(define (and obj1 obj2) 
;;	(if obj1 (if obj2 #t #f) #f))


;; Numbers
;; (exact? num)
;; (inexact? num)

;; All From ObjTst
(define (numcompare num1 num2)
	(numcompare-prim num1 num2))

(define =
	(lambda ls
		(=helper (car ls) (cdr ls))))

(define (=helper test rest)
	(if (null? rest)
		#t
		(if (eqv? (numcompare test (car rest)) 0)
			(=helper test (cdr rest))
			#f)))

(define < 
	(lambda ls
		(<helper (car ls) (cdr ls))))
(define (lt num1 num2)	 
	(if (= (numcompare num1 num2) -1)
		#t
		#f))
(define (<helper curr rest)
	(if (null? rest)
		#t
		(if (lt curr (car rest))
			(<helper (car rest) (cdr rest))
			#f)))

(define >
	(lambda ls
		(>helper (car ls) (cdr ls))))
(define (gt num1 num2)	 
	(if (= (numcompare num1 num2) 1)	 	
		#t
		#f))
(define (>helper curr rest)
	(if (null? rest)
		#t
		(if (gt curr (car rest))
			(>helper (car rest) (cdr rest))
			#f)))

(define <=
	(lambda ls
		(<=helper (car ls) (cdr ls))))
(define (lte num1 num2)	 
	(if (or (< num1 num2) (= num1 num2))
		#t
		#f))
(define (<=helper curr rest)
	(if (null? rest)
		#t
		(if (lte curr (car rest))
			(<=helper (car rest) (cdr rest))
			#f)))

(define >=
	(lambda ls
		(>=helper (car ls) (cdr ls))))
(define (gte num1 num2)	 
	(if (or (> num1 num2) (= num1 num2))
		#t
		#f))
(define (>=helper curr rest)
	(if (null? rest)
		#t
		(if (gte curr (car rest))
			(>=helper (car rest) (cdr rest))
			#f)))

(define (addobj obj1 obj2)
	(addobj-prim obj1 obj2))

(define (subobj obj1 obj2)
	(subobj-prim obj1 obj2))

(define (mulobj obj1 obj2)
	(mulobj-prim obj1 obj2))

(define (divobj obj1 obj2)
	(divobj-prim obj1 obj2))

(define +
	(lambda ls
		(addlist 0 ls)))
(define (addlist curr ls)
	(if (null? ls)
		curr
		(addlist (addobj curr (car ls)) (cdr ls))))

(define - 
	(lambda ls
		(sublist (car ls) (cdr ls))))
(define (sublist curr ls)
	(if (null? ls)
		curr
		(sublist (subobj curr (car ls)) (cdr ls))))

(define *
	(lambda ls
		(mullist (car ls) (cdr ls))))
(define (mullist curr ls)
	(if (null? ls)
		curr
		(mullist (mulobj curr (car ls)) (cdr ls))))
	
(define /
	(lambda ls
		(divlist (car ls) (cdr ls))))
(define (divlist curr ls)
	(if (null? ls)
		curr
		(divlist (divobj curr (car ls)) (cdr ls))))
	
(define (zero? num)
	(= num 0))

(define (positive? real) 
	(> real 0))

(define (negative? real)
	(< real 0))

(define (even? int) 
	(if (= (modulo int 2) 0)
		#t
		#f))

(define (odd? int) 
	(not (even? int)))
	
;; (quotient int1 int2)
;; (remainder int1 int2) 
(define (modulo int1 int2) 
	(call-static 'Microsoft.VisualBasic.CompilerServices.ObjectType 'ModObj int1 int2))
;; (truncate real) 

(define (floor real)
 	(call-static 'System.Math 'Floor  real))
 	
 (define (ceiling real) 
 	(call-static 'System.Math 'Ceiling  real))
 
 (define (round real) 
 	(call-static 'System.Math 'Round  real))
 
 (define (abs real)
 	(call-static 'System.Math 'Abs  real))
 
(define (max-helper curr ls)
	(if (null? ls)
		curr
		(max-helper (call-static 'System.Math 'Max curr (car ls)) (cdr ls))))
(define max 
	(lambda ls
		(max-helper (car ls) (cdr ls))))

(define (min-helper curr ls)
	(if (null? ls)
		curr
		(min-helper (call-static 'System.Math 'Min curr (car ls)) (cdr ls))))
(define min 
	(lambda ls
		(min-helper (car ls) (cdr ls))))

;; (gcd int ...) 
;; (lcm int ...) 
(define (expt num1 num2)
	(call-static 'System.Math 'Pow '(System.Double System.Double) num1 num2))
;; (exact->inexact num) 
;; (inexact->exact num) 
;; (rationalize real1 real2)
;; (numerator rat)
;; (denominator rat) 
;; (real-part num) 
;; (imag-part num) 
;; (make-rectangular real1 real2)
;; (make-polar real1 real2) 
;; (angle num) 
;; (magnitude num) 

(define (sqrt num) 
	(call-static 'System.Math 'Sqrt   num))
 (define (exp num)  
 	(call-static 'System.Math 'Exp  num))
 (define (log num)
 	(call-static 'System.Math 'Log  num))
 (define (sin num)
 	(call-static 'System.Math 'Sin  num))
 (define (cos num)
 	(call-static 'System.Math 'Cos  num))	
 (define (tan num)
 	(call-static 'System.Math 'Tan  num))	
 (define (asin num)
 	(call-static 'System.Math 'Asin  num))	
 (define (acos num)
 	(call-static 'System.Math 'Acos  num))	
 (define (atan num)
 	(call-static 'System.Math 'Atan  num))	

;; check to see if has a "." and if so make a double, otherwise int32

 (define (string->number str) 
 	(call-static 'System.Convert 'ToDouble str))

;; (string->number string radix) 

(define (number->string num) 
	(tostring num))
	
;; (number->string num radix) 

;; Characters
;; (char=? char1 char2 char3 ...)
;; (char<? char1 char2 char3 ...) 
;; (char>? char1 char2 char3 ...) 
;; (char<=? char1 char2 char3 ...)
;; (char>=? char1 char2 char3 ...)
;; (char-ci=? char1 char2 char3 ...)
;; (char-ci<? char1 char2 char3 ...) 
;; (char-ci>? char1 char2 char3 ...) 
;; (char-ci<=? char1 char2 char3 ...)
;; (char-ci>=? char1 char2 char3 ...)
;; (char-alphabetic? char) 
;; (char-numeric? char)
;; (char-lower-case? letter)
;; (char-upper-case? letter) 
;; (char-whitespace? char) 
;; (char-upcase char) 
;; (char-downcase char) 
;; (char->integer char) 
;; (integer->char int) 

;;  Strings (multi versions on all comps later when better handle on how to do it cleanly

(define (strcompare str1 str2 case-sensitive)
	(strcompare-prim str1 str2 case-sensitive))

(define (string=? string1 string2) 
	(if (= (strcompare string1 string2 #f) 0)
		#t
		#f))

(define (string<? string1 string2) 
	(if (< (strcompare string1 string2 #f) 0)
		#t
		#f))

(define (string>? string1 string2) 
	(if (< (strcompare string1 string2 #f) 0)
		#t
		#f))

(define (string<=? string1 string2) 
	(if (<= (strcompare string1 string2 #f) 0)
		#t
		#f))

(define (string>=? string1 string2) 
	(if (<= (strcompare string1 string2 #f) 0)
		#t
		#f))

(define (string-ci=? string1 string2) 
	(if (= (strcompare  string1 string2 #t) 0)
		#t
		#f))

(define (string-ci<? string1 string2) 
	(if (< (strcompare  string1 string2 #t) 0)
		#t
		#f))

(define (string-ci>? string1 string2) 
	(if (< (strcompare  string1 string2 #t) 0)
		#t
		#f))

(define (string-ci<=? string1 string2) 
	(if (<= (strcompare  string1 string2 #t) 0)
		#t
		#f))

(define (string-ci>=? string1 string2) 
	(if (<= (strcompare  string1 string2 #t) 0)
		#t
		#f))

;;(define (string char) 
;;	(new-string-prim char))
	
;;(define (make-string n . rest)
;;	(if (null? rest)
;;		(new-string-prim char 1)
;;		(new 'System.String #\? n)
;;		(new 'System.String (car rest) n)))
		
(define (string-length str)
	(get-property str 'Length))
	
(define (string-ref str n) 
	(get-property str 'Chars n))
	
;;(string-set! string n char) 

(define (string-copy str) 
	(call-static 'System.String 'Copy str))

(define (append-helper ls)
	(if (null? ls)
		""
		(call-static 'System.String 'Concat (car ls) (append-helper (cdr ls)))))

(define string-append 
	(lambda ls
		(append-helper ls)))

(define (substring str start end) 
	(call str 'Substring start (- end start)))

; strings are immutable in clr, so state change likely remain unsupported
;(define (string-fill! string char)  

;(define (string->list string) 
;(define (list->string list) 

;;  Vectors (Arrays)

;; (vector obj ...) 

;; (vector-fill! vector obj) 
;; (vector->list vector) 


(define (make-vector n . typename)
	(vector-prim n typename))

(define (vector-length vec)
	(vector-length-prim vec))
	
(define (vector-ref vector n)
	(vector-ref-prim vector n))
	
(define (vector-set! vector n obj)
	(vector-set!-prim vector n obj))

;;  IO
;; (input-port? obj)
;; (current-input-port) 
;; (open-input-file filename) 
;; (close-input-port input-port) 
;; (call-with-input-file filename proc) 
;; (with-input-from-file filename thunk) 
;; (read) 
;; (read input-port) 
;; (read-char) 
;; (read-char input-port) 
;; (peek-char) 
;; (peek-char input-port) 
;; (eof-object? obj) 
;; (char-ready?) 
;; (char-ready? input-port) 
;; (output-port? obj) 
;; (current-output-port) 
;; (open-output-file filename) 
;; (close-output-port output-port) 
;; (call-with-output-file filename proc) 
;; (with-output-to-file filename thunk) 

(define (write obj) 
	(call-static 'Tachy.Util 'Dump obj))

;; (write obj output-port) 

;; (display obj) 

(define (display obj)
	(call-static 'System.Console 'WriteLine (tostring obj)))

;; (display obj output-port) 

;; (write-char char) 
;; (write-char char output-port) 

;; (newline) 
;; (newline output-port) 

;; (load filename) 
;; (transcript-on filename) 
;; (transcript-off) 

