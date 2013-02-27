//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
namespace Microsoft.Practices.WebClientFactory.CustomWizardPages
{
    partial class SolutionPropertiesPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SolutionPropertiesPage));
            this._folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this._supportingLibrariesTextBox = new System.Windows.Forms.TextBox();
            this._browseSupportingLibraryButton = new System.Windows.Forms.Button();
            this._rootNamespaceTextbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this._supportingLibrariesList = new System.Windows.Forms.ListView();
            this.label5 = new System.Windows.Forms.Label();
            this._showDocumentation = new System.Windows.Forms.CheckBox();
            this.assembliesTreeView = new System.Windows.Forms.TreeView();
            this._iconList = new System.Windows.Forms.ImageList(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._modelValidationProvider = new Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            resources.ApplyResources(this.infoPanel, "infoPanel");
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // _supportingLibrariesTextBox
            // 
            resources.ApplyResources(this._supportingLibrariesTextBox, "_supportingLibrariesTextBox");
            this._supportingLibrariesTextBox.Name = "_supportingLibrariesTextBox";
            this._modelValidationProvider.SetPerformValidation(this._supportingLibrariesTextBox, true);
            this._modelValidationProvider.SetSourcePropertyName(this._supportingLibrariesTextBox, "SupportLibrariesPath");
            // 
            // _browseSupportingLibraryButton
            // 
            resources.ApplyResources(this._browseSupportingLibraryButton, "_browseSupportingLibraryButton");
            this._browseSupportingLibraryButton.Name = "_browseSupportingLibraryButton";
            this._browseSupportingLibraryButton.UseVisualStyleBackColor = true;
            this._browseSupportingLibraryButton.Click += new System.EventHandler(this.OnBrowseSupportingLibraryClick);
            // 
            // _rootNamespaceTextbox
            // 
            resources.ApplyResources(this._rootNamespaceTextbox, "_rootNamespaceTextbox");
            this._rootNamespaceTextbox.Name = "_rootNamespaceTextbox";
            this._modelValidationProvider.SetPerformValidation(this._rootNamespaceTextbox, true);
            this._modelValidationProvider.SetSourcePropertyName(this._rootNamespaceTextbox, "RootNamespace");
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // _supportingLibrariesList
            // 
            resources.ApplyResources(this._supportingLibrariesList, "_supportingLibrariesList");
            this._supportingLibrariesList.ForeColor = System.Drawing.SystemColors.GrayText;
            this._supportingLibrariesList.Name = "_supportingLibrariesList";
            this._supportingLibrariesList.TileSize = new System.Drawing.Size(370, 18);
            this._supportingLibrariesList.UseCompatibleStateImageBehavior = false;
            this._supportingLibrariesList.View = System.Windows.Forms.View.Tile;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // _showDocumentation
            // 
            resources.ApplyResources(this._showDocumentation, "_showDocumentation");
            this._showDocumentation.Name = "_showDocumentation";
            this._showDocumentation.UseVisualStyleBackColor = true;
            // 
            // assembliesTreeView
            // 
            resources.ApplyResources(this.assembliesTreeView, "assembliesTreeView");
            this.assembliesTreeView.ImageList = this._iconList;
            this.assembliesTreeView.Name = "assembliesTreeView";
            // 
            // _iconList
            // 
            this._iconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_iconList.ImageStream")));
            this._iconList.TransparentColor = System.Drawing.Color.White;
            this._iconList.Images.SetKeyName(0, "WebProjectIcon");
            this._iconList.Images.SetKeyName(1, "CSharpProjectIcon");
            this._iconList.Images.SetKeyName(2, "SolutionFolderIcon");
            this._iconList.Images.SetKeyName(3, "SolutionIcon");
            this._iconList.Images.SetKeyName(4, "WebFormIcon");
            this._iconList.Images.SetKeyName(5, "GenericProjectItemIcon");
            this._iconList.Images.SetKeyName(6, "CSharpItemIcon");
            this._iconList.Images.SetKeyName(7, "FolderIcon");
            this._iconList.Images.SetKeyName(8, "WebConfigIcon");
            this._iconList.Images.SetKeyName(9, "VisualBasicProjectIcon");
            this._iconList.Images.SetKeyName(10, "VisualBasicItemIcon");
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.assembliesTreeView);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // _modelValidationProvider
            // 
            this._modelValidationProvider.ErrorProvider = this._errorProvider;
            this._modelValidationProvider.RulesetName = "";
            this._modelValidationProvider.SourceTypeName = "Microsoft.Practices.WebClientFactory.CustomWizardPages.SolutionPropertiesModel, M" +
                "icrosoft.Practices.WebClientFactory.GuidancePackage";
            // 
            // SolutionPropertiesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._showDocumentation);
            this.Controls.Add(this._supportingLibrariesList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._supportingLibrariesTextBox);
            this.Controls.Add(this._browseSupportingLibraryButton);
            this.Controls.Add(this._rootNamespaceTextbox);
            this.Controls.Add(this.label3);
            this.Name = "SolutionPropertiesPage";
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this._rootNamespaceTextbox, 0);
            this.Controls.SetChildIndex(this._browseSupportingLibraryButton, 0);
            this.Controls.SetChildIndex(this.infoPanel, 0);
            this.Controls.SetChildIndex(this._supportingLibrariesTextBox, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this._supportingLibrariesList, 0);
            this.Controls.SetChildIndex(this._showDocumentation, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _supportingLibrariesTextBox;
        private System.Windows.Forms.Button _browseSupportingLibraryButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _rootNamespaceTextbox;
        private System.Windows.Forms.ListView _supportingLibrariesList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox _showDocumentation;
        private System.Windows.Forms.TreeView assembliesTreeView;
        private System.Windows.Forms.ImageList _iconList;
        private System.Windows.Forms.GroupBox groupBox1;
        private Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider _modelValidationProvider;
    }
}
