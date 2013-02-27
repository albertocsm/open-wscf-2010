//===============================================================================
// Microsoft patterns & practices
//  GAX Extension Library
//===============================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================
#region using

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.Common;
using System.Collections.Specialized;
using System.Windows.Forms.Design;

#endregion using

namespace Microsoft.Practices.RecipeFramework.Extensions.Editors.ClassBrowser
{
    /// <summary>
    /// Allows browsing for a class name.
    /// </summary>
    internal class ClassBrowserEditorForm : System.Windows.Forms.Form
    {
        #region Designer stuff
        private System.Windows.Forms.ImageList imgIcons;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog dlgOpenAssembly;
        private System.Windows.Forms.ToolTip tpTooltip;
        private System.Windows.Forms.TreeView tvBrowser;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassBrowserEditorForm));
            this.imgIcons = new System.Windows.Forms.ImageList(this.components);
            this.tvBrowser = new System.Windows.Forms.TreeView();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.dlgOpenAssembly = new System.Windows.Forms.OpenFileDialog();
            this.tpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgIcons
            // 
            this.imgIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons.ImageStream")));
            this.imgIcons.TransparentColor = System.Drawing.Color.White;
            this.imgIcons.Images.SetKeyName(0, "");
            this.imgIcons.Images.SetKeyName(1, "");
            this.imgIcons.Images.SetKeyName(2, "");
            this.imgIcons.Images.SetKeyName(3, "");
            this.imgIcons.Images.SetKeyName(4, "");
            this.imgIcons.Images.SetKeyName(5, "");
            this.imgIcons.Images.SetKeyName(6, "");
            // 
            // tvBrowser
            // 
            this.tvBrowser.AccessibleDescription = null;
            this.tvBrowser.AccessibleName = null;
            resources.ApplyResources(this.tvBrowser, "tvBrowser");
            this.tvBrowser.BackgroundImage = null;
            this.tvBrowser.Font = null;
            this.tvBrowser.ImageList = this.imgIcons;
            this.tvBrowser.Name = "tvBrowser";
            this.tvBrowser.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("tvBrowser.Nodes")))});
            this.tvBrowser.Sorted = true;
            this.tpTooltip.SetToolTip(this.tvBrowser, resources.GetString("tvBrowser.ToolTip"));
            this.tvBrowser.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.OnBeforeExpand);
            this.tvBrowser.DoubleClick += new System.EventHandler(this.tvBrowser_DoubleClick);
            this.tvBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterSelect);
            // 
            // pnlButtons
            // 
            this.pnlButtons.AccessibleDescription = null;
            this.pnlButtons.AccessibleName = null;
            resources.ApplyResources(this.pnlButtons, "pnlButtons");
            this.pnlButtons.BackgroundImage = null;
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Font = null;
            this.pnlButtons.Name = "pnlButtons";
            this.tpTooltip.SetToolTip(this.pnlButtons, resources.GetString("pnlButtons.ToolTip"));
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = null;
            this.btnCancel.Name = "btnCancel";
            this.tpTooltip.SetToolTip(this.btnCancel, resources.GetString("btnCancel.ToolTip"));
            // 
            // btnOK
            // 
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackgroundImage = null;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = null;
            this.btnOK.Name = "btnOK";
            this.tpTooltip.SetToolTip(this.btnOK, resources.GetString("btnOK.ToolTip"));
            // 
            // dlgOpenAssembly
            // 
            this.dlgOpenAssembly.DefaultExt = "dll";
            resources.ApplyResources(this.dlgOpenAssembly, "dlgOpenAssembly");
            // 
            // ClassBrowserEditorForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.tvBrowser);
            this.Controls.Add(this.pnlButtons);
            this.Font = null;
            this.Name = "ClassBrowserEditorForm";
            this.tpTooltip.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        #region Fields
        List<Assembly> assemblies;
        List<Type> types;
        #endregion

        #region Ctor

        /// <summary>
        /// Provides a UI for the <see cref="ClassBrowserEditor"/>
        /// </summary>
        public ClassBrowserEditorForm(List<Assembly> assemblies, List<Type> types)
        {
            this.assemblies = assemblies;
            this.types = types;
            InitializeComponent();
        }

        #endregion Ctor

        #region Protected methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Cursor.Current = Cursors.WaitCursor;
            OnLoadNodes();
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Provide a hook for derived classes to modify the initial 
        /// list of types.
        /// </summary>
        /// 

        protected virtual void OnLoadNodes()
        {
            tvBrowser.Nodes.Clear();

            foreach(Assembly assembly in this.assemblies)
            {
                TreeNode node = new AssemblyNode(assembly);
                node.Nodes.Add(new TreeNode());
                tvBrowser.Nodes.Add(node);
            }
        }
        #endregion Protected methods

        #region Event handlers

        private void OnBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            HybridDictionary namespaces = new HybridDictionary();

            if(e.Node is AssemblyNode && e.Node.Tag != null)
            {
                tvBrowser.SuspendLayout();

                e.Node.Nodes.Clear();
                Assembly asm = (Assembly)e.Node.Tag;
                e.Node.Tag = null;

                foreach(Type t in this.types)
                {
                    if(t.Assembly == asm)
                    {
                        TreeNode ns = null;
                        // Non-namespaced types go directly under the assembly node.
                        if(t.Namespace == null)
                        {
                            ns = e.Node;
                        }
                        else
                        {
                            // Check if have already created the namespace node.
                            object key = String.Intern(t.Namespace);
                            ns = (TreeNode)namespaces[key];
                            if(ns == null)
                            {
                                ns = new NamespaceNode(t);
                                e.Node.Nodes.Add(ns);
                                namespaces.Add(key, ns);
                            }
                        }

                        if (t.IsClass)
                        {
                            ns.Nodes.Add(new ClassNode(t));
                        }
                        else if (t.IsAnsiClass)
                        {
                            ns.Nodes.Add(new ClassNode(t));
                        }
                        else if (t.IsInterface)
                        {
                            ns.Nodes.Add(new InterfaceNode(t));
                        }
                        else if (t.IsEnum)
                        {
                            ns.Nodes.Add(new EnumNode(t));
                        }
                    }
                }

                tvBrowser.ResumeLayout();
            }
        }

        private void btnOpen_Click(object sender, System.EventArgs e)
        {
            try
            {
                if(dlgOpenAssembly.ShowDialog(this) == DialogResult.OK)
                {
                    Assembly asm = Assembly.LoadFrom(dlgOpenAssembly.FileName);
                    TreeNode node = new AssemblyNode(asm);
                    node.Nodes.Add(new TreeNode());
                    tvBrowser.Nodes.Add(node);
                }
            }
            catch(Exception ex)
            {
                IUIService svc = (IUIService)this.GetService(typeof(IUIService));
                if (svc != null)
                {
                    svc.ShowError(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }

        private void tvBrowser_DoubleClick(object sender, System.EventArgs e)
        {
            if(tvBrowser.SelectedNode is ClassNode)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void tvBrowser_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnOK.Enabled = SelectedType != null;
        }

        #endregion Event handlers

        #region Properties

        /// <summary>
        /// Exposes the full type name of the element selected.
        /// </summary>
        internal string TypeFullName
        {
            get
            {
                if(tvBrowser.SelectedNode == null ||
                    !(tvBrowser.SelectedNode is ClassNode)) return null;

                Type t = (Type)tvBrowser.SelectedNode.Tag;

                return t.FullName;
            }
        }

        /// <summary>
        /// Exposes the currently selected type.
        /// </summary>
        protected Type SelectedType
        {
            get
            {
                if(tvBrowser.SelectedNode == null ||
                    !(tvBrowser.SelectedNode is ClassNode)) return null;

                return (Type)tvBrowser.SelectedNode.Tag;
            }
        }

        /// <summary>
        /// Exposes the <see cref="TreeView"/> used to show types.
        /// </summary>
        protected TreeView Browser
        {
            get { return this.tvBrowser; }
        }

        #endregion Properties

        #region Derived nodes

        protected class AssemblyNode : TreeNode
        {
            public AssemblyNode(Assembly asm)
            {
                base.Text = ReflectionHelper.GetAssemblyName(asm);
                base.Tag = asm;
                base.ImageIndex = 0;
                base.SelectedImageIndex = 0;
            }
        }

        protected class NamespaceNode : TreeNode
        {
            public NamespaceNode(Type t)
            {
                base.Text = t.Namespace;
                base.Tag = t;
                base.ImageIndex = 1;
                base.SelectedImageIndex = 1;
            }
        }

        protected class ClassNode : TreeNode
        {
            public ClassNode(Type t)
            {
                base.Text = t.Name;
                base.Tag = t;
                base.ImageIndex = 2;
                base.SelectedImageIndex = 2;
            }
        }

        protected class InterfaceNode : ClassNode
        {
            public InterfaceNode(Type t)
                : base(t)
            {
                base.ImageIndex = 3;
                base.SelectedImageIndex = 3;
            }
        }

        protected class EnumNode : ClassNode
        {
            public EnumNode(Type t)
                : base(t)
            {
                base.ImageIndex = 4;
                base.SelectedImageIndex = 4;
            }
        }
        #endregion Derived nodes
    }
}
