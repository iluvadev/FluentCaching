﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentCaching.Keys;
using FluentCaching.Keys.Helpers;

namespace FluentCaching.Keys.Builders
{
    internal class KeyBuilder<T> : IKeyBuilder<T>
        where T : class
    {
        private bool _hasDynamicParts = false;
        private List<Func<KeyContextSource<T>, string>> _keyPartBuilders = new();

        private readonly IKeyContextBuilder _keyContextBuilder;
        private readonly IExpressionsHelper _expressionHelper;

        public KeyBuilder() 
            : this(new KeyContextBuilder(new ComplexKeysHelper()), new ExpressionsHelper())
        {
            
        }

        internal KeyBuilder(IKeyContextBuilder keyContextBuilder, IExpressionsHelper expressionHelper)
        {
            _keyContextBuilder = keyContextBuilder;
            _expressionHelper = expressionHelper;
        }

        public void AppendStatic<TValue>(TValue value)
        {
            var staticPart = value?.ToString();
            ThrowIfKeyPartIsEmpty(staticPart);
            _keyPartBuilders.Add(_ => staticPart);
        }

        public void AppendExpression<TValue>(Expression<Func<T, TValue>> valueGetter)
        {
            _hasDynamicParts = true;
            var property = _expressionHelper.GetProperty(valueGetter).Name;
            _keyContextBuilder.AddKey(property);
            var compiledExpression = _expressionHelper
                .RewriteWithSafeToString(valueGetter)
                .Compile();
            Func<KeyContextSource<T>, string> keyPartBuilder = 
                _ => ThrowIfKeyPartIsEmpty(_.Store != null
                    ? compiledExpression(_.Store)
                    : _.Retrieve[property]?.ToString());
            _keyPartBuilders.Add(keyPartBuilder);
        }

        public string BuildFromCachedObject(T cachedObject) => BuildKey(new KeyContextSource<T>(cachedObject));

        public string BuildFromStringKey(string stringKey)
        {
            var context = _keyContextBuilder.BuildKeyContextFromString(stringKey);
            var source = new KeyContextSource<T>(context);

            return BuildKey(source);
        }

        public string BuildFromObjectKey(object objectKey)
        {
            var context = _keyContextBuilder.BuildKeyContextFromObject(objectKey);
            var source = new KeyContextSource<T>(context);

            return BuildKey(source);
        }

        public string BuildFromStaticKey()
        {
            if (_hasDynamicParts)
            {
                throw new KeyPartMissingException();
            }

            return BuildKey(KeyContextSource<T>.Null);
        }

        private static string ThrowIfKeyPartIsEmpty(string part)
        {
            if (string.IsNullOrEmpty(part))
            {
                throw new KeyPartMissingException();
            }

            return part;
        }

        private string BuildKey(KeyContextSource<T> contextSource) => string.Join(string.Empty, BuildKeyParts(contextSource));
        
        private IEnumerable<string> BuildKeyParts(KeyContextSource<T> contextSource)
        {
            foreach (var action in _keyPartBuilders)
            {
                yield return action(contextSource);
            }
        }
    }
}
