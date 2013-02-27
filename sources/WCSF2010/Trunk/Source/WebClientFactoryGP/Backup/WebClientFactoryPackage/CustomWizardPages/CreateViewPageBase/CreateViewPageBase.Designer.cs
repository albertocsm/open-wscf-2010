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
    partial class CreateViewPageBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateViewPageBase));
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.webSiteTreeView = new System.Windows.Forms.TreeView();
            this._iconList = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this._foldersComboBox = new System.Windows.Forms.ComboBox();
            this._foldersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this._webSiteProjectsComboBox = new System.Windows.Forms.ComboBox();
            this._webProjectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this._viewNameTextBox = new System.Windows.Forms.TextBox();
            this._viewNameLabel = new System.Windows.Forms.Label();
            this.assembliesTreeView = new System.Windows.Forms.TreeView();
            this.label6 = new System.Windows.Forms.Label();
            this._moduleProjectsComboBox = new System.Windows.Forms.ComboBox();
            this._moduleProjectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._showDocumentation = new System.Windows.Forms.CheckBox();
            this._modelValidationProvider = new Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._foldersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._webProjectsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._moduleProjectsBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // webSiteTreeView
            // 
            resources.ApplyResources(this.webSiteTreeView, "webSiteTreeView");
            this.webSiteTreeView.ImageList = this._iconList;
            this.webSiteTreeView.Name = "webSiteTreeView";
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
            this._iconList.Images.SetKeyName(9, "DesignerProjectItemIcon");
            this._iconList.Images.SetKeyName(10, "ComponentProjectItemIcon");
            this._iconList.Images.SetKeyName(11, "VisualBasicProjectIcon");
            this._iconList.Images.SetKeyName(12, "VisualBasicItemIcon");
            this._iconList.Images.SetKeyName(13, "MasterPageIcon");
            this._iconList.Images.SetKeyName(14, "WebUserControlIcon");
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this._foldersComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this._webSiteProjectsComboBox);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // _foldersComboBox
            // 
            this._foldersComboBox.DataSource = this._foldersBindingSource;
            this._foldersComboBox.DisplayMember = "Name";
            resources.ApplyResources(this._foldersComboBox, "_foldersComboBox");
            this._foldersComboBox.FormattingEnabled = true;
            this._foldersComboBox.Name = "_foldersComboBox";
            // 
            // _foldersBindingSource
            // 
            this._foldersBindingSource.DataSource = typeof(Microsoft.Practices.RecipeFramework.Extensions.DteWrapper.IProjectItemModel);
            this._foldersBindingSource.CurrentChanged += new System.EventHandler(this._foldersBindingSource_CurrentChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // _webSiteProjectsComboBox
            // 
            this._webSiteProjectsComboBox.DataSource = this._webProjectsBindingSource;
            this._webSiteProjectsComboBox.DisplayMember = "Name";
            this._webSiteProjectsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this._webSiteProjectsComboBox, "_webSiteProjectsComboBox");
            this._webSiteProjectsComboBox.FormattingEnabled = true;
            this._webSiteProjectsComboBox.Name = "_webSiteProjectsComboBox";
            this._modelValidationProvider.SetSourcePropertyName(this._webSiteProjectsComboBox, "");
            // 
            // _webProjectsBindingSource
            // 
            this._webProjectsBindingSource.DataSource = typeof(Microsoft.Practices.RecipeFramework.Extensions.DteWrapper.IProjectModel);
            // 
            // _viewNameTextBox
            // 
            resources.ApplyResources(this._viewNameTextBox, "_viewNameTextBox");
            this._viewNameTextBox.Name = "_viewNameTextBox";
            this._modelValidationProvider.SetPerformValidation(this._viewNameTextBox, true);
            this._modelValidationProvider.SetSourcePropertyName(this._viewNameTextBox, "ViewName");
            this._viewNameTextBox.TextChanged += new System.EventHandler(this._viewNameTextBox_TextChanged);
            // 
            // _viewNameLabel
            // 
            resources.ApplyResources(this._viewNameLabel, "_viewNameLabel");
            this._viewNameLabel.Name = "_viewNameLabel";
            // 
            // assembliesTreeView
            // 
            resources.ApplyResources(this.assembliesTreeView, "assembliesTreeView");
            this.assembliesTreeView.ImageList = this._iconList;
            this.assembliesTreeView.Name = "assembliesTreeView";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // _moduleProjectsComboBox
            // 
            this._moduleProjectsComboBox.DataSource = this._moduleProjectsBindingSource;
            this._moduleProjectsComboBox.DisplayMember = "Name";
            this._moduleProjectsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._moduleProjectsComboBox.FormattingEnabled = true;
            resources.ApplyResources(this._moduleProjectsComboBox, "_moduleProjectsComboBox");
            this._moduleProjectsComboBox.Name = "_moduleProjectsComboBox";
            this._modelValidationProvider.SetSourcePropertyName(this._moduleProjectsComboBox, "");
            // 
            // _moduleProjectsBindingSource
            // 
            this._moduleProjectsBindingSource.DataSource = typeof(Microsoft.Practices.RecipeFramework.Extensions.DteWrapper.IProjectModel);
            this._moduleProjectsBindingSource.CurrentChanged += new System.EventHandler(this._moduleProjectsBindingSource_CurrentChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.assembliesTreeView);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.webSiteTreeView);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // _showDocumentation
            // 
            resources.ApplyResources(this._showDocumentation, "_showDocumentation");
            this._showDocumentation.Name = "_showDocumentation";
            this._showDocumentation.UseVisualStyleBackColor = true;
            // 
            // _modelValidationProvider
            // 
            this._modelValidationProvider.ErrorProvider = this._errorProvider;
            this._modelValidationProvider.RulesetName = "";
            this._modelValidationProvider.SourceTypeName = "Microsoft.Practices.WebClientFactory.CustomWizardPages.CreateViewPageBaseModel, M" +
                "icrosoft.Practices.WebClientFactory.GuidancePackage";
            // 
            // CreateViewPageBase
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this._showDocumentation);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._moduleProjectsComboBox);
            this.Controls.Add(this._viewNameTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._viewNameLabel);
            this.Controls.Add(this.label6);
            this.Name = "CreateViewPageBase";
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this._viewNameLabel, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this._viewNameTextBox, 0);
            this.Controls.SetChildIndex(this._moduleProjectsComboBox, 0);
            this.Controls.SetChildIndex(this.infoPanel, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this._showDocumentation, 0);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._foldersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._webProjectsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._moduleProjectsBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView webSiteTreeView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox _viewNameTextBox;
        private System.Windows.Forms.TreeView assembliesTreeView;
        private System.Windows.Forms.ComboBox _moduleProjectsComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource _webProjectsBindingSource;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox _webSiteProjectsComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox _foldersComboBox;
        private System.Windows.Forms.BindingSource _moduleProjectsBindingSource;
        private System.Windows.Forms.BindingSource _foldersBindingSource;
        private System.Windows.Forms.CheckBox _showDocumentation;
        private System.Windows.Forms.ImageList _iconList;
		protected System.Windows.Forms.Label _viewNameLabel;
        private Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider _modelValidationProvider;
    }
}
