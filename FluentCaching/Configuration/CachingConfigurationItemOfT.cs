﻿
using System;
using System.Collections.Generic;
using System.Text;
using FluentCaching.Api;
using FluentCaching.Api.Keys;
using FluentCaching.Keys;
using FluentCaching.Parameters;

namespace FluentCaching.Configuration
{
    internal class CachingConfigurationItem<T> : CachingConfigurationItem where T : class
    {
        public CachingConfigurationItem(CachingOptions options)
        {
            Tracker = options.PropertyTracker as PropertyTracker<T>;
            Options = options;
        }

        public PropertyTracker<T> Tracker { get; }

        public CachingOptions Options { get; }
    }
}
