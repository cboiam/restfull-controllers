using System.Collections.Generic;
using System.Linq;
using RestfullControllers.Core.Responses;
using RestfullControllers.Dummy.Api.Entities;

namespace RestfullControllers.Test
{
    public class DummyResponse
    {
        public static Response<DummyEntity> GetResponse(DummyEntity entity)
        {
            entity.Links = new List<Link>
            {
                new Link
                {
                    Rel = "Self",
                    Href = $"http://localhost/dummies/{entity.Id}",
                    Method = "GET"
                },
                new Link
                {
                    Rel = "Update",
                    Href = $"http://localhost/dummies/{entity.Id}",
                    Method = "PUT"
                },
                new Link
                {
                    Rel = "Update",
                    Href = $"http://localhost/dummies/{entity.Id}",
                    Method = "PATCH"
                },
                new Link
                {
                    Rel = "Delete",
                    Href = $"http://localhost/dummies/{entity.Id}",
                    Method = "DELETE"
                }
            };

            return new Response<DummyEntity>
            {
                Data = entity,
                Links = new List<Link>
                {
                    new Link
                    {
                        Rel = "Get",
                        Href = "http://localhost/dummies",
                        Method = "GET"
                    },
                    new Link
                    {
                        Rel = "Create",
                        Href = "http://localhost/dummies",
                        Method = "POST"
                    },
                    new Link
                    {
                        Rel = "Error",
                        Href = "http://localhost/dummies/error",
                        Method = "GET"
                    }
                }
            };
        }

        public static Response<IEnumerable<DummyEntity>> GetResponse(IEnumerable<DummyEntity> entities)
        {
            var data = entities.ToList();
            data.ForEach(entity => 
            {
                entity.Links = new List<Link>
                {
                    new Link
                    {
                        Rel = "Self",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "GET"
                    },
                    new Link
                    {
                        Rel = "Update",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "PUT"
                    },
                    new Link
                    {
                        Rel = "Update",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "PATCH"
                    },
                    new Link
                    {
                        Rel = "Delete",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "DELETE"
                    }
                };
            });

            return new Response<IEnumerable<DummyEntity>>
            {
                Data = data,
                Links = new List<Link>
                {
                    new Link
                    {
                        Rel = "Get",
                        Href = "http://localhost/dummies",
                        Method = "GET"
                    },
                    new Link
                    {
                        Rel = "Create",
                        Href = "http://localhost/dummies",
                        Method = "POST"
                    },
                    new Link
                    {
                        Rel = "Error",
                        Href = "http://localhost/dummies/error",
                        Method = "GET"
                    }
                }
            };
        }
    }
}