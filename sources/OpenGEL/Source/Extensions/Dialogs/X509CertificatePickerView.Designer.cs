namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
    /// <summary>
    /// The X509 certificates picker UI.
    /// </summary>
	partial class X509CertificatePickerView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(X509CertificatePickerView));
            this.cbStore = new System.Windows.Forms.ComboBox();
            this.lbStore = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.lbLoc = new System.Windows.Forms.Label();
            this.bAccept = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.lbCertificate = new System.Windows.Forms.Label();
            this.txCertificate = new System.Windows.Forms.TextBox();
            this.bCertSelection = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // cbStore
            // 
            this.cbStore.FormattingEnabled = true;
            resources.ApplyResources(this.cbStore, "cbStore");
            this.cbStore.Name = "cbStore";
            this.toolTip1.SetToolTip(this.cbStore, resources.GetString("cbStore.ToolTip"));
            this.cbStore.SelectedIndexChanged += new System.EventHandler(this.cbStore_SelectedIndexChanged);
            // 
            // lbStore
            // 
            resources.ApplyResources(this.lbStore, "lbStore");
            this.lbStore.Name = "lbStore";
            // 
            // cbLocation
            // 
            this.cbLocation.FormattingEnabled = true;
            resources.ApplyResources(this.cbLocation, "cbLocation");
            this.cbLocation.Name = "cbLocation";
            this.toolTip1.SetToolTip(this.cbLocation, resources.GetString("cbLocation.ToolTip"));
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
            // 
            // lbLoc
            // 
            resources.ApplyResources(this.lbLoc, "lbLoc");
            this.lbLoc.Name = "lbLoc";
            // 
            // bAccept
            // 
            resources.ApplyResources(this.bAccept, "bAccept");
            this.bAccept.Name = "bAccept";
            this.bAccept.UseVisualStyleBackColor = true;
            this.bAccept.Click += new System.EventHandler(this.bAccept_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.bCancel, "bCancel");
            this.bCancel.Name = "bCancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // lbCertificate
            // 
            resources.ApplyResources(this.lbCertificate, "lbCertificate");
            this.lbCertificate.Name = "lbCertificate";
            // 
            // txCertificate
            // 
            resources.ApplyResources(this.txCertificate, "txCertificate");
            this.txCertificate.Name = "txCertificate";
            this.txCertificate.ReadOnly = true;
            this.toolTip1.SetToolTip(this.txCertificate, resources.GetString("txCertificate.ToolTip"));
            this.txCertificate.TextChanged += new System.EventHandler(this.txCertificate_OnTextChanged);
            // 
            // bCertSelection
            // 
            resources.ApplyResources(this.bCertSelection, "bCertSelection");
            this.bCertSelection.Name = "bCertSelection";
            this.bCertSelection.UseVisualStyleBackColor = true;
            this.bCertSelection.Click += new System.EventHandler(this.bCertSelection_Click);
            // 
            // X509CertificatePickerView
            // 
            this.AcceptButton = this.bAccept;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.Controls.Add(this.bCertSelection);
            this.Controls.Add(this.txCertificate);
            this.Controls.Add(this.lbCertificate);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bAccept);
            this.Controls.Add(this.cbStore);
            this.Controls.Add(this.lbStore);
            this.Controls.Add(this.cbLocation);
            this.Controls.Add(this.lbLoc);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "X509CertificatePickerView";
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cbStore;
		private System.Windows.Forms.Label lbStore;
		private System.Windows.Forms.ComboBox cbLocation;
		private System.Windows.Forms.Label lbLoc;
		private System.Windows.Forms.Button bAccept;
		private System.Windows.Forms.Button bCancel;
		private System.Windows.Forms.Label lbCertificate;
		private System.Windows.Forms.TextBox txCertificate;
		private System.Windows.Forms.Button bCertSelection;
        private System.Windows.Forms.ToolTip toolTip1;
	}
}