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
using System.Text;

namespace Microsoft.Practices.RecipeFramework.Extensions.Dialogs
{
	/// <summary>
	/// Flags that indicate the scope types for the object to pick. 
	/// You can combine multiple scope types if all specified scopes use the same settings. 
	/// </summary>
	[Flags]	    
	public enum DsObjectPickerScopeTypes
	{
        /// <summary/>
		TargetComputer = 0x00000001,
        /// <summary/>
        UplevelJoinedDomain = 0x00000002,
        /// <summary/>
        DownlevelJoinedDomain = 0x00000004,
        /// <summary/>
        EnterpriseDomain = 0x00000008,
        /// <summary/>
        GlobalCatalog = 0x00000010,
        /// <summary/>
        ExternalUplevelDomain = 0x00000020,
        /// <summary/>
        ExternalDownlevelDomain = 0x00000040,
        /// <summary/>
        Workgroup = 0x00000080,
        /// <summary/>
        UserEnteredUplevelScope = 0x00000100,
        /// <summary/>
        UserEnteredDownlevelScope = 0x00000200
	}

	/// <summary>
	/// Flags that determine the object picker options.
	/// </summary>
	public enum DsObjectPickerInitialSettings
	{
		/// <summary>
		/// Allow user to select multiple objects.
		/// </summary>
		Multiselect = 0x00000001,
		/// <summary>
		/// Specify that this is not a domain controller.
		/// </summary>
		SkipTargetComputerDCCheck = 0x00000002
	}

	/// <summary>
	/// Flags that indicate the format used to return ADsPath for objects selected from this scope. 
	/// The flScope member can also indicate the initial scope displayed in the Look in drop-down list. 
	/// </summary>
	[Flags]
	public enum DsObjectPickerInitialScopes
	{
        /// <summary/>
        StartingScope = 0x00000001,
        /// <summary/>
        WantProviderWinNT = 0x00000002,
        /// <summary/>
        WantProviderLDAP = 0x00000004,
        /// <summary/>
        WantProviderGC = 0x00000008,
        /// <summary/>
        WantSidPath = 0x00000010,
        /// <summary/>
        WantDownlevelBuiltinPath = 0x00000020,
        /// <summary/>
        DefaultFilterUsers = 0x00000040,
        /// <summary/>
        DefaultFilterGroups = 0x00000080,
        /// <summary/>
        DefaultFilterComputers = 0x00000100,
        /// <summary/>
        DefaultFilterContacts = 0x00000200
	}

	/// <summary>
	/// Filter flags to use for an up-level scope, 
	/// regardless of whether it is a mixed or native mode domain. 
	/// </summary>
	[Flags]
	public enum DsObjectPickerUplevelFlags
	{
        /// <summary/>
        IncludeAdvancedView = 0x00000001,
        /// <summary/>
        Users = 0x00000002,
        /// <summary/>
        BuiltinGroups = 0x00000004,
        /// <summary/>
        WellknownPrincipals = 0x00000008,
        /// <summary/>
        UniversalGroupsDL = 0x00000010,
        /// <summary/>
        UniversalGroupsSE = 0x00000020,
        /// <summary/>
        GlobalGroupsDL = 0x00000040,
        /// <summary/>
        GlobalGroupsSE = 0x00000080,
        /// <summary/>
        DomainLocalGroupsDL = 0x00000100,
        /// <summary/>
        DomainLocalGroupsSE = 0x00000200,
        /// <summary/>
        Contacts = 0x00000400,
        /// <summary/>
        Computers = 0x00000800
	}

	/// <summary>
	/// Contains the filter flags to use for down-level scopes
	/// </summary>
	//[CLSCompliant(false)]
	[Flags]
	public enum DsObjectPickerDownlevelFlags : uint
	{
        /// <summary/>
        Users = 0x80000001,
        /// <summary/>
        LocalGroups = 0x80000002,
        /// <summary/>
        GlobalGroups = 0x80000004,
        /// <summary/>
        Computers = 0x80000008,
        /// <summary/>
        World = 0x80000010,
        /// <summary/>
        AuthenticatedUser = 0x80000020,
        /// <summary/>
        Anonymous = 0x80000040,
        /// <summary/>
        Batch = 0x80000080,
        /// <summary/>
        CreatorOwner = 0x80000100,
        /// <summary/>
        CreatorGroup = 0x80000200,
        /// <summary/>
        FilterDialup = 0x80000400,
        /// <summary/>
        Interactive = 0x80000800,
        /// <summary/>
        Network = 0x80001000,
        /// <summary/>
        Service = 0x80002000,
        /// <summary/>
        System = 0x80004000,
        /// <summary/>
        ExcludeBuiltinGroups = 0x80008000,
        /// <summary/>
        TerminalServer = 0x80010000,
        /// <summary/>
        AllWellknownSids = 0x80020000,
        /// <summary/>
        LocalService = 0x80040000,
        /// <summary/>
        NetworkService = 0x80080000,
        /// <summary/>
        RemoteLogon = 0x80100000
	}
}
