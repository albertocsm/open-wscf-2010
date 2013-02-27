//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory 2010
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
using System;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;
using System.IO;
using System.Text;

namespace Microsoft.Practices.WebClientFactory.Actions
{
    /// <summary>
    /// The action creates a project item from a string passed to the action
    /// in the Content input property. The other two input properties of the
    /// action are (a) targetFileName - provides the name of the item file 
    /// and (b) ProjectItem - identifies the project item where the item is created. 
    /// </summary>
    [ServiceDependency(typeof(DTE))]
    public class AddItemFromStringExtendedAction : ConfigurableAction
    {
        private const string SolutionFolderKind = "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
        private const string DtePropertiesFullPath = "FullPath";

        #region Input Properties

        private string content;
        /// <summary>
        /// The string with the content of the item. In most cases it is a class
        /// generated using the T4 engine.
        /// </summary>
        [Input(Required = true)]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string targetFileName;
        /// <summary>
        /// Name of the file where the item is to be stored.
        /// </summary>
        [Input(Required = true)]
        public string TargetFileName
        {
            get { return targetFileName; }
            set { targetFileName = value; }
        }

        private Project project;
        /// <summary>
        /// Project where the item it is going to be inserted.
        /// </summary>
        [Input(Required = true)]
        public Project Project
        {
            get { return project; }
            set { project = value; }
        }

        private ProjectItem inputProjectItem;
        /// <summary>
        /// ProjectItem where the item it is going to be inserted.
        /// This can be null, and the item will be added to the root folder.
        /// </summary>
        [Input(Required = false)]
        public ProjectItem InputProjectItem
        {
            get { return inputProjectItem; }
            set { inputProjectItem = value; }
        }

        private bool open = true;
        /// <summary>
        /// Gets or sets if the item should opened after created
        /// </summary>
        [Input(Required = true)]
        public bool Open
        {
            get { return open; }
            set { open = value; }
        }
        #endregion Input Properties

        #region Output Properties

        private ProjectItem projectItem;
        /// <summary>
        /// A property that can be used to pass the creted item to
        /// a following action.
        /// </summary>
        [Output]
        public ProjectItem ProjectItem
        {
            get { return projectItem; }
            set { projectItem = value; }
        }

        #endregion Output Properties

        /// <summary>
        /// The method that creates a new item from the intput string.
        /// </summary>
        public override void Execute()
        {
            DTE service = (DTE)GetService(typeof(DTE));
            string tempFileName = Path.GetTempFileName();
            using (StreamWriter writer = new StreamWriter(tempFileName, false, new UTF8Encoding(true, true)))
            {
                writer.WriteLine(content);
            }
            if (inputProjectItem != null)
            {
                projectItem = InputProjectItem.ProjectItems.AddFromTemplate(tempFileName, targetFileName);
            }
            else
            {
                projectItem = project.ProjectItems.AddFromTemplate(tempFileName, targetFileName);
            }

            if (open)
            {
                Window window = projectItem.Open("{00000000-0000-0000-0000-000000000000}");
                window.Visible=true;
                window.Activate();
            }
            File.Delete(tempFileName);
        }

        /// <summary>
        /// Undoes the creation of the item, then deletes the item
        /// </summary>
        public override void Undo()
        {
            if (projectItem != null)
            {
                projectItem.Delete();
            }
        }
    }
}
