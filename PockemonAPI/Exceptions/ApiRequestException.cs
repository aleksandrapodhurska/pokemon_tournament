namespace PokemonAPI.Exceptions
{
    public class ApiRequestException : Exception
    {
        public ApiRequestException(string message) : base(message) { }
        public ApiRequestException(string message, Exception innerException) : base(message, innerException) { }
    }

}
