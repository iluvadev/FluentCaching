﻿using System;
using FluentAssertions;
using FluentCaching.Tests.Integration.Extensions;
using FluentCaching.Tests.Integration.Models;
using Xunit;

namespace FluentCaching.Tests.Integration.Configuration
{
    public class ConfigurationTests : BaseTest
    {
        [Fact]
        public void ForOfT_NotAPropertyExpression_DoesNotThrowException()
        {
            Action forOfUser = () =>
                CacheBuilder
                    .For<User>(_ => _.UseAsKey(u => 1 + 1).Complete());

            forOfUser.Should().NotThrow();
        }
    }
}
