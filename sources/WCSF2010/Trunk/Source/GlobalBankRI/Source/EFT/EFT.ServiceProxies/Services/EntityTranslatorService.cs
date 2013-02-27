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
using System.Collections.Generic;
using System.Text;

namespace GlobalBank.Commercial.EBanking.Modules.EFT.ServiceProxies.Services
{
    public class EntityTranslatorService : IEntityTranslatorService
    {
        private List<IEntityTranslator> _translators = new List<IEntityTranslator>();

        public void RegisterEntityTranslator(IEntityTranslator translator)
        {
            if (translator == null)
                throw new ArgumentNullException("translator");

            _translators.Add(translator);
        }

        public void RemoveEntityTranslator(IEntityTranslator translator)
        {
            if (translator == null)
                throw new ArgumentNullException("translator");

            _translators.Remove(translator);
        }

        public bool CanTranslate<TTarget, TSource>()
        {
            return CanTranslate(typeof(TTarget), typeof(TSource));
        }

        public bool CanTranslate(Type targetType, Type sourceType)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType");
            if (sourceType == null)
                throw new ArgumentNullException("sourceType");

            return IsArrayConversionPossible(targetType, sourceType) || FindTranslator(targetType, sourceType) != null;
        }

        public TTarget Translate<TTarget>(object source)
        {
            return (TTarget)Translate(typeof(TTarget), source);
        }

        public object Translate(Type targetType, object source)
        {
            if (targetType == null)
                throw new ArgumentNullException("targetType");

            if (source == null)
            {
                if (targetType.IsArray)
                {
                    return null;
                }
                else
                {
                    throw new ArgumentNullException("source");
                }
            }

            Type sourceType = source.GetType();

            if (IsArrayConversionPossible(targetType, sourceType))
            {
                return TranslateArray(targetType, source);
            }
            else
            {
                IEntityTranslator translator = FindTranslator(targetType, sourceType);
                if (translator != null)
                {
                    return translator.Translate(this, targetType, source);
                }
            }

            throw new EntityTranslatorException("No translator is available to perform the operation.");
        }

        private object TranslateArray(Type targetType, object source)
        {
            Type targetItemType = targetType.GetElementType();
            Array sourceArray = (Array)source;
            Array result = (Array)Activator.CreateInstance(targetType, sourceArray.Length);
            for (int i = 0; i < sourceArray.Length; i++)
            {
                object value = sourceArray.GetValue(i);
                if (value != null)
                    result.SetValue(Translate(targetItemType, sourceArray.GetValue(i)), i);
            }
            return result;
        }

        private bool IsArrayConversionPossible(Type targetType, Type sourceType)
        {
            if (targetType.IsArray && targetType.GetArrayRank() == 1 && sourceType.IsArray && sourceType.GetArrayRank() == 1)
            {
                return CanTranslate(targetType.GetElementType(), sourceType.GetElementType());
            }
            return false;
        }

        private IEntityTranslator FindTranslator(Type targetType, Type sourceType)
        {
            IEntityTranslator translator = _translators.Find(delegate(IEntityTranslator test)
            {
                return test.CanTranslate(targetType, sourceType);
            });

            return translator;
        }
    }
}
