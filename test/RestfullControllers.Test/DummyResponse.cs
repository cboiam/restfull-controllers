using System.Collections.Generic;
using System.Linq;
using RestfullControllers.Core.Responses;
using RestfullControllers.Dummy.Api.Entities;

namespace RestfullControllers.Test
{
    public class DummyResponse
    {
        public static Response<DummyEntity> GetResponse(DummyEntity entity = null)
        {
            if (entity != null)
            {
                entity.Links = new List<Link>
                {
                    new Link
                    {
                        Rel = "self",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "GET"
                    },
                    new Link
                    {
                        Rel = "update",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "PUT"
                    },
                    new Link
                    {
                        Rel = "update",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "PATCH"
                    },
                    new Link
                    {
                        Rel = "delete",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "DELETE"
                    }
                };

                entity.Person.Links = new List<Link>
                {
                    new Link
                    {
                        Rel = "self",
                        Href = $"http://localhost/people/{entity.Person.DocumentNumber}",
                        Method = "GET"
                    }
                };

                var people = entity.People.ToList();
                people.ForEach(person => 
                {
                    person.Links = new List<Link>
                    {
                        new Link
                        {
                            Rel = "self",
                            Href = $"http://localhost/people/{person.DocumentNumber}",
                            Method = "GET"
                        }
                    };
                });
                entity.People = people;
            }

            return new Response<DummyEntity>
            {
                Data = entity,
                Links = new List<Link>
                {
                    new Link
                    {
                        Rel = "get",
                        Href = "http://localhost/dummies",
                        Method = "GET"
                    },
                    new Link
                    {
                        Rel = "create",
                        Href = "http://localhost/dummies",
                        Method = "POST"
                    },
                    new Link
                    {
                        Rel = "error",
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
                        Rel = "self",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "GET"
                    },
                    new Link
                    {
                        Rel = "update",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "PUT"
                    },
                    new Link
                    {
                        Rel = "update",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "PATCH"
                    },
                    new Link
                    {
                        Rel = "delete",
                        Href = $"http://localhost/dummies/{entity.Id}",
                        Method = "DELETE"
                    }
                };

                entity.Person.Links = new List<Link>
                {
                    new Link
                    {
                        Rel = "self",
                        Href = $"http://localhost/people/{entity.Person.DocumentNumber}",
                        Method = "GET"
                    }
                };

                var people = entity.People.ToList();
                people.ForEach(person => 
                {
                    person.Links = new List<Link>
                    {
                        new Link
                        {
                            Rel = "self",
                            Href = $"http://localhost/people/{person.DocumentNumber}",
                            Method = "GET"
                        }
                    };
                });
                entity.People = people;
            });

            return new Response<IEnumerable<DummyEntity>>
            {
                Data = data,
                Links = new List<Link>
                {
                    new Link
                    {
                        Rel = "get",
                        Href = "http://localhost/dummies",
                        Method = "GET"
                    },
                    new Link
                    {
                        Rel = "create",
                        Href = "http://localhost/dummies",
                        Method = "POST"
                    },
                    new Link
                    {
                        Rel = "error",
                        Href = "http://localhost/dummies/error",
                        Method = "GET"
                    }
                }
            };
        }
    }
}