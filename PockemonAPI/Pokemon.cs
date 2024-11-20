using System.Text.Json.Serialization;

namespace PockemonAPI
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Ties { get; set; }
        public int Losses { get; set; }
        public int Wins { get; set; }
        public int BaseExperience { get; set; }

    }

    public class PokemonDTO
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("base_experience")]
        public int BaseExperience { get; set; }
    }

}
