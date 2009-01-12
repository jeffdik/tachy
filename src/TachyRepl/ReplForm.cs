using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Tachy
{
	/// <summary>
	/// Summary description for ReplForm.
	/// </summary>
	public class ReplForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox replInputBox;
		private System.Windows.Forms.TextBox replOutputBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		A_Program  prog = new A_Program();

		public ReplForm()
		{
			InitializeComponent();
			prog.LoadEmbededInitTachy();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.replInputBox = new System.Windows.Forms.TextBox();
			this.replOutputBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// replInputBox
			// 
			this.replInputBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.replInputBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.replInputBox.Location = new System.Drawing.Point(0, 0);
			this.replInputBox.Multiline = true;
			this.replInputBox.Name = "replInputBox";
			this.replInputBox.Size = new System.Drawing.Size(520, 466);
			this.replInputBox.TabIndex = 0;
			this.replInputBox.Text = "";
			this.replInputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.replInputBox_KeyDown);
			this.replInputBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.replInputBox_KeyPress);
			// 
			// replOutputBox
			// 
			this.replOutputBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.replOutputBox.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.replOutputBox.Location = new System.Drawing.Point(0, 354);
			this.replOutputBox.Multiline = true;
			this.replOutputBox.Name = "replOutputBox";
			this.replOutputBox.ReadOnly = true;
			this.replOutputBox.Size = new System.Drawing.Size(520, 112);
			this.replOutputBox.TabIndex = 1;
			this.replOutputBox.Text = "";
			// 
			// ReplForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(520, 466);
			this.Controls.Add(this.replOutputBox);
			this.Controls.Add(this.replInputBox);
			this.Name = "ReplForm";
			this.Text = "ReplForm";
			this.ResumeLayout(false);

		}
		#endregion

		private void replInputBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		}

		private void replInputBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (Convert.ToInt32(e.KeyChar) == 10)
			{
				StreamWriter str = new StreamWriter("..\\..\\transcript.ss", true);
				try 
				{
					String val = this.replInputBox.Text;
					str.WriteLine(val);
					object result = prog.Eval(new StringReader(val));
					this.replOutputBox.Text += result + "\r\n";
					this.replInputBox.Clear();
				} 
				catch (Exception ex) 
				{
					MessageBox.Show("Tachy Error: " + ex.ToString());
				}
				str.Close();
			}
		}
	}
}
