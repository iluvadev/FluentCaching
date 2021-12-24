﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentCaching.Keys.Helpers
{
    internal interface IExpressionsHelper
    {
        MemberInfo GetProperty<T, TValue>(Expression<Func<T, TValue>> expression);
    }
}