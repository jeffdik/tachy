using System;
using CsharpClass1;
using System.ComponentModel; //used by the TypeConverter below

namespace CsharpClass2
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	
	[TypeConverter(typeof(ExpandableObjectConverter))] 
	//This TypeConverter attribute is required to properly view objects of this class in the property editor while debugging Tachy code
	public class Class2
	{
		private Class1 a;
		private Class1 b;

		public Class2(Class1 a, Class1 b)
		{
			this.a = a;
			this.b = b;
		}

		public Class1 A
		{
			get {return a;}
			set {a = value;}
		}

		public Class1 B
		{
			get {return b;}
			set {b = value;}
		}

		public int SumX()
		{
			return a.X + b.X;
		}
	}
}
