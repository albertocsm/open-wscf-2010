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
    partial class CreateFoundationalModulePage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateFoundationalModulePage));
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._iconList = new System.Windows.Forms.ImageList(this.components);
            this._testProject = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._createModuleInterfaceLibrary = new System.Windows.Forms.CheckBox();
            this._showDocumentation = new System.Windows.Forms.CheckBox();
            this._moduleNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.assembliesTreeView = new System.Windows.Forms.TreeView();
            this.label6 = new System.Windows.Forms.Label();
            this._webProjectsComboBox = new System.Windows.Forms.ComboBox();
            this._webProjectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._modelValidationProvider = new Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._webProjectsBindingSource)).BeginInit();
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
            // _testProject
            // 
            resources.ApplyResources(this._testProject, "_testProject");
            this._testProject.Name = "_testProject";
            this._testProject.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._createModuleInterfaceLibrary);
            this.groupBox1.Controls.Add(this._showDocumentation);
            this.groupBox1.Controls.Add(this._testProject);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // _createModuleInterfaceLibrary
            // 
            resources.ApplyResources(this._createModuleInterfaceLibrary, "_createModuleInterfaceLibrary");
            this._createModuleInterfaceLibrary.Name = "_createModuleInterfaceLibrary";
            this._createModuleInterfaceLibrary.UseVisualStyleBackColor = true;
            // 
            // _showDocumentation
            // 
            resources.ApplyResources(this._showDocumentation, "_showDocumentation");
            this._showDocumentation.Name = "_showDocumentation";
            this._showDocumentation.UseVisualStyleBackColor = true;
            // 
            // _moduleNameTextBox
            // 
            resources.ApplyResources(this._moduleNameTextBox, "_moduleNameTextBox");
            this._moduleNameTextBox.Name = "_moduleNameTextBox";
            this._modelValidationProvider.SetPerformValidation(this._moduleNameTextBox, true);
            this._moduleNameTextBox.ReadOnly = true;
            this._modelValidationProvider.SetSourcePropertyName(this._moduleNameTextBox, "ModuleName");
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // _webProjectsComboBox
            // 
            this._webProjectsComboBox.DataSource = this._webProjectsBindingSource;
            this._webProjectsComboBox.DisplayMember = "ProjectName";
            this._webProjectsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._webProjectsComboBox.FormattingEnabled = true;
            resources.ApplyResources(this._webProjectsComboBox, "_webProjectsComboBox");
            this._webProjectsComboBox.Name = "_webProjectsComboBox";
            this._webProjectsComboBox.ValueMember = "ProjectModel";
            // 
            // _webProjectsBindingSource
            // 
            this._webProjectsBindingSource.DataSource = typeof(Microsoft.Practices.WebClientFactory.CustomWizardPages.ProjectListItem);
            this._webProjectsBindingSource.CurrentChanged += new System.EventHandler(this._webProjectsBindingSource_CurrentChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.assembliesTreeView);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // _modelValidationProvider
            // 
            this._modelValidationProvider.ErrorProvider = this._errorProvider;
            this._modelValidationProvider.RulesetName = "";
            this._modelValidationProvider.SourceTypeName = "Microsoft.Practices.WebClientFactory.CustomWizardPages.CreateFoundationalModulePa" +
                "geModel, Microsoft.Practices.WebClientFactory.GuidancePackage";
            // 
            // CreateFoundationalModulePage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this._webProjectsComboBox);
            this.Controls.Add(this._moduleNameTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Name = "CreateFoundationalModulePage";
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this._moduleNameTextBox, 0);
            this.Controls.SetChildIndex(this._webProjectsComboBox, 0);
            this.Controls.SetChildIndex(this.infoPanel, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._webProjectsBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider _errorProvider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox _testProject;
        private System.Windows.Forms.TextBox _moduleNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView assembliesTreeView;
        private System.Windows.Forms.ComboBox _webProjectsComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.BindingSource _webProjectsBindingSource;
        private System.Windows.Forms.ImageList _iconList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox _showDocumentation;
        private Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.ValidationProvider _modelValidationProvider;
        private System.Windows.Forms.CheckBox _createModuleInterfaceLibrary;
    }
}
