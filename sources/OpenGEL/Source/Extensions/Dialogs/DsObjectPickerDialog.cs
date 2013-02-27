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
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.DirectoryServices;
using System.Diagnostics;
using System.Security.Principal;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	/// <summary>
	/// Directory services object picker dialog.
	/// </summary>
	[ToolboxItem(true)]
	[DesignerCategory("Dialogs")]
	[DefaultProperty("SelectedObjects")]
	[ToolboxItemFilter("Microsoft.Practices.Windows.Forms.CredentialsDialog")]
	[PermissionSet(SecurityAction.Demand, Name="FullTrust")]
	public class DsObjectPickerDialog : CommonDialog
	{
		#region Fields

		private List<DsObjectPickerItem> selectedObjects;
		private DsObjectPickerScopeTypes scopeTypes;
		private DsObjectPickerInitialSettings initialSettings;
		private DsObjectPickerInitialScopes initialScopes;
		private DsObjectPickerUplevelFlags uplevelFlags;
		private DsObjectPickerDownlevelFlags downlevelFlags;
        private UnsafeNativeMethods.DsopInitInfo initInfo;
        private UnsafeNativeMethods.DsopScopeInitInfo scopeInitInfo;
        private bool mustInit;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private const string CfstrDsopDsSelectionList = "CFSTR_DSOP_DS_SELECTION_LIST";

		#endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DsObjectPickerDialog"/> class.
        /// </summary>
        public DsObjectPickerDialog()
            : base()
        {
            // Set initial values;
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DsObjectPickerDialog"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public DsObjectPickerDialog(IContainer container)
            : this()
		{
			container.Add(this);
        }

        #endregion

        #region Properties

        /// <summary>
		/// Gets or sets the selected objects.
		/// </summary>
		/// <value>The selected objects.</value>
		public List<DsObjectPickerItem> SelectedObjects
		{
			get { return selectedObjects; }
			set { selectedObjects = value; }
		}

		/// <summary>
		/// Gets or sets the scope types.
		/// </summary>
		/// <value>The scope types.</value>
		public DsObjectPickerScopeTypes ScopeTypes
		{
			get { return scopeTypes; }
			set { scopeTypes = value; }
		}

		/// <summary>
		/// Gets or sets the initial settings.
		/// </summary>
		/// <value>The initial settings.</value>
		public DsObjectPickerInitialSettings InitialSettings
		{
			get { return initialSettings; }
			set { initialSettings = value; }
		}

		/// <summary>
		/// Gets or sets the initial scopes.
		/// </summary>
		/// <value>The initial scopes.</value>
		public DsObjectPickerInitialScopes InitialScopes
		{
			get { return initialScopes; }
			set { initialScopes = value; }
		}

		/// <summary>
		/// Gets or sets the uplevel flags.
		/// </summary>
		/// <value>The uplevel flags.</value>
		public DsObjectPickerUplevelFlags UplevelFlags
		{
			get { return uplevelFlags; }
			set { uplevelFlags = value; }
		}

		/// <summary>
		/// Gets or sets the downlevel flags.
		/// </summary>
		/// <value>The downlevel flags.</value>
		//[CLSCompliant(false)]
		public DsObjectPickerDownlevelFlags DownlevelFlags
		{
			get { return downlevelFlags; }
			set { downlevelFlags = value; }
		}

		#endregion

		#region CommonDialog overrides

		/// <summary>
		/// When overridden in a derived class, specifies a common dialog box.
		/// </summary>
		/// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
		/// <returns>
		/// true if the dialog box was successfully run; otherwise, false.
		/// </returns>
		protected override bool RunDialog(IntPtr hwndOwner)
		{
            IDsObjectPicker iPicker = null;
			try
			{
				iPicker = InitializeObjectPicker();
				System.Runtime.InteropServices.ComTypes.IDataObject dataObj = null;
				iPicker.InvokeDialog(hwndOwner, out dataObj);
				this.selectedObjects = ProcessSelections(dataObj);
				return (selectedObjects.Count > 0);
			}
			finally
			{
                iPicker = null;
			}
		}

		/// <summary>
		/// When overridden in a derived class, resets the properties of a common dialog box to their default values.
		/// </summary>
		public override void Reset()
		{
			selectedObjects = new List<DsObjectPickerItem>();

            // min default values
            //scopeTypes = DsObjectPickerScopeTypes.TargetComputer |
            //             DsObjectPickerScopeTypes.UplevelJoinedDomain;
            scopeTypes = DsObjectPickerScopeTypes.DownlevelJoinedDomain |
                         DsObjectPickerScopeTypes.EnterpriseDomain |
                         DsObjectPickerScopeTypes.ExternalDownlevelDomain |
                         DsObjectPickerScopeTypes.ExternalUplevelDomain |
                         DsObjectPickerScopeTypes.GlobalCatalog |
                         DsObjectPickerScopeTypes.TargetComputer |
                         DsObjectPickerScopeTypes.UplevelJoinedDomain |
                         DsObjectPickerScopeTypes.UserEnteredDownlevelScope |
                         DsObjectPickerScopeTypes.UserEnteredUplevelScope;

			initialSettings = DsObjectPickerInitialSettings.SkipTargetComputerDCCheck;

			initialScopes = DsObjectPickerInitialScopes.StartingScope | 
                            DsObjectPickerInitialScopes.DefaultFilterUsers | 
                            DsObjectPickerInitialScopes.DefaultFilterGroups;

            // min default values
            //uplevelFlags = DsObjectPickerUplevelFlags.Users |
            //                DsObjectPickerUplevelFlags.WellknownPrincipals |
            //                DsObjectPickerUplevelFlags.BuiltinGroups |
            //                DsObjectPickerUplevelFlags.IncludeAdvancedView;
            uplevelFlags = DsObjectPickerUplevelFlags.BuiltinGroups |
						   DsObjectPickerUplevelFlags.DomainLocalGroupsDL |
						   DsObjectPickerUplevelFlags.DomainLocalGroupsSE |
						   DsObjectPickerUplevelFlags.GlobalGroupsDL |
						   DsObjectPickerUplevelFlags.GlobalGroupsSE |
						   DsObjectPickerUplevelFlags.IncludeAdvancedView |
						   DsObjectPickerUplevelFlags.UniversalGroupsDL |
						   DsObjectPickerUplevelFlags.UniversalGroupsSE |
						   DsObjectPickerUplevelFlags.Users |
						   DsObjectPickerUplevelFlags.WellknownPrincipals;

            // min default values
            //downlevelFlags = DsObjectPickerDownlevelFlags.Users;
            downlevelFlags = DsObjectPickerDownlevelFlags.AllWellknownSids |
                             DsObjectPickerDownlevelFlags.Users |
                             DsObjectPickerDownlevelFlags.LocalGroups |
                             DsObjectPickerDownlevelFlags.GlobalGroups;
            
            mustInit = true;
		}

		#endregion

		#region Private Methods

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

		private IDsObjectPicker InitializeObjectPicker()
		{
            DSObjectPicker picker = new DSObjectPicker();
            IDsObjectPicker ipicker = (IDsObjectPicker)picker;

            if (mustInit)
            {
                unsafe
                {
                    initInfo.cbSize = Marshal.SizeOf(initInfo);
                    initInfo.pwzTargetComputer = null; // local computer			
                    initInfo.cDsScopeInfos = 1;
                    initInfo.flOptions = (int)initialSettings;
                    initInfo.cAttributesToFetch = 0;
                    initInfo.apwzAttributeNames = null;
                    scopeInitInfo.cbSize = Marshal.SizeOf(typeof(UnsafeNativeMethods.DsopScopeInitInfo));
                    scopeInitInfo.flType = (int)scopeTypes;
                    scopeInitInfo.flScope = (int)initialScopes;
                    scopeInitInfo.FilterFlags.Uplevel.flBothModes = (int)uplevelFlags;
                    scopeInitInfo.FilterFlags.Uplevel.flMixedModeOnly = (int)uplevelFlags;
                    scopeInitInfo.FilterFlags.Uplevel.flNativeModeOnly = (int)uplevelFlags;
                    scopeInitInfo.FilterFlags.flDownlevel = (int)downlevelFlags;
                    scopeInitInfo.pwzADZPath = null;
                    scopeInitInfo.pwzDcName = null;
                    scopeInitInfo.hr = null;
                    fixed (UnsafeNativeMethods.DsopScopeInitInfo* pdsopScope = &scopeInitInfo)
                    {
                        initInfo.aDsScopeInfos = pdsopScope;
                    }
                }
                mustInit = false;
            }

			ipicker.Initialize(ref initInfo);

			return ipicker;
		}

		private List<DsObjectPickerItem> ProcessSelections(System.Runtime.InteropServices.ComTypes.IDataObject dataObj)
		{
			List<DsObjectPickerItem> selections = new List<DsObjectPickerItem>();

			if (dataObj == null)
			{
				return selections;
			}
			
			STGMEDIUM stg = new STGMEDIUM();
			stg.tymed = TYMED.TYMED_HGLOBAL;
			stg.unionmember = IntPtr.Zero;
			stg.pUnkForRelease = null;

			FORMATETC fe = new FORMATETC();
            if (!this.DesignMode)
            {
                fe.cfFormat = (short)System.Windows.Forms.DataFormats.GetFormat(CfstrDsopDsSelectionList).Id;
            }
			fe.ptd = IntPtr.Zero;
			fe.dwAspect = DVASPECT.DVASPECT_CONTENT;  
			fe.lindex = -1;
			fe.tymed = TYMED.TYMED_HGLOBAL;

			dataObj.GetData(ref fe, out stg);

			IntPtr pDsSL = UnsafeNativeMethods.GlobalLock(stg.unionmember);

			try
			{
				IntPtr current = pDsSL;
				int cnt = Marshal.ReadInt32(current);

				if (cnt > 0)
				{
					current = (IntPtr)((int)current + (Marshal.SizeOf(typeof(uint)) * 2));
					for (int i = 0; i < cnt; i++)
					{
						UnsafeNativeMethods.DsSelection selection = 
                            (UnsafeNativeMethods.DsSelection)Marshal.PtrToStructure(current, 
                            typeof(UnsafeNativeMethods.DsSelection));
						Marshal.DestroyStructure(current, typeof(UnsafeNativeMethods.DsSelection));

						current = (IntPtr)((int)current + Marshal.SizeOf(typeof(UnsafeNativeMethods.DsSelection)));

						DsObjectPickerItem item = new DsObjectPickerItem();
						item.Name = selection.pwzName;
                        item.Upn = GetUpnFromSelection(selection);
                        item.ClassName = selection.pwzClass.ToLowerInvariant();
                        item.Sid = GetSidFromUpn(item.Upn);
						selections.Add(item);
					}
				}
			}
			finally
			{
                if (stg.unionmember != IntPtr.Zero)
                {
                    UnsafeNativeMethods.GlobalUnlock(stg.unionmember);
                }
                UnsafeNativeMethods.ReleaseStgMedium(ref stg);
			}

			return selections;
		}

        private string GetUpnFromSelection(UnsafeNativeMethods.DsSelection selection)
        {
            if (!string.IsNullOrEmpty(selection.pwzUPN))
            {
                return selection.pwzUPN;
            }

            const string sidPropertyName = "objectSid";
            string upn = selection.pwzADsPath;

            // Try to get the UPN value from AD path
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(upn))
                {
                    if (entry.Properties.Contains(sidPropertyName))
                    {
                        SecurityIdentifier sid = new SecurityIdentifier((byte[])entry.Properties[sidPropertyName].Value, 0);                        
                        if (sid.IsAccountSid())
                        {
                            NTAccount acc = (NTAccount)sid.Translate(typeof(NTAccount));
                            upn = acc.Value;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.Message);
            }

            return upn;
        }

        private string GetSidFromUpn(string upn)
        {
            NTAccount account = new NTAccount(upn);
            SecurityIdentifier sid = (SecurityIdentifier)account.Translate(typeof(SecurityIdentifier));
            return sid.Value;
        }

		#endregion
	}
}
