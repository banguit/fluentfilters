# Fluent Filters for ASP.NET Core

### Configuration:
After installing the package to your ASP.NET Core Web Application you should replace default filter provider by custom from library. Your Startup class should looks like shown below:
```csharp
// Startup.cs
using FluentFilters;
using FluentFilters.Criteria;

namespace DotNetCoreWebApp
{
  public class Startup
  {
    //...
    public void ConfigureServices(IServiceCollection services)
    {
      //...
      services.AddMvc(option =>
      {
        option.Filters.Add(new AddHeaderAttribute("Hello", "World"), c =>
        {
          // Example of using predefined FluentFilters criteria
          c.Require(new ActionFilterCriteria("About"))
            .Or(new ControllerFilterCriteria("Account"))
            .And(new ActionFilterCriteria("Login"));
        });
      });

      // Replace default filter provider by custom from FluentFilters library
      Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionExtensions.Replace(services, ServiceDescriptor.Singleton<IFilterProvider, FluentFilterFilterProvider>());
      //...
    }
    //...
  }
}
```

If you are looking for FluentFilters for ASP.NET MVC2/3, you can find them [here](http://fluentfilters.codeplex.com/)