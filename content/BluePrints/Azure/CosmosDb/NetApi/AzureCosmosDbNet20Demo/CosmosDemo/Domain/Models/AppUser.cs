using Newtonsoft.Json;

namespace CosmosDemo.Domain.Models
{
    public class AppUser
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
