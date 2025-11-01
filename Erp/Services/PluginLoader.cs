using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Erp.Services
{
    public class PluginLoader
    {
        public List<RouteEntry> LoadPluginRoutes(string pluginPath)
        {
            var assembly = Assembly.LoadFrom(pluginPath);
            var routes = new List<RouteEntry>();

            foreach (var type in assembly.GetTypes())
            {
                var attrs = type.GetCustomAttributes(typeof(RouteAttribute), true);
                foreach (RouteAttribute attr in attrs)
                {
                    routes.Add(new RouteEntry(attr.Template, type));
                }
            }

            return routes;
        }

    }

    public class RouteEntry
    {
        public string Template { get; set; }
        public Type ComponentType { get; set; }

        public RouteEntry(string template, Type component)
        {
            Template = template;
            ComponentType = component;
        }
    }
}
