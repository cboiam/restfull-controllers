using System;
using FluentAssertions;
using RestfullControllers.Core.Attributes;
using RestfullControllers.Core.Exceptions;
using RestfullControllers.Core.Extensions;
using Xunit;

namespace RestfullControllers.Test.Extensions
{
    public class GetEntityIdExtensionTest
    {
        [Fact]
        public void GetEntityId_ReturnsEntityId()
        {
            new IdEntity { Identifier = 1 }
                .GetEntityId()
                .Should().Be(1);
        }

        [Fact]
        public void GetEntityId_ThrowErrorWhenNoIdAttribute()
        {
            Action action = () => new EntityWithoutId().GetEntityId();
            action.Should().Throw<EntityWithoutIdException<EntityWithoutId>>()
                .WithMessage("EntityWithoutId must have an IdAttribute");
        }

        [Fact]
        public void GetEntityId_ThrowErrorWhenMultipleIdAttributes()
        {
            Action action = () => new EntityWithMultipleIds().GetEntityId();
            action.Should().Throw<EntityWithMultipleIdsException<EntityWithMultipleIds>>()
                .WithMessage("EntityWithMultipleIds must not have more than one IdAttribute");
        }

        private class IdEntity
        {
            [Id]
            public int Identifier { get; set; }
        }

        private class EntityWithoutId { }

        private class EntityWithMultipleIds
        {
            [Id]
            public int Id1 { get; set; }
            [Id]
            public int Id2 { get; set; }
        }
    }
}