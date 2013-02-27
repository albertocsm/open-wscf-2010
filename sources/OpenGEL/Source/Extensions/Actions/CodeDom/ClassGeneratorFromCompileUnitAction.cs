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
#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using Microsoft.Practices.RecipeFramework.Extensions.Actions.CodeDom.Helpers;
using System.Globalization; 
#endregion

namespace Microsoft.Practices.RecipeFramework.Extensions.Actions.CodeDom
{
    /// <summary>
    /// Generates class from CodeCompileUnit.
    /// </summary>
    public class ClassGeneratorFromCompileUnitAction : ConfigurableAction
    {
        private CodeCompileUnit compileUnit;
        private LanguageType language;
        private CodeDomProvider codeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ClassGeneratorFromCompileUnitAction"/> class.
        /// </summary>
        public ClassGeneratorFromCompileUnitAction()
        {
            this.language = LanguageType.cs;
        }

        #region Input properties

        
        /// <summary>
        /// CodeCompileUnit to generate the class
        /// </summary>
        [Input(Required = true)]
        public CodeCompileUnit CompileUnit
        {
            get { return compileUnit; }
            set { compileUnit = value; }
        }

        /// <summary>
        /// Language for the code provider
        /// </summary>
        [Input(Required = false)]
        public LanguageType Language
        {
            get { return language; }
            set { language = value; }
        }

        /// <summary>
        /// Gets or sets the code provider.
        /// </summary>
        /// <value>The code provider.</value>
        [Input(Required = false)]
        public CodeDomProvider CodeProvider
        {
            get { return codeProvider; }
            set { codeProvider = value; }
        } 

        #endregion

        #region Output properties

        private string _class;
        /// <summary>
        /// Class generated from the CodeCompileUnit using the provided Language
        /// </summary>
        [Output]
        public string Class
        {
            get { return _class; }
            set { _class = value; }
        }
        #endregion

        #region Action Member

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Execute"/>.
        /// </summary>
        public override void Execute()
        {
            CodeDomProvider provider = this.codeProvider ?? ProviderFactory.CreateProvider(this.language);

            CodeGeneratorOptions options = new CodeGeneratorOptions();

            using(StringWriter tw = new StringWriter(CultureInfo.InvariantCulture))
            {
                provider.GenerateCodeFromCompileUnit(this.compileUnit, tw, options);
                this._class = tw.ToString();
            }
        }

        /// <summary>
        /// See <see cref="M:Microsoft.Practices.RecipeFramework.IAction.Undo"/>.
        /// </summary>
        public override void Undo()
        {
            //Not Implemented
        }

        #endregion

        private static class ProviderFactory
        {
            /// <summary>
            /// Creates the provider.
            /// </summary>
            /// <param name="language">The language.</param>
            /// <returns></returns>
            public static CodeDomProvider CreateProvider(LanguageType language)
            {
                if(language == LanguageType.cs)
                {
                    return new CSharpCodeProvider();
                }
                else if(language == LanguageType.vb)
                {
                    return new VBCodeProvider();
                }
                throw new NotSupportedException(string.Format(CultureInfo.CurrentUICulture, Properties.Resources.LanguageNotSupported, language.ToString()));
            }
        }
    }
}
