using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Tachy;

namespace TachyExtension
{
	/// <summary>
	/// Summary description for CallstackWindow.
	/// </summary>
	public class ListViewWindow : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ListView ListView1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ListViewWindow()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ListView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// ListView1
			// 
			this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																												this.columnHeader1,
																												this.columnHeader2});
			this.ListView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ListView1.FullRowSelect = true;
			this.ListView1.HideSelection = false;
			this.ListView1.Location = new System.Drawing.Point(0, 0);
			this.ListView1.MultiSelect = false;
			this.ListView1.Name = "ListView1";
			this.ListView1.Size = new System.Drawing.Size(150, 150);
			this.ListView1.TabIndex = 0;
			this.ListView1.View = System.Windows.Forms.View.Details;
			this.ListView1.ItemActivate += new System.EventHandler(this.ListView1_ItemActivate);
			this.ListView1.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "";
			this.columnHeader1.Width = 19;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Name";
			this.columnHeader2.Width = 160;
			// 
			// ListViewWindow
			// 
			this.Controls.Add(this.ListView1);
			this.Name = "ListViewWindow";
			this.ResumeLayout(false);

		}
		#endregion

		private void ListView1_ItemActivate(object sender, System.EventArgs e)
		{
			if (((string)ListView1.Tag) == "locals")
				DebugInfo.LocalsListView_ItemActivate(sender, e);
			else if (((string)ListView1.Tag) == "call stack")
				DebugInfo.CallstackListView_ItemActivate(sender, e);
		}

		private void ListView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ListView1_ItemActivate(sender, e);
		}
	}
}
