﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Practices.RecipeFramework.Extensions.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Practices.RecipeFramework.Extensions.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Any Project of any language .
        /// </summary>
        internal static string AnyProjectReference {
            get {
                return ResourceManager.GetString("AnyProjectReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose a Certificate used for signing/encryption.\nThe Current store is : {0} - {1}..
        /// </summary>
        internal static string ChooseCertificateDielogMessage {
            get {
                return ResourceManager.GetString("ChooseCertificateDielogMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A CodeElement.
        /// </summary>
        internal static string CodeElementReference {
            get {
                return ResourceManager.GetString("CodeElementReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} project(s) failed to compile..
        /// </summary>
        internal static string CompilationFailed {
            get {
                return ResourceManager.GetString("CompilationFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose application configuration file.
        /// </summary>
        internal static string ConfigurationFileChooserTitle {
            get {
                return ResourceManager.GetString("ConfigurationFileChooserTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The folder &apos;{0}&apos; does not exists. 
        ///Do you want to create the new folder?.
        /// </summary>
        internal static string CreateNewFolderMessage {
            get {
                return ResourceManager.GetString("CreateNewFolderMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid directory name {0}.
        /// </summary>
        internal static string CreateSolutionDirectory_NoDots {
            get {
                return ResourceManager.GetString("CreateSolutionDirectory_NoDots", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid operation: {0}.
        /// </summary>
        internal static string CredUIReturn {
            get {
                return ResourceManager.GetString("CredUIReturn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select a file.
        /// </summary>
        internal static string DefaultFileChooserTitle {
            get {
                return ResourceManager.GetString("DefaultFileChooserTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All Files (*.*) |*.*.
        /// </summary>
        internal static string DefaultFilterExpression {
            get {
                return ResourceManager.GetString("DefaultFilterExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select a folder.
        /// </summary>
        internal static string DefaultFolderChooserTitle {
            get {
                return ResourceManager.GetString("DefaultFolderChooserTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No selection available.
        /// </summary>
        internal static string DteHelper_NoSelection {
            get {
                return ResourceManager.GetString("DteHelper_NoSelection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution Explorer not active.
        /// </summary>
        internal static string DteHelper_NoSolutionExplorer {
            get {
                return ResourceManager.GetString("DteHelper_NoSolutionExplorer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path not relative to the solution path.
        /// </summary>
        internal static string DteHelper_PathNotRelativeToSln {
            get {
                return ResourceManager.GetString("DteHelper_PathNotRelativeToSln", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Un supported project kind.
        /// </summary>
        internal static string DteHelper_UnsupportedProjectKind {
            get {
                return ResourceManager.GetString("DteHelper_UnsupportedProjectKind", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Executing action {0}.
        /// </summary>
        internal static string ExecutingAction {
            get {
                return ResourceManager.GetString("ExecutingAction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The recipe &apos;{0}&apos; ({1}) throw an exception while executing the action &apos;{2}&apos;.
        ///Error description: {3}..
        /// </summary>
        internal static string FailSafeCoordinatorExceptionMessage {
            get {
                return ResourceManager.GetString("FailSafeCoordinatorExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Folder {0} does not exist.
        /// </summary>
        internal static string FolderDoesNotExist {
            get {
                return ResourceManager.GetString("FolderDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The Credential Management API requires Windows XP / Windows Server 2003 or later..
        /// </summary>
        internal static string FunctionNotSupported {
            get {
                return ResourceManager.GetString("FunctionNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hierarchy is not a project..
        /// </summary>
        internal static string HierarchyArgumentException {
            get {
                return ResourceManager.GetString("HierarchyArgumentException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hierarchy is not a project..
        /// </summary>
        internal static string HierarchyIsNotProjectException {
            get {
                return ResourceManager.GetString("HierarchyIsNotProjectException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hierarchy is not sited..
        /// </summary>
        internal static string HierarchyNotSitedException {
            get {
                return ResourceManager.GetString("HierarchyNotSitedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}{1}{2}.
        /// </summary>
        internal static string InnerExceptionMessage {
            get {
                return ResourceManager.GetString("InnerExceptionMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find insertion point {0}.
        /// </summary>
        internal static string InsertionPointException {
            get {
                return ResourceManager.GetString("InsertionPointException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The caption has a maximum length of {0} characters..
        /// </summary>
        internal static string InvalidCaptionLength {
            get {
                return ResourceManager.GetString("InvalidCaptionLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Condition {0}/n{1}.
        /// </summary>
        internal static string InvalidConditionException {
            get {
                return ResourceManager.GetString("InvalidConditionException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid Identifier..
        /// </summary>
        internal static string InvalidIdentifiedArgumentException {
            get {
                return ResourceManager.GetString("InvalidIdentifiedArgumentException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The banner image height must be {0} pixels..
        /// </summary>
        internal static string InvalidImageHeight {
            get {
                return ResourceManager.GetString("InvalidImageHeight", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The banner image width must be {0} pixels..
        /// </summary>
        internal static string InvalidImageWidth {
            get {
                return ResourceManager.GetString("InvalidImageWidth", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The message has a maximum length of {0} characters..
        /// </summary>
        internal static string InvalidMessageLength {
            get {
                return ResourceManager.GetString("InvalidMessageLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password has a maximum length of {0} characters..
        /// </summary>
        internal static string InvalidPasswordLength {
            get {
                return ResourceManager.GetString("InvalidPasswordLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The principal name has a maximum length of {0} characters..
        /// </summary>
        internal static string InvalidPrincipalLength {
            get {
                return ResourceManager.GetString("InvalidPrincipalLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The target has a maximum length of {0} characters..
        /// </summary>
        internal static string InvalidTargetLength {
            get {
                return ResourceManager.GetString("InvalidTargetLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Language {0} not supported..
        /// </summary>
        internal static string LanguageNotSupported {
            get {
                return ResourceManager.GetString("LanguageNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Node0.
        /// </summary>
        internal static string NodeName0 {
            get {
                return ResourceManager.GetString("NodeName0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Node1.
        /// </summary>
        internal static string NodeName1 {
            get {
                return ResourceManager.GetString("NodeName1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Node2.
        /// </summary>
        internal static string NodeName2 {
            get {
                return ResourceManager.GetString("NodeName2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The folder &apos;{0}&apos; already contains files. These files may be overwritten by this process. 
        ///Do you want to proceed and overwrite this files?.
        /// </summary>
        internal static string OverwriteFilesInFolder {
            get {
                return ResourceManager.GetString("OverwriteFilesInFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An item with the name &apos;{0}&apos; already exists on disk in the specified project. 
        ///Do you want to overwrite it?.
        /// </summary>
        internal static string OverwriteItemMessage {
            get {
                return ResourceManager.GetString("OverwriteItemMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Project is not a VS project..
        /// </summary>
        internal static string ProjectIsNotVSProject {
            get {
                return ResourceManager.GetString("ProjectIsNotVSProject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Any Project Item.
        /// </summary>
        internal static string ProjectItemReference {
            get {
                return ResourceManager.GetString("ProjectItemReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Any CSharp Project.
        /// </summary>
        internal static string ProjectReference {
            get {
                return ResourceManager.GetString("ProjectReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Any Solution Folder.
        /// </summary>
        internal static string SolutionFolderReference {
            get {
                return ResourceManager.GetString("SolutionFolderReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Solution or Solution Folder.
        /// </summary>
        internal static string SolutionOrSolutionFolderReference {
            get {
                return ResourceManager.GetString("SolutionOrSolutionFolderReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A Solution.
        /// </summary>
        internal static string SolutionReference {
            get {
                return ResourceManager.GetString("SolutionReference", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Processing Actions... .
        /// </summary>
        internal static string StatusBarProgressMessage {
            get {
                return ResourceManager.GetString("StatusBarProgressMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ready.
        /// </summary>
        internal static string StatusBarReadyMessage {
            get {
                return ResourceManager.GetString("StatusBarReadyMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A string was expected.
        /// </summary>
        internal static string TypeNameValidatorExpectsString {
            get {
                return ResourceManager.GetString("TypeNameValidatorExpectsString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type named &apos;{0}&apos; was not found..
        /// </summary>
        internal static string TypeNameValidatorTypeNotFound {
            get {
                return ResourceManager.GetString("TypeNameValidatorTypeNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value is not optional.
        /// </summary>
        internal static string TypeNameValidatorValueNotOptional {
            get {
                return ResourceManager.GetString("TypeNameValidatorValueNotOptional", resourceCulture);
            }
        }
    }
}
