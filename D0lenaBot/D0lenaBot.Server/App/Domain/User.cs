using Newtonsoft.Json;

namespace D0lenaBot.Server.App.Domain
{
    // ToDo: 
    // * Decouple storage properties and decorators from domain model.
    // * Think about the partitio key
    public class User
    {
        public User()
        {
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string City => "Rosario";
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}
