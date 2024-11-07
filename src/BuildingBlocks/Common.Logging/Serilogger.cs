using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog.Sinks.Elasticsearch;
using Serilog;

namespace Common.Logging
{
    public static class Serilogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
            (context, configuration) =>
            {
                var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
                var enviromentName = context.HostingEnvironment.EnvironmentName ?? "Development";
                var elasticUri = context.Configuration
                    .GetValue<string>("ElasticConfiguration:Uri");
                var userName = context.Configuration
                    .GetValue<string>("ElasticConfiguration:UserName");
                var password = context.Configuration
                    .GetValue<string>("ElasticConfiguration:Password");

                configuration
                    .WriteTo.Debug()
                    .WriteTo.Console(outputTemplate:
                        "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}"
                    )
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri ?? "https://localhost:9200"))
                    {
                        IndexFormat = $"aspnet_microservice_log-{applicationName}-" +
                        $"{enviromentName}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfReplicas = 1,
                        NumberOfShards = 2,
                        ModifyConnectionSettings = x => x.BasicAuthentication(userName, password)
                    })
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Enviroment", enviromentName)
                    .Enrich.WithProperty("Application", applicationName)
                    .ReadFrom.Configuration(context.Configuration);
            };
    }
}
