using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Exceptions;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Text.Json;

namespace PockemonAPI.Controllers
{
    [Route("pokemon/tournament")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        public const int MaxId = 151;

        public PokemonController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration.GetValue<string>("AppSettings:connStr");

        }


        [HttpGet("statistics")]
        public async Task<IActionResult> GetPokemonStatistics([FromQuery] string sortBy, [FromQuery] string sortDirection = "desc")
        {
            ValidateSortingParameters(sortBy, sortDirection);

            List<Pokemon> pokemons = new List<Pokemon>{};

            try
            {
                pokemons = await FetchEightRandomPokemons();

                SimulateBattle(pokemons);

                pokemons = SortPokemons(pokemons, sortBy, sortDirection);

                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred: {ex.Message}" });
            }

        }
        private IActionResult? ValidateSortingParameters(string sortBy, string sortDirection)
        {
            var sortByOptions = new[] { "wins", "losses", "ties", "name", "id" };
            if (string.IsNullOrEmpty(sortBy))
            {
                return BadRequest(new { error = "sortBy parameter is required" });
            }

            var sortDirections = new[] { "asc", "desc" };
            if (!sortDirections.Contains(sortDirection.ToLower()))
            {
                return BadRequest(new { error = "sortDirection parameter is invalid" });
            }

            if (!sortByOptions.Contains(sortBy.ToLower()))
            {
                return BadRequest(new { error = "sortBy parameter is invalid" });
            }
            return null;
        }
        private async Task<List<Pokemon>> FetchEightRandomPokemons() {
            var pokemonsList = new List<Pokemon>();
            int[] seenIds = new int[] {};
            Random random = new Random();
            int randomId = 0;

            while (pokemonsList.Count < 8)
            {
                do
                {
                    randomId = random.Next(1, MaxId); // generate a random number between 1 and 99
                } while (seenIds.Contains(randomId));

                var response = await _httpClient.GetAsync($"{_apiUrl}/pokemon/{randomId}");
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApiRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
                var data = await response.Content.ReadAsStringAsync();
                    
                if (string.IsNullOrEmpty(data))
                {
                    throw new ApiRequestException($"No Pokémon found with ID: {randomId}");
                }

                var fetchedPokemon = JsonSerializer.Deserialize<PokemonDTO>(data);

                Pokemon pokemon = new Pokemon();
                pokemon.Id = randomId;
                pokemon.Name = fetchedPokemon.Name;
                pokemon.Type = fetchedPokemon.Types.First().Type.Name;
                pokemon.Ties = 0;
                pokemon.Losses = 0;
                pokemon.Wins = 0;
                pokemon.BaseExperience = fetchedPokemon.BaseExperience;
                pokemonsList.Add(pokemon);

                seenIds.Append(randomId);
            }
            return pokemonsList;
        }

        private void SimulateBattle(List<Pokemon> pokemons) {
            for (int i = 0; i < pokemons.Count; i++)
            {
                for (int j = i + 1; j < pokemons.Count; j++)
                {
                    var battleResult = CheckBattleResult(pokemons[i], pokemons[j]);

                    if (battleResult == 1)
                    {
                        pokemons[i].Wins++;
                        pokemons[j].Losses++;
                    }
                    else if (battleResult == -1)
                    {
                        pokemons[j].Wins++;
                        pokemons[i].Losses++;
                    }
                    else
                    {
                        pokemons[i].Ties++;
                        pokemons[j].Ties++;
                    }
                }
            }

        }

        private int CheckBattleResult(Pokemon pokemon1, Pokemon pokemon2)
        {
            var battleCriteria = new Dictionary<string, string>()  {
                { "water", "fire"},
                { "fire", "grass"},
                { "grass", "electric"},
                { "electric", "water"},
                { "ghost", "psychic"},
                { "psychic", "fighting"},
                { "fighting", "dark"},
                { "dark", "ghost"}
            };

            if (battleCriteria.TryGetValue(pokemon1.Type, out string defeatType) && defeatType == pokemon2.Type)
            {
                return 1;
            }
            if (battleCriteria.TryGetValue(pokemon2.Type, out defeatType) && defeatType == pokemon1.Type)
            {
                return -1;
            }

            if (pokemon1.BaseExperience > pokemon2.BaseExperience) 
                return 1;
            if (pokemon1.BaseExperience < pokemon2.BaseExperience) 
                return -1;

            return 0;
        }

        private List<Pokemon> SortPokemons(List<Pokemon> pokemons, string sortBy, string sortDirection)
        {
            var sortDirections = sortDirection.ToLower() == "asc" ? "OrderBy" : "OrderByDescending";

            var propertyInfo = typeof(Pokemon).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Invalid sortBy parameter: {sortBy}");
            }

            var sortedPokemons = sortDirections == "OrderBy"
                ? pokemons.OrderBy(pokemon => propertyInfo.GetValue(pokemon, null)).ToList()
                : pokemons.OrderByDescending(pokemon => propertyInfo.GetValue(pokemon, null)).ToList();

            return sortedPokemons;
        }
    }
}
