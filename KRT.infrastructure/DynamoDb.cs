using Amazon.DynamoDBv2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KRT.infrastructure
{
    public static class DynamoDb
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(options);

            services.AddAWSService<IAmazonDynamoDB>();

            return services;
        }
    }
}
