[![Downloads](https://img.shields.io/nuget/dt/FluentFilters.svg)](https://www.nuget.org/packages/FluentFilters/)

# Fluent Filters for ASP.NET Core

ASP.NET Core have ability to register filters globally. It's works great, but sometimes it would be nice to specify conditions for filter execution and FluentFlters will help with this task.

### Install package
For ASP.NET Core Web Application you should use FluentFilter version 0.3.* and higher. Currently the latest version 0.3.0.
To install the latest package you can use Nuget Package Manager in Visual Studio or specify dependency in project.json file as shown below and call for package restore.

```javascript
{
    //...

    "dependencies": {

        //...
        "FluentFilters": "0.3.0"
    },

    //...
}    
```


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
      Microsoft.Extensions.DependencyInjection.Extensions.ServiceCollectionDescriptorExtensions.Replace(services, ServiceDescriptor.Singleton<IFilterProvider, FluentFilterFilterProvider>());
      //...
    }
    //...
  }
}
```

### Registering filters
To register filters with criteria, you need do it in usual way but calling extended methods Add or AddService. Below you can see signature of these methods.
```csharp
// Register filter by instance
void Add(this FilterCollection collection, IFilterMetadata filter, Action<IFilterCriteriaBuilder> criteria);

// Register filter by type
IFilterMetadata Add(this FilterCollection collection, Type filterType, Action<IFilterCriteriaBuilder> criteria)
IFilterMetadata Add(this FilterCollection collection, Type filterType, int order, Action<IFilterCriteriaBuilder> criteria)
IFilterMetadata AddService(this FilterCollection collection, Type filterType, Action<IFilterCriteriaBuilder> criteria)
IFilterMetadata AddService(this FilterCollection collection, Type filterType, int order, Action<IFilterCriteriaBuilder> criteria)
```


### Specify conditions
To specify the conditions, you should set the chain of criteria for the filter at registration. Using criteria, you can set whether to execute a filter or not. The library already provides three criteria for use:

 * **ActionFilterCriteria** - filter by specified action 
 * **AreaFilterCriteria** - filter by specified area
 * **ControllerFilterCriteria** - filter by specified controller

For one filter, you can only specify two chains of criteria. These are the chains of criteria that are required and which should be excluded.

```csharp
option.Filters.Add(typeof(CheckAuthenticationAttribute), c =>
{
    // Execute if current area "Blog"
    c.Require(new AreaFilterCriteria("Blog"));
    // But ignore if current controller "Account"
    c.Exclude(new ControllerFilterCriteria("Account"));
});
```

Chains of criteria are constructed by using the methods And(IFilterCriteria criteria) and Or(IFilterCriteria criteria), which work as conditional logical operators && and ||.

```csharp
option.Filters.Add(typeof(DisplayTopBannerFilterAttribute), c =>
{
    c.Require(new IsFreeAccountFilterCriteria())
        .Or(new AreaFilterCriteria("Blog"))
        .Or(new AreaFilterCriteria("Forum"))
            .And(new IsMemberFilterCriteria());

    c.Exclude(new AreaFilterCriteria("Administrator"))
        .Or(new ControllerFilterCriteria("Account"))
            .And(new ActionFilterCriteria("LogOn"));
});
```

If using the C# language, then the code above can be understood as (like pseudocode):
```csharp
if( IsFreeAccountFilterCriteria() || area == "Blog" || 
    (area == "Forum" && IsMemberFilterCriteria()) ) 
{
    if(area != "Administrator")
    {
        DisplayTopBannerFilter();
    }
    else if(controller != "Account" && action != "LogOn")
    {
        DisplayTopBannerFilter();
    }
}
```

### Implementation of custom criteria
To create a custom criterion you should inherit your class from the FluentFilters.IFilterCriteria interface and implement only one method Match with logic to making decision about filter execution. As example, look to the source code for ActionFilterCriteria: 
```csharp
public class ActionFilterCriteria : IFilterCriteria
{
    #region Fields

    private readonly string _actionName;

    #endregion

    #region Constructor

    /// <summary>
    /// Filter by specified action
    /// </summary>
    /// <param name="actionName">Name of the action</param>
    public ActionFilterCriteria(string actionName)
    {
        _actionName = actionName;
    }

    #endregion

    #region Implementation of IActionFilterCriteria

    public bool Match(FilterProviderContext context)
    {
        return string.Equals(_actionName, context.ActionContext.RouteData.GetRequiredString("action"), StringComparison.OrdinalIgnoreCase);
    }

    #endregion
}
```

`Note: If you are looking for FluentFilters for ASP.NET MVC2/3, you can find them [here](http://fluentfilters.codeplex.com/)`
