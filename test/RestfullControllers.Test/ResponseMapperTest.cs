using System.Collections.Generic;
using FluentAssertions;
using Moq;
using RestfullControllers.Core;
using RestfullControllers.Dummy.Api.Entities;
using Xunit;

namespace RestfullControllers.Test
{
    public class ResponseMapperTest
    {
        private readonly Mock<ILinkMapper<DummyEntity>> linkMapper;
        private readonly ResponseMapper<DummyEntity> responseMapper;

        public ResponseMapperTest()
        {
            linkMapper = new Mock<ILinkMapper<DummyEntity>>();
            responseMapper = new ResponseMapper<DummyEntity>(linkMapper.Object);
        }

        [Fact]
        public void MapResponse_ShouldCallLinkMappingsAndReturnEntity()
        {
            var entity = new DummyEntityFaker().Generate();

            var response = responseMapper.MapResponse(entity);

            response.Data.Should().BeEquivalentTo(entity);
            linkMapper.Verify(x => x.MapControllerLinks(), Times.Once);
            linkMapper.Verify(x => x.MapEntityLinks(entity), Times.Once);
        }

        [Fact]
        public void MapResponse_ShouldCallLinkMappingsAndReturnEntities()
        {
            var entities = new DummyEntityFaker().Generate(3);

            var response = responseMapper.MapResponse(entities);

            response.Data.Should().BeEquivalentTo(entities);
            linkMapper.Verify(x => x.MapControllerLinks(), Times.Once);
            linkMapper.Verify(x => x.MapEntityLinks(It.IsIn<DummyEntity>(entities)), Times.Exactly(3));
        }

        [Fact]
        public void MapResponse_ShouldNotCallLinkMappings_WhenEntityIsNull()
        {
            var response = responseMapper.MapResponse(null as DummyEntity);

            response.Data.Should().BeNull();
            linkMapper.Verify(x => x.MapControllerLinks(), Times.Once);
            linkMapper.Verify(x => x.MapEntityLinks(It.IsAny<DummyEntity>()), Times.Never);
        }

        [Fact]
        public void MapResponse_ShouldNotCallLinkMappings_WhenEntitiesIsNull()
        {
            var response = responseMapper.MapResponse(null as List<DummyEntity>);

            response.Data.Should().BeNull();
            linkMapper.Verify(x => x.MapControllerLinks(), Times.Once);
            linkMapper.VerifyNoOtherCalls();
        }

        [Fact]
        public void MapResponse_ShouldNotCallLinkMappings_WhenEntitiesIsEmpty()
        {
            var response = responseMapper.MapResponse(new List<DummyEntity>());

            response.Data.Should().NotBeNull().And.BeEmpty();
            linkMapper.Verify(x => x.MapControllerLinks(), Times.Once);
            linkMapper.VerifyNoOtherCalls();
        }
    }
}