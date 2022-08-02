using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
  public IConfiguration Configuration { get; private set; }
  protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile(Directory.GetCurrentDirectory()+"/integrationsettings.json")
                .Build();
 
            config.AddConfiguration(Configuration);
        });
    }
}