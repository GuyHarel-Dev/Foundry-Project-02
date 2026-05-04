namespace UI.Extensions
{
    static public class BuilderServicesExtensions
    {

        public static IServiceCollection AddFoundryHttpService(this IServiceCollection services, IConfiguration config)
        {
            // Extract the base URL from configuration or use a default value
            var baseUrl = config["FoundryWebApi:BaseUrl"] ?? "https://localhost:5001"; // Default URL if not specified in configuration

            services.AddScoped(sp =>
              new HttpClient
              {
                  BaseAddress = new Uri(baseUrl)
              });


            return services;
        }
    }
}
