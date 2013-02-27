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
using System.Collections;
using System.Collections.Specialized;

namespace Microsoft.Practices.Web.UI.WebControls.Utility
{
	/// <summary>
	/// An utility class to operate with <see cref="IDictionary"/> objects.
	/// </summary>
    public static class DictionaryHelper
    {
		/// <summary>
		/// Merges two <see cref="IDictionary"/> objects.
		/// </summary>
		/// <param name="dictionary1">An <see cref="IDictionary"/> object to merge.</param>
		/// <param name="dictionary2">An <see cref="IDictionary"/> object to merge.</param>
		/// <returns>An <see cref="OrderedDictionary"/> with the result of the merge operation.</returns>
        public static IDictionary MergeNameValueDictionaries(IDictionary dictionary1, IDictionary dictionary2)
        {
            Guard.ArgumentNotNull(dictionary1, "dictionary1");
            Guard.ArgumentNotNull(dictionary2, "dictionary2");
            
            OrderedDictionary result = new OrderedDictionary();
            AppendNameValuePairs(result, dictionary1);
            AppendNameValuePairs(result, dictionary2);
            return result;
        }

		/// <summary>
		/// Creates a read-only dictionary from the specified <see cref="IDictionary"/> object.
		/// </summary>
		/// <param name="dictionary">An <see cref="IDictionary"/> object from which to create the read-oly dictionary.</param>
		/// <returns>A read-only <see cref="OrderedDictionary"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validation done by Guard class.")]
        public static IDictionary GetReadOnlyDictionary(IDictionary dictionary)
        {
            Guard.ArgumentNotNull(dictionary, "dictionary");

            OrderedDictionary result = new OrderedDictionary();
            foreach (DictionaryEntry entry in dictionary)
            {
                result.Add(entry.Key, entry.Value);
            }
            return result.AsReadOnly();
        }

        private static void AppendNameValuePairs(IDictionary target, IDictionary source)
        {
            Guard.ArgumentNotNull(target, "target");
            Guard.ArgumentNotNull(source, "source");

            foreach (string key in source.Keys)
            {
                target.Add(key, source[key]);
            }
        }
    }
}
