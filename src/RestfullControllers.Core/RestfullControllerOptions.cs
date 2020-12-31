using Newtonsoft.Json.Serialization;

namespace RestfullControllers.Core
{
    public class RestfullControllerOptions
    {
        public NamingStrategy RelNamingStrategy { get; set; } = new CamelCaseNamingStrategy();
    }
}