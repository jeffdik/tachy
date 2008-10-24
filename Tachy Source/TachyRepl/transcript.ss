(+ 12 34)
(+ 234 234)
(+ (* 12 2) (- 3 4))
(- (* 12 2) (- 3 4))
(using "TopSupport.Core")
(call-static 'Common.StringUtil.FirstElement "Test Test Test" " ")
(call-static 'Common.StringUtil "FirstElement" "Test Test Test" " ")
(call-static 'Common.StringUtil "GetNrOfElements" "Test Test Test" " ")
(call-static 'Commcon.StringUtil "GetNrOfElements" "Test Test Test" " ")
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " ")
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" ("Test Test Test" " "))
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" '("Test Test Test" " "))
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " ")
(call-static 'TopSupport.Core.Common.StringUtil 'NewLine)
(call-static 'System.Math 'Floor 2)
(call-static 'System.Math 'Floor 2.3)
(call-static 'System.Math 'Floor 2.3)
(call-static 'TopSupport.Core.Common.StringUtil 'NewLine)
(using "TopSupport.Core")
(load-assembly "TopSupport.Core")
(load-assembly "TopSupport.Core")
(load-assembly "TopSupport.Core")
(using "TopSupport.Core")
(call-static 'TopSupport.Core.Common.StringUtil 'NewLine)
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " ")
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " ")
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " ")
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " ")
(call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " ")
(setq a (call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " "))
(set a 1)
(set 'a 1)
(let a 1)
(let 'a 1)
(+ 2 (call-static 'TopSupport.Core.Common.StringUtil "GetNrOfElements" "Test Test Test" " "))
(load-assembly "TopSupport.Expressions")
(using "TopSupport.Expressions")
(set! a 1)
(a)
a
(set! lit1 (new 'TopSupport.Expressions.IntLiteralValue 1))
lit1
(set! lit2 (new 'Expressions.IntLiteralValue 1))
(set! lit2 (new 'IntLiteralValue 2))
lit2
(set! intbin1 (new 'IntBinaryIntPlusInt lit1 lit2))
intbin1
(get-property intbin1 'Value)
(get-property intbin1 'LeftOperand)
(get-property 'StringUtil 'NewLine)
(get-property StringUtil 'NewLine)
(StringUtil.NewLine)
intbin1.Value
(intBin1)
intBin1
intbin1
(intbin1 'Value)
intBin1
intbin1
a
A
intbin1
(get-property intbin1 'LeftOperand)
intbin1.Value
intbin1.Value
intbin1
(load-assembly "TopSupport.Expressions")
(using "TopSupport.Expressions")
(set! lit1 (new IntLiteralValue 1))
(set! lit1 (new 'IntLiteralValue 1))
(set! lit1 (new 'TopSupport.Expressions.IntLiteralValue 1))
(set! lit1 (new 'IntLiteralValue 1))
(set! lit1 (new 'IntLiteralValue 1))
(set! lit1 (new "IntLiteralValue" 1))
(set! a 1)
(set! lit1 (new 'IntLiteralValue 1))
(set! lit1 (new 'IntLiteralValue 1))
lit1
(load-assembly "MyDll\TopSupport.Expressions.dll")
(sET! A 1)
(set! a 1)
a
a
a
(+ 1 2)
(+ 1 2)
(+ 1 2)
(+ 1 2)
(+ 1 2)
(+ 1 2)
(+ (- 2 1) 3)
(+ (- 2 1) 3)
(set! a 1)
(+ a 1)
(a.x 1 2)
(a.x 1 2)
(a:x 1 2)
(a:x b:1 2)
(a.x 1 2)
(a.x a.y)
(set! a (+ 1 2).fff)
a
(set! a (+ 1 2).fff)
(set! a (+ 1 2).fff)
(new 'IntLiteralValue 1)
(new 'IntLiteralValue 1)
(new 'IntLiteralValue 1)
(set! a (new 'IntLiteralValue 1))
a
exit















































(+ (- 2 1).test)
(set! a (+ 1 2).fff)
(set! a (+ 1 2).fff)
(set! a (+ 1 2).ffff)
(set! a (+ 1 2).fff)
(set! a 1)
a
(get-property-prim a value)
(get-property-prim a 'value)
(get-property a 'value)
(set! a 2)
a
(get_property a 'value)
(get-property a 'value)
(set a! 1)
(set! a 1)
a
(set! a (+ 1 2).fff)
(get_property a 1)
(get_property a 'value)
(set! c a.value)
(new 'IntLiteralValue 1)
(set! a (new 'IntLiteralValue 1))
(get-property a 'value)
(get-property a 'Value)
a
(get-property a 'LeftOperand)
(get-property a 'IValue)
(get-property a 'DValue)
(get-property a 'Value)
a.Value
a.Value
a.Value
a.Value
(get-property a 'Value)
(get-property a 'Value)
(get-property a 'Value)
a.Value
a.Value
(get-property a 'Value)
(get-property a 'Value)
a.Value
(get-property a 'Value)
a.Value
(get-property a 'Value)
(get-property a 'Value)
a.Value
a.Value
(get-property a 'Value)
(get-property a 'Value)
a.Value
(get-property a 'Value)
(get-property a 'Value)
(get-property a 'Value)
(get-property a 'Value)
(get-property a 'Value)
(get-property a 'Value)
a.Value
a.Value
a.Value
a.Value
(+ 1 a.Value)
(+ 41 a.Value)
(set! b "a.Value")
(eval b)
(apply b)
(newline)
(+ 1 2)
(exit)
(set! b '(+ 1 2))
b
(call-static 'Eval b)
a.Value
(new 'IntLiteralValue 3).Value
(set! a (new 'IntLiteralValue 3).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b 1)
(set! b (new 'IntLiteralValue 2).Value)
a.Value
(set b (new 'IntLiteralValue 2).Value)
(set b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
b
(set cb (new 'IntLiteralValue 2).Value)
(set cb (new 'IntLiteralValue 2).Value)
b
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (new 'IntLiteralValue 2).Value)
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (new 'IntLiteral 2).Value)
(set! b (new 'IntLiteral 2).Value)
(set! b (new 'IntLiteral 2).Value)
(set! b (new 'IntLiteral 2).Value)
(set! b (new 'IntLiteral 2).Value)
(set! b (new 'IntLiteral 2).Value)
(set! b (get-property (new 'IntLiteral 2) 'Value)
(set! b (get-property (new 'IntLiteral 2) 'Value))
(set! b (new 'IntLiteralValye 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (new 'IntLiteralValue 2).Value)
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b (get-property (new 'IntLiteralValue 2) 'Value))
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 2).Value)
(+ 23 (new 'IntLiteralValue 2).Value)
(+ 23 (new 'IntLiteralValue 2).Value 23)
(a.SpecialValue)
a
(call-static 'System.Math 'Asin 2)
(call-static 'System.Math 'Asin 0)
(call-static 'System.Math 'Asin 1)
(call-static 'Asin 1)
(call-static 'System.Math 'Asin 1)
(call 'Asin 1)
(call-static 'Asin 1)
(call-static 'Math.Asin 1)
(call 'Math.Asin 1)
(+ aLit1 2)
(+ aLit1.Value 2)
aLit1.Value
(+ (new 'IntLiteralValue 3).Value 6)
(+ (set! a (new 'IntLiteralValue 3).Value 6))
(+ (set! a (new 'IntLiteralValue 3).Value) 6)
(set! a (new 'IntLiteralValue 3).Value)
(+ (set! a (new 'IntLiteralValue 3)).Value 6)
(+ (set! a (new 'IntLiteralValue 3)).Value 6)
(+ System.Math.PI 9)
(+ Math.PI 9)
(+ Math.PI 9
(set! b (new 'IntBinaryIntPlusInt aLit1 aLit1))
b
(+ 1 b.Value)
(+ 1 b.LeftOperand)
(+ 1 b.LeftOperand.Value)
(+ 1 b.LeftOperand.Value)
(+ 1 b.LeftOperand.Value.ToString)
(a.b 2 3)
(get-property 'System.Math 'PI)
(get-property 'Math 'PI)
(set a.Value 8)
aLit1
(set-property aLit1 'Value 4)
aLit1
(set-property aLit1 'Value 3)
(set! aLit1.Value 5)
aLit1
(set! aLit1.Value 15)
aLit1
(set! aLit1.Value (+ aLit1.Value aLit1.Value))
aLit1
(set! b (new 'IntBinaryIntPlusInt aLit1 aLit1))
b.LeftOperand.Value
b
(set! aLit1.Value 10)
b
(set! b.LeftOperand.Value 55)
b
(call b 'ToString)
(call b.LeftOperand.Value 'ToString)
(set! c (new 'String))
(set! c (new 'String ""))
aLit1
(call aLit1 'TestMethod 3 4)
(set! b (new 'IntBinaryIntPlusInt aLit1 aLit1))
(call b.LeftOperand 'TestMethod 2 88)
(call (new 'IntLiteralValue 7) 'TestMethod 88 77)
(call (new 'IntBinaryIntPlusInt aLit1 aLit1).LeftOperand 'TestMethod 123 3)
(call (new 'IntBinaryIntPlusInt aLit1 aLit1).LeftOperand 'TestMethod 123 3)
(call (new 'IntBinaryIntPlusInt aLit1 aLit1).LeftOperand 'TestMethod 123 3)
(call (new 'IntBinaryIntPlusInt aLit1 aLit1).LeftOperand 'TestMethod 1222222223 3)
(call (new 'IntBinaryIntPlusInt aLit1 aLit1).LeftOperand 'TestMethod 1222222223 345345345345345345)
((lambda x (+ 1 x) 2)
((lambda x (+ 1 x)) 2)
(lambda (x) (+ x 2))
((lambda (x) (+ x 2)) 5)
((lambda () (+ 3 2)) ())
((lambda () (+ 3 2)) )
(set! a '(+ 4 55))
a
((lambda () a) )
((lambda () 'a) )
(set! a '(+ 4 55))
(set! a '(+ 4 55))
(set! a '(+ 1 2))
(macro-expand a)
(macro-expand 'a)
aLit1
aLit1.Value
(set! b (new 'IntLiteralValue 2))
(set! b (new 'IntLiteralValue 2).Value)
(set! b (new 'IntLiteralValue 3).Value)
b
(set! (new 'IntLiteralValue 3).Value 35)
(set! b (new 'IntLiteralValue 3).Value)
aLit1.Value
aBin1
aBin1.LeftOperand.Value
(+ aBin1.LeftOperand.Value 2)
(+ (new 'IntBinaryIntPlusInt aBin1 aLit1).Value 2)
(+ (new 'IntBinaryIntPlusInt aBin1 aLit1).Value 2)
(+ (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.Value 2)
(+ (new 'IntBinaryIntPlusInt aBin1 aLit1).Value 2)
(new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand
(+ (new 'IntBinaryIntPlusInt aBin1 aLit1).Value 2)
aBin1.LeftOperand
aBin1.LeftOperand.Value
(+ (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand.Value 2)
(+ (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand.Value 2)
(+ (new 'IntBinaryIntPlusInt aBin1 aLit1).Value 2)
(new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand
(new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand)
a
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
a
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand.Value)
a
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
a
(set! aBin1.Value 10)
(set! aLit1.Value 10)
a
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand)
a
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a.Value 5)
(set! aLit1.Value 5)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a.Value 12)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
a
a.LeftOperand
a.LeftOperand.LeftOperand
(set! a (new 'IntBinaryIntPlusInt aBin1 aLit1).LeftOperand.LeftOperand)
aLit1
aLit1
aLit1.Value
aLit1.Value
(call aLit1 'ToString)
aLit1.Value
aLit1.Value
aLit1.Value
aLit1
aLit1
aLit2
exit
test

aLit1



(aLit1)
aLit1
aLit1
aLit1
aBin1
(set! aLit1 (new 'IntLiteralValue 13))
aBin1
aBin1.Value
aBin1.LeftOperand
aBin1.LeftOperand.Value
(set! aBin1 (new 'IntBinaryIntPlusInt aLit1 aLit2))
aBin1
(set! aLit1.Value 2134234)
aBin1
