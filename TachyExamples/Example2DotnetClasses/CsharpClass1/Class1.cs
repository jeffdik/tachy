using System;
using System.ComponentModel; //used by the TypeConverter below

namespace CsharpClass1
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>

	[TypeConverter(typeof(ExpandableObjectConverter))] 
	//This TypeConverter attribute is required to properly view objects of this class in the property editor while debugging Tachy code
	public class Class1
	{
		int x;
		int y;
		public Class1(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public int X
		{
			get {return x;}
			set {x = value;}
		}

		public int Y
		{
			get {return y;}
			set {y = value;}
		}

		public int Product()
		{
			return x * y;
		}

		public int AddX(int a)
		{
			return x + a;
		}
	}
}
