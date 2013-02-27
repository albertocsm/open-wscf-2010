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
using System.Security.Cryptography.X509Certificates;
using Microsoft.Practices.RecipeFramework.Library;

namespace Microsoft.Practices.RecipeFramework.Extensions.CommonHelpers
{
    /// <summary>
    /// Helper class for <see cref="System.Security.Cryptography.X509Certificates.X509Certificate"/> certificates.
    /// </summary>
    public static class X509CertificateHelper
    {
        /// <summary>
        /// Gets the find value.
        /// </summary>
        /// <param name="findType">Type of the find.</param>
        /// <param name="value">The value.</param>
        /// <returns>The find value formated as string.</returns>
        public static string GetFindValue(X509FindType findType, X509Certificate2 value)
        {
            string findValue = null;

            switch (findType)
            {
                case X509FindType.FindBySubjectDistinguishedName:
                    findValue = GetSubjectDistinguishedName(value.SubjectName.Name);
                    break;
                case X509FindType.FindByThumbprint:
                    findValue = value.Thumbprint;
                    break;
                case X509FindType.FindBySubjectName:
                    findValue = value.SubjectName.Name;
                    break;
                case X509FindType.FindBySerialNumber:
                    findValue = value.SerialNumber;
                    break;
                default:
                    findValue = value.ToString(false);
                    break;
            }

            return findValue;
        }

        /// <summary>
        /// Gets the name of the subject distinguished.
        /// </summary>
        /// <param name="subjectName">Name of the subject.</param>
        /// <returns>Returns the SubjectDistinguishedName under the form: CN={DistinguishedName}</returns>
        public static string GetSubjectDistinguishedName(string subjectName)
        {
            Guard.ArgumentNotNull(subjectName, "subjectName");

            string result = subjectName;
            string[] subjectNameParts = subjectName.Split(',');

            if (subjectNameParts != null)
            {
                foreach (string part in subjectNameParts)
                {
                    if (part.StartsWith("CN", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = part;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
