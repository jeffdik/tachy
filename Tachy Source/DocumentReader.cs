using System;
using System.IO;
using System.Windows.Forms;
using EnvDTE;

namespace Tachy
{
	/// <summary>
	/// Summary description for DocumentReader.
	/// </summary>
	public class DocumentReader : TextReader	
	{
		internal Document document;
		internal int Line = 0;
		internal int Col = 0;
		
		private string strDocument;
		private int documentIndex;
		private int tabSize;

		public DocumentReader(Document document)
		{
			this.document = document;
			TextDocument textDocument = (TextDocument)document.Object("TextDocument");
			EditPoint editPoint = textDocument.StartPoint.CreateEditPoint();
			editPoint.StartOfDocument();
			strDocument = editPoint.GetText(textDocument.EndPoint);
			documentIndex = -1;
			SetTabSize();
		}

		private void SetTabSize()
		{
			// TODO: determine VS plain text tab size
			tabSize = 4;
		}


/*		public int Position
		{
			get {return editPoint.AbsoluteCharOffset;}
			set {editPoint.CharRight(value - editPoint.AbsoluteCharOffset);}
		}
*/

		public override int Read()
		{
			if (documentIndex >= strDocument.Length - 1)
				return -1;
			else
			{
				if ((documentIndex == -1) || (strDocument[documentIndex] == ((char) 10))) //eol
				{
					Line++;
					Col = 1;
				}
				else if (strDocument[documentIndex] == ((char) 9)) //tab
					Col = (((int) ((Col + tabSize) / tabSize)) * tabSize) + 1;
				else
					Col++;

				documentIndex++;
				return strDocument[documentIndex];
			}
		}

		public override string ReadLine()
		{
			if (documentIndex >= strDocument.Length - 1)
				return "";
			
			documentIndex++;
			string result;
			int eolIndex = strDocument.IndexOf((char) 13, documentIndex);
			if (eolIndex > -1)
			{
				result = strDocument.Substring(documentIndex, eolIndex - documentIndex + 1);
				documentIndex = eolIndex;
			}
			else
			{
				result = strDocument.Substring(documentIndex);
				documentIndex = strDocument.Length - 1;
			}

			return result;
		}
		
		/*
		private string ReadBufferLine()
		{
			Line++;
			strLine = editPoint.GetLines(Line, Line + 1);
			Col = 1;

			return strLine;

			//old ReadLine()
			string result = editPoint.GetLines(editPoint.Line, editPoint.Line + 1);
			
			editPoint.EndOfLine();
			if (!editPoint.AtEndOfDocument)
				Read();

			//Clipboard.SetDataObject(result, true);
			return result;
			
		}
	*/
		public override int Peek()
		{
			if (documentIndex >= strDocument.Length - 1)
				return -1;
			else
				return strDocument[documentIndex + 1];
		}


	}
}
